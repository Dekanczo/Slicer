using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommandLine;

namespace GcodeCreator
{
     class Options
    {
        [Option("savedFolderPath", Required = false, HelpText = "Папка, в которую будет производиться сохранение.")]
        public string SavedFolderPath { get; set; }

        [Option("modelName", Required = false, HelpText = "Название модели.")]
        public string modelName { get; set; }

        [Option("layerHeight", Required = true, HelpText = ".")]
        public double layerHeight { get; set; }


        [Option("temperatureSetpointTemperatures", Required = false, HelpText = ".")]
        public string temperatureSetpointTemperatures { get; set; }


        [Option("topSolidLayers", Required = false, HelpText = ".")]
        public string topSolidLayers { get; set; }


        [Option("bottomSolidLayers", Required = false, HelpText = ".")]
        public string bottomSolidLayers { get; set; }


        [Option("perimiterOutlines", Required = false, HelpText = ".")]
        public string perimiterOutlines { get; set; }


        [Option("M104", Required = false, HelpText = ".")]
        public string M104 { get; set; }


        [Option("M109", Required = false, HelpText = ".")]
        public string M109 { get; set; }


        [Option("M140", Required = false, HelpText = ".")]
        public string M140 { get; set; }


        [Option("M190", Required = false, HelpText = ".")]
        public string M190 { get; set; }

        [Option("scaleKoefficient", Required = true, HelpText = "Коэффициент увеличения модели.")]
        public float scaleKoefficient { get; set; }

        [Option("onlySettings", Required = true, HelpText = "Коэффициент увеличения модели.")]
        public int onlySettings { get; set; }

    }
}
