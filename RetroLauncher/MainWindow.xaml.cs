using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using BrandonPotter.XBox;
using System.Diagnostics;
using System.Windows.Interop;
using System.Runtime.InteropServices;
using System.Threading;

namespace RetroLauncher
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        [DllImport("user32.dll", SetLastError = true)]
        static extern IntPtr SetParent(IntPtr hWndChild, IntPtr hWndNewParent);

        public MainWindow()
        {
            InitializeComponent();

            CreateControllerInputs();
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            System.Environment.Exit(1);
        }

        private void Dreamcast_Click(object sender, RoutedEventArgs e)
        {
            var path = ConfigurationManager.AppSettings["dreamcastDirectory"];

            //spConsoleList.Visibility = Visibility.Collapsed;
            //spGameList.Visibility = Visibility.Visible;

            dgGameList.ItemsSource = new DirectoryInfo(path).GetFiles();
        }

        private void SNES_Click(object sender, RoutedEventArgs e)
        {
            var path = ConfigurationManager.AppSettings["snesDirectory"];

            //spConsoleList.Visibility = Visibility.Collapsed;
            //spGameList.Visibility = Visibility.Visible;

            dgGameList.ItemsSource = new DirectoryInfo(path).GetFiles();
        }

        private void N64_Click(object sender, RoutedEventArgs e)
        {
            var path = ConfigurationManager.AppSettings["n64Directory"];

            //spConsoleList.Visibility = Visibility.Collapsed;
            //spGameList.Visibility = Visibility.Visible;

            dgGameList.ItemsSource = new DirectoryInfo(path).GetFiles();
        }

        private void LaunchGame_Click(object sender, RoutedEventArgs e)
        {
            FileInfo fileInfo = new FileInfo(dgGameList.SelectedItem.ToString());
            string filePath = getConsolePath(fileInfo);

            filePath += dgGameList.SelectedItem.ToString();

            if (fileInfo.Extension == ".cdi")
            {
                var dreamcastIni = new IniFile(ConfigurationManager.AppSettings["dreamcastConfigDirectory"]);
                dreamcastIni.Write("DefaultImage", filePath, "ImageReader");
            }

            if (fileInfo.Extension == ".smc")
            {
                var snesIni = new IniFile(ConfigurationManager.AppSettings["snesConfigDirectory"]);
                snesIni.Write("Fullscreen:Enabled", "TRUE", "Display/Win");
            }

            if (fileInfo.Extension == ".n64" || fileInfo.Extension == ".z64" || fileInfo.Extension == ".v64")
            {
                var n64Ini = new IniFile(ConfigurationManager.AppSettings["n64ConfigDirectory"]);
                n64Ini.Write("", "", "");
            }
            
            Process emulator = new Process();

            emulator.StartInfo.FileName = filePath;
            emulator.Start();
        }
        
        private string getConsolePath(FileInfo dgitem)
        {
            string consolePath = "";

            if (dgitem.Extension == ".cdi")
            {
                consolePath = ConfigurationManager.AppSettings["dreamcastDirectory"];
            }
            else if (dgitem.Extension == ".smc")
            {
                consolePath = ConfigurationManager.AppSettings["snesDirectory"];
            }
            else if (dgitem.Extension == ".v64" || dgitem.Extension == ".z64")
            {
                consolePath = ConfigurationManager.AppSettings["n64Directory"];
            }

            return consolePath;
        }

        private void CreateControllerInputs()
        {
            var controller = XBoxController.GetConnectedControllers().FirstOrDefault();

            if (controller != null && controller.IsConnected)
            {
                if (controller.ButtonStartPressed)
                {
                    throw new NotImplementedException();
                }

                if (controller.ButtonAPressed)
                {
                    throw new NotImplementedException();
                }

                if (controller.ButtonBPressed)
                {
                    throw new NotImplementedException();
                }

                if (controller.ButtonUpPressed || controller.ThumbLeftY < 0)
                {
                    System.Windows.Forms.SendKeys.Send("UP");
                }

                if (controller.ButtonDownPressed || controller.ThumbLeftY > 0)
                {
                    System.Windows.Forms.SendKeys.Send("DOWN");
                }

                if (controller.ButtonLeftPressed || controller.ThumbLeftX < 0)
                {
                    System.Windows.Forms.SendKeys.Send("DOWN");
                }

                if (controller.ButtonRightPressed || controller.ThumbLeftX > 0)
                {
                    System.Windows.Forms.SendKeys.Send("RIGHT");
                }
            }
        }
    }
}
