using System;
using System.Collections.Generic;
using System.Linq;
using SkiaSharp;
using System.IO;

namespace GcodeCreator
{
    class Drawer
    {
        SKBitmap bitmap;
        SKCanvas canvas;
        SKImage image;
        SKData data;
        SKPath convex;

        string folderPath;
        int windowW, windowH;

        public Drawer(int windowW, int windowH, float shiftX, float shiftY, string folderPath)
        {
            this.windowW = windowW;
            this.windowH = windowH;
            this.folderPath = folderPath;

            bitmap = new SKBitmap(windowW, windowH);
            canvas = new SKCanvas(bitmap);
            canvas.Translate(shiftX, shiftY);

        }

        public void DrawSlice(List<SKPoint[]> conturs, List<int[]> intendStatus, int sliceNumber)
        {
            canvas.Clear(SKColors.Black);
            SKPaint paint = new SKPaint();
            
            for (int k = 0; k < conturs.Count(); k++)
            {
                convex = new SKPath();
                convex.AddPoly(conturs[intendStatus[k][0]], true);

                if (intendStatus[k][1] % 2 == 0)
                    paint.Color = SKColors.White;
                else
                    paint.Color = SKColors.Black;
                canvas.DrawPath(convex, paint);
            }

            image = SKImage.FromBitmap(bitmap);
            data = image.Encode(SKEncodedImageFormat.Png, 100);

            using (var stream = File.OpenWrite(Path.Combine(folderPath, $"{sliceNumber}.png")))
                data.SaveTo(stream);
        }
    }
}
