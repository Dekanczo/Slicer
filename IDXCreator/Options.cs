using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommandLine;

namespace IDXCreator
{
    //C:\Users\Denis\source\repos\SlicerSolutions\IDXCreator\bin\Debug\IDXCreator.exe --saved-folder-path C:\Users\Denis\source\repos\SlicerSolutions\Slicer\bin\Debug\test --blanking-layer-value ONE --layer-thickness TWO --lift-distance THREE --layer-time FOUR
    class Options
    {

        [Option("saved-folder-path", Required = false, HelpText = "Папку, в которую будет производиться сохранение.")]
        public string SavedFolderPath { get; set; }

        [Option("blanking-layer-value", Required = false, HelpText = "Время затухания.")]
        public string textBox_blankingLayer { get; set; }

        [Option("layer-thickness", Required = false, HelpText = "Толщина слоя.")]
        public string textBox_layerThickness { get; set; }

        [Option("lift-distance", Required = true, HelpText = "Шаг по оси z.")]
        public string textBox_liftDistance { get; set; }

        [Option("layer-time", Required = false, HelpText = "Время свечения на слой.")]
        public string textBox_layerTime { get; set; }

        [Option("scale-koefficient", Required = true, HelpText = "Коэффициент увеличения модели.")]
        public float scaleKoefficient { get; set; }
    }
}
