using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Ink;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace HermitePad.Tools
{
    public class FillTool
    {
        private readonly InkCanvas _inkCanvas;

        public FillTool(InkCanvas inkCanvas)
        {
            _inkCanvas = inkCanvas;
        }

        public void Fill(Point clickPoint, Color fillColor)
        {
            // Render the canvas to a bitmap to analyze pixels
            int width = (int)_inkCanvas.ActualWidth;
            int height = (int)_inkCanvas.ActualHeight;

            if (width <= 0 || height <= 0)
                return;

            // Create a render target bitmap
            RenderTargetBitmap rtb = new RenderTargetBitmap(
                width, height, 96, 96, PixelFormats.Pbgra32);
            rtb.Render(_inkCanvas);

            // Convert to writable bitmap for pixel manipulation
            WriteableBitmap writableBitmap = new WriteableBitmap(rtb);

            // Get the pixel data
            int stride = writableBitmap.BackBufferStride;
            int pixelCount = width * height;
            byte[] pixels = new byte[stride * height];
            writableBitmap.CopyPixels(pixels, stride, 0);

            // Get clicked pixel position
            int x = (int)clickPoint.X;
            int y = (int)clickPoint.Y;

            if (x < 0 || x >= width || y < 0 || y >= height)
                return;

            // Get the target color (color at clicked position)
            Color targetColor = GetPixelColor(pixels, x, y, width, stride);

            // Don't fill if clicking on the same color
            if (ColorsEqual(targetColor, fillColor))
                return;

            // Perform flood fill
            FloodFill(pixels, width, height, stride, x, y, targetColor, fillColor);

            // Create a new image with the filled pixels
            WriteableBitmap filledBitmap = new WriteableBitmap(width, height, 96, 96, 
                PixelFormats.Pbgra32, null);
            filledBitmap.WritePixels(new Int32Rect(0, 0, width, height), pixels, stride, 0);

            // Convert the filled bitmap to strokes by creating a visual representation
            // We'll create a rectangle shape with the filled image as background
            AddFilledRegionToCanvas(filledBitmap, fillColor);
        }

        private void FloodFill(byte[] pixels, int width, int height, int stride, 
            int startX, int startY, Color targetColor, Color fillColor)
        {
            Queue<Point> queue = new Queue<Point>();
            bool[,] visited = new bool[width, height];

            queue.Enqueue(new Point(startX, startY));
            visited[startX, startY] = true;

            byte fillR = fillColor.R;
            byte fillG = fillColor.G;
            byte fillB = fillColor.B;
            byte fillA = fillColor.A;

            while (queue.Count > 0)
            {
                Point p = queue.Dequeue();
                int x = (int)p.X;
                int y = (int)p.Y;

                // Set the pixel to fill color
                SetPixelColor(pixels, x, y, stride, fillR, fillG, fillB, fillA);

                // Check all 4 neighbors
                CheckAndEnqueue(queue, visited, pixels, width, height, stride, 
                    x + 1, y, targetColor);
                CheckAndEnqueue(queue, visited, pixels, width, height, stride, 
                    x - 1, y, targetColor);
                CheckAndEnqueue(queue, visited, pixels, width, height, stride, 
                    x, y + 1, targetColor);
                CheckAndEnqueue(queue, visited, pixels, width, height, stride, 
                    x, y - 1, targetColor);
            }
        }

        private void CheckAndEnqueue(Queue<Point> queue, bool[,] visited, byte[] pixels,
            int width, int height, int stride, int x, int y, Color targetColor)
        {
            if (x < 0 || x >= width || y < 0 || y >= height)
                return;

            if (visited[x, y])
                return;

            Color pixelColor = GetPixelColor(pixels, x, y, width, stride);
            if (ColorsEqual(pixelColor, targetColor))
            {
                visited[x, y] = true;
                queue.Enqueue(new Point(x, y));
            }
        }

        private Color GetPixelColor(byte[] pixels, int x, int y, int width, int stride)
        {
            int index = y * stride + x * 4;
            if (index + 3 >= pixels.Length)
                return Colors.Transparent;

            byte b = pixels[index];
            byte g = pixels[index + 1];
            byte r = pixels[index + 2];
            byte a = pixels[index + 3];

            return Color.FromArgb(a, r, g, b);
        }

        private void SetPixelColor(byte[] pixels, int x, int y, int stride, 
            byte r, byte g, byte b, byte a)
        {
            int index = y * stride + x * 4;
            if (index + 3 >= pixels.Length)
                return;

            pixels[index] = b;
            pixels[index + 1] = g;
            pixels[index + 2] = r;
            pixels[index + 3] = a;
        }

        private bool ColorsEqual(Color c1, Color c2)
        {
            // Allow small tolerance for anti-aliasing
            int tolerance = 30;
            return Math.Abs(c1.R - c2.R) <= tolerance &&
                   Math.Abs(c1.G - c2.G) <= tolerance &&
                   Math.Abs(c1.B - c2.B) <= tolerance &&
                   Math.Abs(c1.A - c2.A) <= tolerance;
        }

        private void AddFilledRegionToCanvas(WriteableBitmap bitmap, Color fillColor)
        {
            // Create a rectangle that covers the canvas with the filled bitmap as background
            // This is a simplified approach - in a production app, you might want to
            // convert filled regions to actual vector shapes or strokes
            
            // For now, we'll add a visual element to the canvas
            var image = new System.Windows.Controls.Image
            {
                Source = bitmap,
                Width = bitmap.PixelWidth,
                Height = bitmap.PixelHeight,
                Stretch = Stretch.None
            };

            Canvas.SetLeft(image, 0);
            Canvas.SetTop(image, 0);
            Canvas.SetZIndex(image, -1); // Place behind ink strokes

            _inkCanvas.Children.Insert(0, image);
        }
    }
}
