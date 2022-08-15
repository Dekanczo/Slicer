using CommandLine;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Globalization;

namespace IDXCreator
{

    class IDXCreator
    {

        public static void Main(string[] args)
        {
            var usCulture = new CultureInfo("en-US");
            Thread.CurrentThread.CurrentCulture = usCulture;
            Thread.CurrentThread.CurrentUICulture = usCulture;

            try
            {
                CommandLine.Parser.Default.ParseArguments<Options>(args)
                    .WithParsed(RunOptions)
                    .WithNotParsed(HandleParseError);
            }
            catch
            {
                Console.ReadLine();
            }
        }


        static void RunOptions(Options opts)
        {
            string folderPath = opts.SavedFolderPath; 
            File.WriteAllText("problem.txt", Path.Combine(folderPath, "conturs.descriptions"));
            string slicesDescription = File.ReadAllText(Path.Combine(folderPath, "conturs.descriptions"));

            List<List<SKPoint[]>> slices = new List<List<SKPoint[]>>();
            List<List<int[]>> intendStatuses = new List<List<int[]>>();
            float scaleKoefficient = opts.scaleKoefficient;

            using (var descFile = new StreamReader(Path.Combine(folderPath, "conturs.descriptions")))
            {
                string line;
                List<SKPoint[]> curSlice;
                List<SKPoint> curContur = new List<SKPoint>();

                while (!descFile.EndOfStream)
                {
                    line = descFile.ReadLine();

                    if (line == "DescriptionBegin")
                        continue;
                    else if (line == "DescriptionEnd")
                        break;
                    else if (line == "SliceBegin")
                    {
                        curSlice = new List<SKPoint[]>();
                        slices.Add(curSlice);
                        intendStatuses.Add(new List<int[]>());

                        while ((line = descFile.ReadLine()) != "SliceEnd")
                        {
                            if (line == "ConturBegin")
                            {
                                string[] intendStatusPair = descFile.ReadLine().Split(',');
                                intendStatuses.Last().Add(new int[]
                                {
                                    int.Parse(intendStatusPair[0]),
                                    int.Parse(intendStatusPair[1])
                                });

                                curContur.Clear();

                                while ((line = descFile.ReadLine()) != "ConturEnd")
                                {
                                    string[] coords = line.Split(' ');
                                    curContur.Add(new SKPoint(
                                        float.Parse(coords[0]) * scaleKoefficient,
                                        float.Parse(coords[1]) * scaleKoefficient
                                    ));
                                };

                                curSlice.Add(curContur.ToArray());
                            }
                        }
                    }
                    else
                        continue;
                }



                float 
                    minX = float.MaxValue, 
                    minY = float.MaxValue, 
                    maxX = float.MinValue, 
                    maxY = float.MinValue;

                for (int k = 0; k < slices.Count; k++)
                    for (int l = 0; l < slices[k].Count; l++)
                    {
                        minX = Math.Min(minX, slices[k][l].Min(e => e.X));
                        minY = Math.Min(minY, slices[k][l].Min(e => e.Y));
                        maxX = Math.Max(maxX, slices[k][l].Max(e => e.X)); 
                        maxY = Math.Max(maxY, slices[k][l].Max(e => e.Y));
                    }

                float
                    dx = minX + (maxX - minX) / 2.0f,
                    dy = minY + (maxY - minY) / 2.0f;

                int
                    windowW = 1024,
                    windowH = 768;
                float
                    shiftX = windowW / 2.0f - (maxX - minX) / 2.0f - minX, 
                    shiftY = windowH / 2.0f - (maxY - minY) / 2.0f - minY;
                Drawer drawer = new Drawer(windowW, windowH, shiftX, shiftY, folderPath);

                for (int k = 0; k < slices.Count; k++)
                    drawer.DrawSlice(slices[k], intendStatuses[k], k);
            }

            string idxData =
$@"[Version]
Ver = 0.5

[BuildData]
TotalLayer = 104

[Build and Slicing Parameters]
Pix per mm X = 9.30909
Pix per mm Y = 9.30909
X Resolution = 1024
Y Resolution = 768
Layer Thickness = {opts.textBox_layerThickness}
Layer Time = {opts.textBox_layerTime}
Bottom Layers Time = 5000
Number of Bottom Layers = 3
Blanking Layer Time = {opts.textBox_blankingLayer}
Build Direction = Bottom_Up
Lift Distance = {opts.textBox_liftDistance}
Slide / Tilt Value = 0
Anti Aliasing = False
Use Mainlift GCode Tab = False
Anti Aliasing Value = 1.5
Z Lift Feed Rate = 50.00000
Z Lift Retract Rate = 100.00000
Flip X = False
Flip Y = True

[Machine Configuration]
Platform X Size = 110
Platform Y Size = 82.5
Platform Z Size = 190
Max X Feedrate = 100
Max Y Feedrate = 100
Max Z Feedrate = 100
Machine Type = UV_DLP

[Preview]
PreviewDirection = Front
PreviewSizeX = 256
PreviewSizeY = 256

[PixelData]
{string.Join("\n", Enumerable.Range(0, slices.Count).Select(i => $"{i}.png"))}
";

            File.WriteAllText(Path.Combine(folderPath, "settings.idx"), idxData);
        }



        static void HandleParseError(IEnumerable<Error> errs)
        {
            //handle errors
        }
    }
}