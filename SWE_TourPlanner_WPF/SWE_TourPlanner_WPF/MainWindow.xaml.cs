using System;
using System.Threading.Tasks;
using System.Windows;

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
            string appDir = AppDomain.CurrentDomain.BaseDirectory;
            string filePath = System.IO.Path.Combine(appDir, ((ViewModel)DataContext).LeafletFilePath);
            webView.CoreWebView2.Navigate(filePath);
            webView.Visibility = Visibility.Visible;
        }
    }
}
