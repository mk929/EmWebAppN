using EmWebApp.Util;
using EmWebApp.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

using iTextSharp.text;
using iTextSharp.text.xml.simpleparser;
using iTextSharp.text.html.simpleparser;
using iTextSharp.text.pdf;
using System.Text;
using System.CodeDom.Compiler;

namespace EmWebApp.BLL
{
    public class ConfirmationLetterPdf
    {
        public static string GetPdfTemplateFileName(AppointmentType appointmentType, int stayType)
        {
            string pdfTemplateFileName = string.Empty; 
            if (appointmentType.Code == 1 && stayType == 5) // "WP (Domestic Worker)" passport renewal, todo: enums
            {
                pdfTemplateFileName = EmWebAppConfig.CnslrLtrPdfTmplPath + EmWebAppConfig.DomesticWorker_PassportRenew_TmplFileName;
            }
            else
            {
                pdfTemplateFileName = EmWebAppConfig.CnslrLtrPdfTmplPath + appointmentType.ConsularLtrPdfTmplFilename;
            }
            return pdfTemplateFileName;
        }
        public static MemoryStream GetAppointmentLetterStream(string pdfTemplate, ConsularApptVM consularAppt)
        {
            PdfReader pdfReader = new PdfReader(pdfTemplate);
            MemoryStream memStream = new MemoryStream();
            using (PdfStamper pdfStamper = new PdfStamper(pdfReader, memStream, '\0', true))
            {
                FillFormFields(pdfStamper, consularAppt);
                pdfStamper.Writer.CloseStream = false;
            }
            memStream.Position = 0;
            return memStream;
        }
        public static MemoryStream GetAppointmentLetterStream2(string pdfTemplate, ConsularApptVM consularAppt)
        {

            PdfReader pdfReader = new PdfReader(pdfTemplate);
            using (var tempFileColl = new TempFileCollection())
            {
                string tempFile = tempFileColl.AddExtension("pdf");
                using (PdfStamper pdfStamper = new PdfStamper(pdfReader, new FileStream(tempFile, FileMode.OpenOrCreate)))
                {
                    FillFormFields(pdfStamper, consularAppt);
                }
                using (FileStream fileStream = File.OpenRead(tempFile))
                {
                    MemoryStream memStream = new MemoryStream();
                    memStream.SetLength(fileStream.Length);
                    fileStream.Read(memStream.GetBuffer(), 0, (int)fileStream.Length);
                    memStream.Position = 0;
                    return memStream;
                }
            }
        }
        private static void FillFormFields(PdfStamper pdfStamper, ConsularApptVM consularAppt)
        {
            AcroFields pdfFormFields = pdfStamper.AcroFields;

            pdfFormFields.SetField("Name", consularAppt.Name);
            pdfFormFields.SetField("PassportNumber", consularAppt.PassportNumber);
            pdfFormFields.SetField("AppointmentDate", String.Format("{0:dd MMM, yyyy [dddd]}", consularAppt.AppointmentDate));
            pdfFormFields.SetField("QueueNumber", consularAppt.QueueNumber.ToString());
            AppointmentType appointmentType = ConsularAppointmentTypes.GetAppointmentType(consularAppt.AppointmentType);
            pdfFormFields.SetField("ServiceType", appointmentType.Description);
            pdfFormFields.SetField("Name2", consularAppt.Name);
            pdfFormFields.SetField("PassportNumber2", consularAppt.PassportNumber);
            pdfFormFields.SetField("PhoneNumber", consularAppt.ContactPhone);
            pdfFormFields.SetField("Email", consularAppt.ContactEmail);
            iTextSharp.text.Image txtImage = null;
            using (var memStream = Graphics.GenerateQrCodeStream(GetQrCodeString(consularAppt)))
            {
                memStream.Position = 0;
                txtImage = iTextSharp.text.Image.GetInstance(memStream);
            }

            var fp = pdfFormFields.GetFieldPositions("QRCode");
            float right = fp[0].position.Right;
            float left = fp[0].position.Left;
            float top = fp[0].position.Top;
            float bottom = fp[0].position.Bottom;

            txtImage.ScaleToFit(115, 115);
            txtImage.SetAbsolutePosition(left, bottom);

            int pageNum = 1;
            PdfContentByte contentByte = pdfStamper.GetOverContent(pageNum);
            contentByte.AddImage(txtImage);

            pdfStamper.FormFlattening = false;
        }

        public static string GetQrCodeString(ConsularApptVM consularApptVM)
        {
            return String.Format(@"{0}$${1}$${2}$${3}$${4}$${5}$$JOB",

                consularApptVM.Name,
                string.Format("{0:dd-MM-yyyy}", consularApptVM.DateOfBirth),
                consularApptVM.Gender == "F" ? "Female" : "Male",
                consularApptVM.PlaceOfBirth,
                consularApptVM.PassportNumber,
                string.Format("{0:dd-MM-yyyy}", consularApptVM.PassportIssuedDate)

                );
        }
    }
}