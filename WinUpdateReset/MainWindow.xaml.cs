using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Diagnostics;
using System.IO;

namespace WinUpdateReset
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private void Hyperlink_RequestNavigate(object sender, RequestNavigateEventArgs e)
        {
            // for .NET Core you need to add UseShellExecute = true
            // see https://docs.microsoft.com/dotnet/api/system.diagnostics.processstartinfo.useshellexecute#property-value
            Process.Start(new ProcessStartInfo(e.Uri.AbsoluteUri));
            e.Handled = true;
        }

        public MainWindow()
        {
            InitializeComponent();
            ComplitedLabel.Visibility = Visibility.Hidden;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            ComplitedLabel.Visibility = Visibility.Hidden;

            // Останавливаем службу Windows Update
            Process batch = new Process();
            batch.StartInfo.FileName = "net.exe";
            batch.StartInfo.Arguments = "stop wuauserv";
            batch.StartInfo.UseShellExecute = true;
            batch.Start();
            batch.WaitForExit();

            // Удаляем каталог с данным об обновлении
            try
            {
                DirectoryInfo di = new DirectoryInfo(@"c:\Windows\SoftwareDistribution");
                di.Delete(true);
            }
            catch
            { }
            finally
            { }

            // Запускаем службу Windows Update
            batch.StartInfo.FileName = "net.exe";
            batch.StartInfo.Arguments = "start wuauserv";
            batch.StartInfo.UseShellExecute = true;
            batch.Start();
            batch.WaitForExit();

            ComplitedLabel.Visibility = Visibility.Visible;

        }
    }
}
