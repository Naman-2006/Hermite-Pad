using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using PdfSharp.Pdf;
using PdfSharp.Drawing;

namespace HermitePad.Core
{
    public class PdfExporter
    {
        public void Export(InkCanvas inkCanvas, CanvasManager canvasManager, string filePath)
        {
            try
            {
                // Create PDF document
                PdfDocument document = new PdfDocument();
                document.Info.Title = "Hermite Pad Export";
                document.Info.Creator = "Hermite Pad";

                // Add a page
                PdfPage page = document.AddPage();
                page.Width = XUnit.FromPoint(inkCanvas.ActualWidth);
                page.Height = XUnit.FromPoint(inkCanvas.ActualHeight);

                // Get graphics object
                XGraphics gfx = XGraphics.FromPdfPage(page);

                // Render the InkCanvas to a bitmap
                RenderTargetBitmap renderBitmap = new RenderTargetBitmap(
                    (int)inkCanvas.ActualWidth,
                    (int)inkCanvas.ActualHeight,
                    96d, 96d,
                    PixelFormats.Pbgra32);
                
                renderBitmap.Render(inkCanvas);

                // Save bitmap to memory stream
                using (MemoryStream stream = new MemoryStream())
                {
                    BitmapEncoder encoder = new PngBitmapEncoder();
                    encoder.Frames.Add(BitmapFrame.Create(renderBitmap));
                    encoder.Save(stream);
                    stream.Position = 0;

                    // Create XImage from stream
                    XImage image = XImage.FromStream(stream);
                    
                    // Draw image on PDF
                    gfx.DrawImage(image, 0, 0, page.Width, page.Height);
                }

                // Add text overlay for math objects
                var mathObjects = canvasManager.GetMathObjects();
                XFont font = new XFont("Arial", 12);
                XBrush brush = XBrushes.Blue;

                foreach (var mathObj in mathObjects)
                {
                    if (!string.IsNullOrEmpty(mathObj.LaTeX))
                    {
                        gfx.DrawString(
                            mathObj.LaTeX,
                            font,
                            brush,
                            mathObj.Position.X,
                            mathObj.Position.Y);
                    }
                }

                // Save the document
                document.Save(filePath);
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to export PDF: {ex.Message}", ex);
            }
        }
    }
}
