using System.Windows;
using SWE_TourPlanner_WPF.ViewLayer;

namespace SWE_TourPlanner_WPF
{
    /// <summary>
    /// Interaktionslogik für CreateTour.xaml
    /// </summary>
    public partial class CreateTour : Window
    {
        public CreateTour(ViewModel sharedViewModel)
        {
            InitializeComponent();
            DataContext = sharedViewModel;
        }

        private void btn_CreateTour_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
