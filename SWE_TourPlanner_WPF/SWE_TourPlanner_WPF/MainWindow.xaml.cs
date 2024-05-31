using Microsoft.EntityFrameworkCore.Metadata;
using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Net.Http;
using System.Security.Policy;
using System.Text;
using System.Text.RegularExpressions;
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
using Microsoft.Web.WebView2.Core;
using Newtonsoft.Json;

namespace SWE_TourPlanner_WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            ((ViewModel)DataContext).UpdateMap = UpdateMapAsync;
        }

        private void btn_CreateTour_Click(object sender, RoutedEventArgs e)
        {
            CreateTour createTour = new CreateTour(((ViewModel)DataContext));
            createTour.Closed += CreateTour_Closed;
            this.IsEnabled = false;
            createTour.Show();
        }

        private void CreateTour_Closed(object sender, EventArgs e)
        {
            this.IsEnabled = true;
        }

        private async Task UpdateMapAsync()
        {
            webView.Visibility = Visibility.Hidden;
            await webView.EnsureCoreWebView2Async(null);
            // Assuming "example.html" is at the root of your project directory and set to "Copy to Output Directory"
            string appDir = AppDomain.CurrentDomain.BaseDirectory;
            string filePath = System.IO.Path.Combine(appDir, ViewModel.LeafletFilePath);
            webView.CoreWebView2.Navigate(filePath);
            webView.Visibility = Visibility.Visible;
        }
    }
}
