using Microsoft.Web.WebView2.WinForms;
using Newtonsoft.Json;
using SWE_TourPlanner_WPF.BusinessLayer;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace SWE_TourPlanner_WPF
{
    public class ViewModel : INotifyPropertyChanged
    {
        public const string DirectionsFilePath = "Resources/directions.js";
        public const string LeafletFilePath = "Resources/leaflet.html";

        public delegate Task UpdateMapDelegate();
        public UpdateMapDelegate UpdateMap { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        public ObservableCollection<Tour> Tours { get; set; }

        private Tour _selectedTour;
        public Tour SelectedTour
        {
            get { return _selectedTour; }
            set
            {
                if (_selectedTour != value)
                {
                    _selectedTour = value;
                    SelectedTourChanged();
                }
            }
        }

        public bool SelectedTourIsNotCalculated
        {
            get
            {
                return SelectedTour != null && SelectedTour.Id <= 0;
            }
        }

        private void SelectedTourChanged()
        {

            if (_selectedTour != null)
            {
                DrawRoute();
            }

            OnPropertyChanged(nameof(SelectedTour));
            OnPropertyChanged(nameof(IsTourSelected));
            OnPropertyChanged(nameof(SelectedTourIsNotCalculated));
        }

        public bool IsTourSelected
        {
            get { return SelectedTour != null && Tours.Contains(SelectedTour); }
        }

        private ICommandHandler _openCreateWindow;
        public ICommandHandler OpenCreateWindow
        {
            get
            {
                return _openCreateWindow ?? (_openCreateWindow = new ICommandHandler(() => SelectEmtyTour(), () => true));
            }
        }

        private ICommandHandler _deleteTourCommand;
        public ICommandHandler DeleteTourCommand
        {
            get
            {
                return _deleteTourCommand ?? (_deleteTourCommand = new ICommandHandler(() => DeleteTour(), () => IsTourSelected));
            }
        }

        private ICommandHandler _createTourCommand;
        public ICommandHandler CreateTourCommand
        {
            get
            {
                return _createTourCommand ?? (_createTourCommand = new ICommandHandler(() => AddTour(), () => SelectedTourIsNotEmpty()));
            }
        }

        private ICommandHandler _updateTourCommand;
        public ICommandHandler UpdateTourCommand
        {
            get
            {
                return _updateTourCommand ?? (_updateTourCommand = new ICommandHandler(() => UpdateTour(), () => IsTourSelected));
            }
        }

        private ICommandHandler _printTourReportCommand;
        public ICommandHandler PrintTourReportCommand
        {
            get
            {
                return _printTourReportCommand ?? (_printTourReportCommand = new ICommandHandler(() => PrintTourReport(), () => IsTourSelected));
            }
        }

        private ICommandHandler _reloadToursCommand;
        public ICommandHandler ReloadToursCommand
        {
            get
            {
                return _reloadToursCommand ?? (_reloadToursCommand = new ICommandHandler(() => ReloadTours(), () => true));
            }
        }

        public ViewModel() 
        {
            Tours = new ObservableCollection<Tour> { };
        }

        public bool SelectedTourIsNotEmpty()
        {
            return SelectedTour != null && SelectedTour.AreAllInputParamsSet();
        }

        public void SelectEmtyTour()
        {
            SelectedTour = new Tour();
        }

        public async Task AddTour()
        {
            try
            {
                SelectedTour = await IBusinessLayer.Instance.AddTour(SelectedTour);
                await ReloadTours();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
            }
        }

        public async Task UpdateTour()
        {
            try
            {
                SelectedTour = IBusinessLayer.Instance.UpdateTour(SelectedTour);
                await ReloadTours();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
            }
        }

        public async Task DeleteTour()
        {
            if (SelectedTour != null)
            {
                try
                {
                    SelectedTour = IBusinessLayer.Instance.RemoveTour(SelectedTour);
                    await ReloadTours();
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.ToString());
                }
            }
        }

        public async Task PrintTourReport()
        {
            if (SelectedTour != null)
            {
                try
                {
                    SelectedTour = IBusinessLayer.Instance.PrintReportPDF(SelectedTour);
                    await ReloadTours();
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.ToString());
                }
            }
        }

        public async Task ReloadTours()
        {
            try
            {
                Tour tour = null;
                Tours = new ObservableCollection<Tour>(await IBusinessLayer.Instance.GetAllTours());

                if (SelectedTour != null)
                {
                    tour = Tours.ToList().Find(t => t.Id == SelectedTour.Id);
                }

                if (SelectedTour == null || tour == null)
                {
                    tour = Tours.FirstOrDefault();
                }

                /*
                 * Loaded anyways
                if (tour != null)
                {
                    tour.TourLogs = IBusinessLayer.Instance.GetAllTourLogsOfTour(tour);
                }
                */
                
                SelectedTour = tour;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
            }
            finally
            {
                OnPropertyChanged(nameof(Tours));
                SelectedTourChanged();
            }
        }

        private void DrawRoute()
        {
            if (!String.IsNullOrEmpty(SelectedTour.OSMjson))
            {
                var directionsContent = $"var directions = {SelectedTour.OSMjson};";
                string appDir = AppDomain.CurrentDomain.BaseDirectory;
                string filePath = System.IO.Path.Combine(appDir, DirectionsFilePath);
                File.WriteAllText(filePath, directionsContent);
                UpdateMap();
            }
        }

        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
