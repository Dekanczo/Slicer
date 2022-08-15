using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommandLine;

namespace Slicer
{
    class Options
    {

        [Option("stl-file-path", Required = true, HelpText = "Путь до STL-модели.")]
        public string STLFilePath { get; set; }

        [Option("saved-folder-path", Required = true, HelpText = "Папку, в которую будет производиться сохранение.")]
        public string SavedFolderPath { get; set; }

        [Option("lift-distance", Required = true, HelpText = "Шаг по оси Z.")]
        public string LiftDistance { get; set; }

    }
}
