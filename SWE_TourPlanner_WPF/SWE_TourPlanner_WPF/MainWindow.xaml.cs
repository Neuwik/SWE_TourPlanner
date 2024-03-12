using System;
using System.Collections.Generic;
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

            for (int i = 1; i < 6; i++)
            {
                ((ViewModel)DataContext).TempTour = new Tour()
                {
                    Name = $"Name {i}",
                    Description = $"Desc {i}",
                    From = $"From {i}",
                    To = $"To {i}",
                    TransportType = ETransportType.Car,
                    Distance = i * 100,
                    Time = i * 30,
                    RouteInformation = $"Info {i}"
                };
                ((ViewModel)DataContext).AddTour();
            }
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
    }
}
