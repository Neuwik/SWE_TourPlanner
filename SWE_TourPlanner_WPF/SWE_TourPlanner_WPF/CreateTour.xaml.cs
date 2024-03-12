﻿using System;
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
using System.Windows.Shapes;

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
            ((ViewModel)DataContext).AddTour();
            this.Close();
        }
    }
}
