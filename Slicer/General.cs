using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using CommandLine;
using SkiaSharp;
using System.Globalization;

namespace Slicer
{
    
    class General
    {


        public static void Main(string[] args)
        {

            var usCulture = new CultureInfo("en-US");
            Thread.CurrentThread.CurrentCulture = usCulture;
            Thread.CurrentThread.CurrentUICulture = usCulture;

            CommandLine.Parser.Default.ParseArguments<Options>(args)
                .WithParsed(RunOptions)
                .WithNotParsed(HandleParseError);
        }

        static void RunOptions(Options opts)
        {
            string folderPath = opts.SavedFolderPath;
            double
                z1 = Int32.MaxValue,
                z2 = Int32.MinValue;
            List<List<Vertex>> conturs = null;
            int[][] intendStatus;
            string stlPathValue = opts.STLFilePath;

            Slicer slicer = new Slicer(stlPathValue);
            double h = Double.Parse(opts.LiftDistance);

            for (int k = 0; k < slicer.stl.getTriangles().Length * 3; k++)
            {
                int tempZ = (int)slicer.stl.GetVertex(k).Z;
                if (z1 > tempZ)
                    z1 = tempZ;
                if (z2 < tempZ)
                    z2 = tempZ;
            }
            var sliceWriter = new StreamWriter(Path.Combine(folderPath, "conturs.descriptions"));
            string 
                outFormat = "", 
                newOutBlock = "", 
                prevOutBlock = "";
                
            // Eval points by Z from min to max
            double zPointsLen = (z2 - z1) / h;
            
            if (zPointsLen - (int)zPointsLen > Math.Pow(10, -10))
                zPointsLen = (int)zPointsLen + 1;
            else
                zPointsLen = (int)zPointsLen;

            double[] zPoints = new double[(int)zPointsLen];
            zPoints[0] = z1 + 0.1;
            zPoints[(int) zPointsLen - 1] = z2 - 0.1;

            for (int i = 1; i < zPointsLen - 1; i++)
                zPoints[i] += zPoints[i - 1] + h;

            AddLine(ref outFormat, $"{h}");
            AddLine(ref outFormat, "DescriptionBegin");
            for (int i = 0; i < zPointsLen; i++)
            {
                try
                {
                    newOutBlock = "";
                    // Получение всех контуров для слоя
                    (conturs, intendStatus) = slicer.GetConturs(zPoints[i]);

                    if (conturs == null)
                        continue;

                    // Формирование формата STL 
                    AddLine(ref newOutBlock, "SliceBegin");
                    for (int k = 0; k < conturs.Count; k++)
                    {
                        AddLine(ref newOutBlock, "ConturBegin");
                        AddLine(ref newOutBlock, $"{intendStatus[k][0]},{intendStatus[k][1]}");

                        foreach (var point in conturs[k])
                            AddLine(ref newOutBlock, $"{point.X} {point.Y}");
                        AddLine(ref newOutBlock, "ConturEnd");
                    }
                    AddLine(ref newOutBlock, "SliceEnd");
                    sliceWriter.Write(newOutBlock);
                    prevOutBlock = newOutBlock;
                }
                catch
                {
                    sliceWriter.Write(prevOutBlock);
                }
            }

            sliceWriter.Close(); 
        }

        static void HandleParseError(IEnumerable<Error> errs)
        {
            //handle errors
        }

        public static void AddLine(ref string source, string input)
        {
            source += input + "\n";
        }

    }

}




