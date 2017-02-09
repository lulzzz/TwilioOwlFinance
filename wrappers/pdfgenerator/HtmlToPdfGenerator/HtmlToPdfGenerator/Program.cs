using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PdfSharp.Drawing;
using PdfSharp.Pdf;

namespace HtmlToPdfGenerator
{
    class Program
    {
        static void Main(string[] args)
        {

            PdfDocument document = new PdfDocument();

            // Create an empty page
            PdfPage page = document.AddPage();

            // Get an XGraphics object for drawing
            XGraphics gfx = XGraphics.FromPdfPage(page);

            // Create a font
            XFont font = new XFont("Verdana", 20, XFontStyle.Bold);

            // Draw the text
            gfx.DrawString("Hello, World!", font, XBrushes.Black, 10, 50);


            string outFile = Path.Combine(@"c:\temp\", "RenderedPage2.pdf");

            // Save the document...
            document.Save(outFile);


            HtmlToPdf();
        }

        private static void HtmlToPdf()
        {
            //PdfConverter pdfConverter = new PdfConverter();
            ////pdfConverter.LicenseKey = "put your license key here";
            //pdfConverter.PdfDocumentOptions.EmbedFonts = false;
            //pdfConverter.PdfDocumentOptions.ShowFooter = false;
            //pdfConverter.PdfDocumentOptions.ShowHeader = false;
            //pdfConverter.PdfDocumentOptions.GenerateSelectablePdf = true;
            //string outFile = Path.Combine(@"c:\temp\", "RenderedPage1.pdf");
            //string url = "http://google.com";
            //string htmlString =
            //    "<html><body margin-top=><h1>I Al Cook hereby agree to the removal of the fraudulent transaction Filled Gas for the amount of $4.78</h1></body></html>";
            //try
            //{
            //    pdfConverter.SavePdfFromHtmlStringToFile(htmlString, outFile);
            //    //pdfConverter.SavePdfFromUrlToFile(url, outFile);
            //}
            //catch (Exception ex)
            //{
            //    Console.WriteLine(ex.Message);
            //    return;
            //}
        }
    }
}
