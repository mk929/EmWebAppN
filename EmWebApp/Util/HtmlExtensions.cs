using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebGrease.Css.Extensions;
using ZXing;
using ZXing.Common;

namespace EmWebApp.Util
{
    public static class Graphics
    {
        public static IHtmlString GetImgSrcForPdf(string fileName)
        {
            using (var stream = new MemoryStream())
            {
                System.Drawing.Bitmap bitMap = new System.Drawing.Bitmap(fileName);
                bitMap.Save(stream, ImageFormat.Png);

                var img = new TagBuilder("img");
                img.MergeAttribute("alt", "");
                img.Attributes.Add("src", String.Format(@"data:image/png;base64,{0}",
                    Convert.ToBase64String(stream.ToArray())));

                return MvcHtmlString.Create(img.ToString(TagRenderMode.SelfClosing));
            }
        }
        public static IHtmlString GetImgTag(string src, string alt)
        {
            var img = new TagBuilder("img");
            img.MergeAttribute("alt", alt);
            img.Attributes.Add("src", src);
            return MvcHtmlString.Create(img.ToString(TagRenderMode.SelfClosing));

        }
        public static MemoryStream GenerateQrCodeStream(string qrContent, int height = 177, int width = 177, int margin = 0)
        {
            var barcodeWriter = new BarcodeWriter
            {
                Format = BarcodeFormat.QR_CODE,
                Options = new EncodingOptions { Height = height, Width = width, Margin = margin }
            };

            using (var bitmap = barcodeWriter.Write(qrContent))
            {
                var stream = new MemoryStream();
                bitmap.Save(stream, ImageFormat.Png);
                stream.Position = 0;
                return stream;
            }
        }
        public static IHtmlString GenerateRelayQrCode(string qrContent,
            int height = 177, int width = 177, int margin = 0)
        {
            var barcodeWriter = new BarcodeWriter
            {
                Format = BarcodeFormat.QR_CODE,
                Options = new EncodingOptions { Height = height, Width = width, Margin = margin }
            };

            using (var bitmap = barcodeWriter.Write(qrContent))
            using (var stream = new MemoryStream())
            {
                bitmap.Save(stream, ImageFormat.Png);

                var img = new TagBuilder("img");
                img.MergeAttribute("alt", "");
                img.Attributes.Add("src", String.Format(@"data:image/png;base64,{0}",
                    Convert.ToBase64String(stream.ToArray())));

                return MvcHtmlString.Create(img.ToString(TagRenderMode.SelfClosing));
            }
        }
    }

    public class BootstrapHtml
    {
        public static MvcHtmlString Dropdown(string id, List<SelectListItem> selectListItems, string label)
        {
            var button = new TagBuilder("button")
            {
                Attributes =
            {
                {"id", id},
                {"type", "button"},
                {"data-toggle", "dropdown"}
            }
            };

            button.AddCssClass("btn");
            button.AddCssClass("btn-default");
            button.AddCssClass("dropdown-toggle");

            button.SetInnerText(label);
            button.InnerHtml += " " + BuildCaret();

            var wrapper = new TagBuilder("div");
            wrapper.AddCssClass("dropdown");

            wrapper.InnerHtml += button;
            wrapper.InnerHtml += BuildDropdown(id, selectListItems);

            return new MvcHtmlString(wrapper.ToString());
        }

        private static string BuildCaret()
        {
            var caret = new TagBuilder("span");
            caret.AddCssClass("caret");

            return caret.ToString();
        }

        private static string BuildDropdown(string id, IEnumerable<SelectListItem> items)
        {
            var list = new TagBuilder("ul")
            {
                Attributes =
            {
                {"class", "dropdown-menu"},
                {"role", "menu"},
                {"aria-labelledby", id}
            }
            };

            var listItem = new TagBuilder("li");
            listItem.Attributes.Add("role", "presentation");

            items.ForEach(x => list.InnerHtml += "<li role=\"presentation\">" + BuildListRow(x) + "</li>");

            return list.ToString();
        }

        private static string BuildListRow(SelectListItem item)
        {
            var anchor = new TagBuilder("a")
            {
                Attributes =
            {
                {"role", "menuitem"},
                {"tabindex", "-1"},
                {"href", item.Value}
            }
            };

            anchor.SetInnerText(item.Text);

            return anchor.ToString();
        }
    }

    /*
        @using(Html.BeginForm("", "", FormMethod.Post))
        {

            var items = new List<SelectListItem>()
            {
                new SelectListItem() { Text = "Item 1", Value = "#" },
                new SelectListItem() { Text = "Item 2", Value = "#" },
            };

            <div class="form-group">
                @Html.Label("Before", new { @class = "col-md-2 control-label" })
                <div class="col-md-10">
                    @Html.DropDownList("Name", items, "Dropdown", new { @class = "form-control"})
                </div>
            </div>

            <br/>
            <br/>
            <br/>

            <div class="form-group">
                @Html.Label("After", new { @class = "col-md-2 control-label" })
                <div class="col-md-10">
                    @BootstrapHtml.Dropdown("dropdownMenu1", items, "Dropdown")
                </div>
            </div>
        }
    */
}