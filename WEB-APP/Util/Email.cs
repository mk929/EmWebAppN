using SelectPdf;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using System.Threading.Tasks;
using System.Web;
using System.Xml.Serialization;

namespace EmWebApp.Util
{
    public static class SmtpClientExtensions
    {
        public static void Prep(this SmtpClient smtpClient)
        {
            smtpClient.SendCompleted += (s, e) =>
            {
                SmtpClient sc = s as SmtpClient;
                AsyncCompletedEventArgs ae = e as AsyncCompletedEventArgs;
                if (ae.Cancelled)
                {
                    // log("[{0}] Send canceled.", ae.UserState);
                }
                if (ae.Error != null)
                {
                    // log("[{0}] Send canceled.", ae.UserState);
                }
            };

            //    smtpClient.Host = "smtp.aol.com"; // "smtp-mail.outlook.com";
            //    smtpClient.Port = 587; //  587;
            //    smtpClient.EnableSsl = true; // true; TLS too
            //    smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network; // true;
            
            smtpClient.UseDefaultCredentials = false;
            smtpClient.Credentials = new NetworkCredential(
                ConfigurationManager.AppSettings["EmailAddress"],
                ConfigurationManager.AppSettings["EmailPassword"]);
                
        }
    }

    public static class Pdf
    {
        public static void WriteHtmlToPdfStream(string htmlContent, MemoryStream pdfStream )
        {
            try
            {
                HtmlToPdf converter = new HtmlToPdf();
                PdfDocument pdfDocument = converter.ConvertHtmlString(htmlContent);
                pdfDocument.Save(pdfStream);
                pdfDocument.Close();
                // reset stream position
                pdfStream.Position = 0;
            }
            catch (Exception ex)
            {
                // log ("An error occurred from Pdf converter: " + ex.Message);
                throw ex;
            }
        }
    }

    public class Email
    {
        public string From { get; set; }
        public string DisplayName { get; set; }
        public string To { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public bool IsHtml { get; set; }

        public MailMessage Message
        {
            get
            {
                MailAddress from = new MailAddress(From, DisplayName, System.Text.Encoding.UTF8);
                MailAddress to = new MailAddress(To);

                MailMessage message = new MailMessage(from, to);
                message.Body = Body + Environment.NewLine;
                message.BodyEncoding = System.Text.Encoding.UTF8;
                message.Subject = Subject;
                message.SubjectEncoding = System.Text.Encoding.UTF8;
                message.IsBodyHtml = IsHtml;

                return message;
            }
        }

        public static Attachment GetAttachmentFromString(string fileName, string content)
        {
            using (MemoryStream contentStream = new System.IO.MemoryStream())
            using (System.IO.StreamWriter writer = new System.IO.StreamWriter(contentStream))
            {
                writer.Write(content);
                writer.Flush();
                writer.Close();
                // reset stream position
                contentStream.Position = 0;

                ContentType contentType = new ContentType(MediaTypeNames.Text.Plain);
                Attachment attachment = new Attachment(contentStream, contentType);
                attachment.ContentDisposition.FileName = fileName;
                contentStream.Close();
                return attachment;
            }
        }
        
        public static Attachment GetPdfAttachmentFromHtmlString(string htmlContent, MemoryStream pdfStream, string pdfFileName)
        {
            Pdf.WriteHtmlToPdfStream(htmlContent, pdfStream);
            return GetPdfAttachmentFromPdfStream(pdfStream, pdfFileName);
        }

        public static Attachment GetPdfAttachmentFromPdfStream(MemoryStream pdfStream, string pdfFileName)
        {
            ContentType contentType = new ContentType(MediaTypeNames.Application.Pdf);
            Attachment attachment = new Attachment(pdfStream, contentType);
            attachment.ContentDisposition.FileName = pdfFileName;
            return attachment;
        }
    }

    public class XmlUtil
    {
        public static T DeserializeXMLFileToObject<T>(string XmlFilename)
        {
            T returnObject = default(T);
            if (string.IsNullOrEmpty(XmlFilename)) return default(T);

            try
            {
                using (StreamReader xmlStream = new StreamReader(XmlFilename))
                {
                    XmlSerializer serializer = new XmlSerializer(typeof(T));
                    returnObject = (T)serializer.Deserialize(xmlStream);
                    xmlStream.Close();
                }
            }
            catch (Exception ex)
            {
                ExceptionLogger.WriteExceptionToConsole(ex, DateTime.Now);
            }
            return returnObject;
        }
    }
}