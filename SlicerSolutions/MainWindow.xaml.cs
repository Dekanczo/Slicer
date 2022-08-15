using Microsoft.Win32;
using System.Windows;
using Microsoft.WindowsAPICodePack.Dialogs;
using System.Linq;
using System.Diagnostics;
using System;
using System.IO;
using System.Threading;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace SlicerSolutions
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public string STLFilePath { get; set; }
        public string SavedFolderPath { get; set; }

        readonly string root = System.IO.Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName;

        public MainWindow()
        {
            InitializeComponent();
            idx_radioButton.IsChecked = true;
        }

        private void STLChoose(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new Microsoft.Win32.OpenFileDialog();
            if (openFileDialog.ShowDialog() == true)
            {
                STLFilePath = openFileDialog.FileName;
                textBox_stlPath.Text = openFileDialog.FileName;
            }
        }

        private void ChooseFolder(object sender, RoutedEventArgs e)
        {
            string currentDirectory = @"C:\";
            var dlg = new CommonOpenFileDialog
            {
                Title = "My Title",
                IsFolderPicker = true,
                InitialDirectory = currentDirectory,

                AddToMostRecentlyUsedList = false,
                AllowNonFileSystemItems = false,
                DefaultDirectory = currentDirectory,
                EnsureFileExists = true,
                EnsurePathExists = true,
                EnsureReadOnly = false,
                EnsureValidNames = true,
                Multiselect = false,
                ShowPlacesList = true
            };

            if (dlg.ShowDialog() == CommonFileDialogResult.Ok)
            {
                SavedFolderPath = dlg.FileName;
                textBox_savedFolderPath.Text = dlg.FileName;
            }
        }

        private void GenerateIDXData(object sender, RoutedEventArgs e)
        {
            try
            {
                string programName = Path.Combine(root, @"IDXCreator\bin\Debug\IDXCreator.exe");
                string strCmdText = buildCommand(new string[] {
                    $@"--saved-folder-path " + "\"" + $@"{SavedFolderPath}" + "\"",
                    $"--blanking-layer-value {textBox_blankingLayer.Text}",
                    $"--layer-thickness {textBox_layerThickness.Text}",
                    $"--lift-distance {textBox_liftDistance.Text}",
                    $"--layer-time {textBox_layerTime.Text}",
                    $"--scale-koefficient {textBox_scaleKoefficient.Text}"
                });

                File.AppendAllText(Path.Combine(SavedFolderPath, "problems.txt").ToString(), strCmdText.ToString());

                Process p = new Process();
                p.StartInfo.FileName = programName;
                p.StartInfo.Arguments = strCmdText;
                p.Start();

                p.EnableRaisingEvents = true;
                string path = Path.Combine(SavedFolderPath, "settings.idx");
                string output = "";
                while ((output = IsFileReady(path)).Length == 0)
                {
                    Task.Delay(500);

                }
                

                setText(output);
            }
            catch (Exception exception)
            {
                File.AppendAllText(Path.Combine(SavedFolderPath, "problems.txt").ToString(), exception.Message);
                Console.WriteLine("Что-то пошло не так. Проверьте, что на вход программе все данные поданы верно.");
                Console.WriteLine(exception.Message);
            }
        }

        private void GenerateGcodeData(object sender, RoutedEventArgs e)
        {
            try
            {
                string programName = Path.Combine(root, @"GcodeCreator\bin\Debug\GcodeCreator.exe");
                
                string strCmdText = buildCommand(new string[] {
                    $@"--savedFolderPath " + "\"" + $@"{SavedFolderPath}" + "\"",
                    $@"--layerHeight" + "\"" + $@"{textBox_layerHeight.Text}" + "\"",
                    $@"--temperatureSetpointTemperatures" + "\"" + $@"{textBox_temperatureSetpointTemperatures.Text}" + "\"",
                    $@"--topSolidLayers" + "\"" + $@"{textBox_topSolidLayers.Text}" + "\"",
                    $@"--bottomSolidLayers" + "\"" + $@"{textBox_bottomSolidLayers.Text}" + "\"",
                    $@"--perimiterOutlines" + "\"" + $@"{textBox_perimiterOutlines.Text}" + "\"",
                    $@"--M104" + "\"" + $@"{textBox_M104.Text}" + "\"",
                    $@"--M109" + "\"" + $@"{textBox_M109.Text}" + "\"",
                    $@"--M140" + "\"" + $@"{textBox_M140.Text}" + "\"",
                    $@"--M190" + "\"" + $@"{textBox_M190.Text}" + "\"",
                    $"--scaleKoefficient {textBox_scaleKoefficient}"
                });
                
                Process p = new Process();
                p.StartInfo.FileName = programName;
                p.StartInfo.Arguments = strCmdText;
                p.Start();

                p.EnableRaisingEvents = true;
                string path = Path.Combine(SavedFolderPath, "settings.gcode");
                string output = "";
                while ((output = IsFileReady(path)).Length == 0)
                {
                    Task.Delay(500);

                }


                setText(output);
            }
            catch (Exception exception)
            {
                Console.WriteLine("Что-то пошло не так. Проверьте, что на вход программе все данные поданы верно.");
                Console.WriteLine(exception.Message);
            }
        }
        private void P_Exited(object sender, EventArgs e)
        {
            
        }

        void setText(string output)
        {
            IdxSettings.Text = output;
        }

        public static string IsFileReady(string filename)
        {
            // If the file can be opened for exclusive access it means that the file
            // is no longer locked by another process.
            try
            {
                return File.ReadAllText(filename);
            }
            catch (Exception)
            {
                return "";
            }
        }

        private void EvaluteSlices(object sender, RoutedEventArgs e)
        {
            try
            {
                double layerHeight;
                if (gcode_radioButton.IsChecked == true)
                    layerHeight = Double.Parse(textBox_layerHeight.Text);
                else
                    layerHeight = Double.Parse(textBox_liftDistance.Text);

                string programName = Path.Combine(root, @"Slicer\bin\Debug\Slicer.exe");
                string strCmdText = buildCommand(new string[] {
                "--stl-file-path " + "\"" + $@"{STLFilePath}" + "\"",
                $@"--saved-folder-path " + "\"" + $@"{SavedFolderPath}" + "\"",
                $"--lift-distance {layerHeight}"
            });

                Process p = new Process();
                p.StartInfo.FileName = programName;
                p.StartInfo.Arguments = strCmdText;
                p.StartInfo.CreateNoWindow = true;
                p.Start();
            }
            catch (Exception exception)
            {
                Console.WriteLine("Что-то пошло не так. Проверьте, что на вход программе все данные поданы верно.");
                Console.WriteLine(exception.Message);
            }
        }

        string buildCommand(in string[][] elements)
        {
            return
                string.Join(
                    " ",
                    elements.Select(row => string.Join(" ", row))
                );
        }

        string buildCommand(in string[] elements)
        {
            return
                string.Join(
                    " ",
                    elements.Select(row => string.Join(" ", row))
                );
        }

        private void gcode_radioButton_Checked(object sender, RoutedEventArgs e)
        {
            groupBox_idx.Visibility = Visibility.Hidden;
            groupBox_gcode.Visibility = Visibility.Visible;
        }

        private void idx_radioButton_Checked(object sender, RoutedEventArgs e)
        {

            groupBox_idx.Visibility = Visibility.Visible;
            groupBox_gcode.Visibility = Visibility.Hidden;
        }
    }
}
