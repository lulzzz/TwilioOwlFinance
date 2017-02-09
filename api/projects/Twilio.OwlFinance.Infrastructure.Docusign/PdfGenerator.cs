using System;
using PdfSharp.Drawing;
using PdfSharp.Drawing.Layout;
using PdfSharp.Pdf;

namespace Twilio.OwlFinance.Infrastructure.Docusign
{
    public class PdfGenerator
    {
        public string GenerateDocument(string fullName, string description, string amount, string serverPath)
        {
            string documentMessage = "I {0} hereby agree to the removal of the fraudulent transaction {1} for the amount of {2}";
            string parsedMessage = string.Format(documentMessage, fullName, description, amount);

            var generatedPdfFilePath = serverPath + "/documents/generated/" + Guid.NewGuid() + ".pdf";

            PdfDocument document = new PdfDocument();

            PdfPage page = document.AddPage();
            XGraphics gfx = XGraphics.FromPdfPage(page);
            XFont font = new XFont("Verdana", 20, XFontStyle.Bold);
            XTextFormatter tf = new XTextFormatter(gfx);

            XRect rect = new XRect(40, 60, 550, 300);
            gfx.DrawRectangle(XBrushes.White, rect);

            tf.DrawString(parsedMessage, font, XBrushes.Black, rect, XStringFormats.TopLeft);
            gfx.DrawString("Signed", font, XBrushes.Black, 40, 175);
            document.Save(generatedPdfFilePath);

            return generatedPdfFilePath;
        }
    }
}
