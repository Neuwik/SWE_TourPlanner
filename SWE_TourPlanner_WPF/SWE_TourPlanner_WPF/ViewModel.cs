using Microsoft.Web.WebView2.WinForms;
using Newtonsoft.Json;
using SWE_TourPlanner_WPF.BusinessLayer;
using SWE_TourPlanner_WPF.MapHelpers;
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
        private const string ApiKey = "5b3ce3597851110001cf62483a397f95f86441adb7cbf0789ae0d615";
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

                    if (_selectedTour != null)
                    {
                        GetRoute();
                    }

                    OnPropertyChanged(nameof(SelectedTour));
                    OnPropertyChanged(nameof(IsTourSelected));
                }
            }
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
            //SeedData();
            ReloadTours();
        }

        public bool SelectedTourIsNotEmpty()
        {
            return SelectedTour != null && SelectedTour.AreAllParamsSet();
        }

        public void SelectEmtyTour()
        {
            SelectedTour = new Tour();
        }

        public void AddTour()
        {
            //Needs to be in Business Layer
            SelectedTour.ImagePath = "/route.png";

            try
            {
                SelectedTour = IBusinessLayer.Instance.AddTour(SelectedTour);
                Tours.Add(SelectedTour);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
            }
            finally
            {
                OnPropertyChanged(nameof(Tours));
            }
        }

        public void UpdateTour()
        {
            try
            {
                SelectedTour = IBusinessLayer.Instance.UpdateTour(SelectedTour);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
            }
            finally
            {
                OnPropertyChanged(nameof(Tours));
            }
        }

        public void DeleteTour()
        {
            if (SelectedTour != null)
            {
                try
                {
                    SelectedTour = IBusinessLayer.Instance.RemoveTour(SelectedTour);
                    Tours.Remove(SelectedTour);
                    SelectedTour = null;
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.ToString());
                }
                finally
                {
                    OnPropertyChanged(nameof(Tours));
                }
            }
        }

        public void ReloadTours()
        {
            try
            {
                Tours = new ObservableCollection<Tour>(IBusinessLayer.Instance.GetAllTours());

                if (SelectedTour != null)
                {
                    SelectedTour = Tours.ToList().Find(t => t == SelectedTour);
                }

                if (SelectedTour == null)
                {
                    SelectedTour = Tours.FirstOrDefault();
                }

                if (SelectedTour != null)
                {
                    SelectedTour.TourLogs = IBusinessLayer.Instance.GetAllToursLogOfTour(SelectedTour);
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
            }
            finally
            {
                OnPropertyChanged(nameof(Tours));
            }
        }

        private async void GetRoute()
        {
            Tour currentTour = new Tour(SelectedTour);
            if (currentTour != null && !String.IsNullOrEmpty(currentTour.From) && !String.IsNullOrEmpty(currentTour.To))
            {
                var json = await new OpenRouteService(ApiKey).GetDirectionsAsync(currentTour.From, currentTour.To, currentTour.TransportType);

                try
                {
                    var directionsContent = $"var directions = {json};";
                    string appDir = AppDomain.CurrentDomain.BaseDirectory;
                    string filePath = System.IO.Path.Combine(appDir, DirectionsFilePath);
                    File.WriteAllText(filePath, directionsContent);
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.ToString());
                }

                var directionsResult = JsonConvert.DeserializeObject<DirectionsResult>(json);
                if (directionsResult != null && directionsResult.Features != null && directionsResult.Features.Count > 0)
                {
                    // Access the first feature
                    var firstFeature = directionsResult.Features[0];

                    // Access the geometry coordinates
                    var coordinates = firstFeature.Geometry.Coordinates;

                    var summary = firstFeature.Properties.Summary;
                    // Format the message
                    string message = $"Distance: {summary.Distance} meters\nDuration: {summary.Duration} seconds";
                    // Show the message box
                    if (currentTour.Id == SelectedTour.Id && UpdateMap != null)
                    {
                        MessageBox.Show(message);
                        await UpdateMap();
                    }

                    // Construct the URL for the map tiles
                    /*MapImageUrl = await ConstructMapUrl(SelectedTour.From, SelectedTour.To);
                    SelectedTour.Img = MapImageUrl;*/
                }
            }
        }

        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
