using Newtonsoft.Json;
using SWE_TourPlanner_WPF.BusinessLayer;
using SWE_TourPlanner_WPF.Models;
using SWE_TourPlanner_WPF.ViewLayer;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows;

namespace SWE_TourPlanner_WPF.ViewLayer
{
    public class ViewModel : INotifyPropertyChanged
    {
        public readonly string DirectionsFilePath;
        public readonly string LeafletFilePath;

        public delegate Task UpdateMapDelegate();
        public UpdateMapDelegate UpdateMap { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        public ObservableCollection<Tour> AllTours { get; set; }

        private string _searchFilter;
        public string SearchFilter
        {
            get { return _searchFilter; }
            set
            {
                _searchFilter = value;
                OnPropertyChanged(nameof(SearchFilter));
                OnPropertyChanged(nameof(FilteredTours));
            }
        }

        public ObservableCollection<Tour> FilteredTours
        {
            get
            {
                return new ObservableCollection<Tour>(AllTours.Where(t => t.ContainsFilter(SearchFilter)).OrderByDescending(t => t.Popularity));
            }
        }

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
            get { return SelectedTour != null && AllTours.Contains(SelectedTour); }
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

        private ICommandHandler _printTourSummarizedReportCommand;
        public ICommandHandler PrintTourSummarizedReportCommand
        {
            get
            {
                return _printTourSummarizedReportCommand ?? (_printTourSummarizedReportCommand = new ICommandHandler(() => PrintTourSummarizedReport(), () => IsTourSelected));
            }
        }

        private ICommandHandler _exportTourToJsonCommand;
        public ICommandHandler ExportTourToJsonCommand
        {
            get
            {
                return _exportTourToJsonCommand ?? (_exportTourToJsonCommand = new ICommandHandler(() => ExportTourToJson(), () => IsTourSelected));
            }
        }

        private ICommandHandler _importToursFromJsonCommand;
        public ICommandHandler ImportToursFromJsonCommand
        {
            get
            {
                return _importToursFromJsonCommand ?? (_importToursFromJsonCommand = new ICommandHandler(() => ImportToursFromJson(), () => true));
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
            try
            {
                string json = File.ReadAllText(TourPlannerConfig.ConfigFile);
                TourPlannerConfig config = JsonConvert.DeserializeObject<TourPlannerConfig>(json);
                DirectionsFilePath = config.ViewModel.DirectionsFilePath;
                LeafletFilePath = config.ViewModel.LeafletFilePath;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                throw new Exception("ViewModel could not read config file.");
            }

            AllTours = new ObservableCollection<Tour> { };

            ReloadTours();
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
                MessageBox.Show($"Tour {SelectedTour.Name} has been added.");
                ReloadTours();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
            }
        }

        public void UpdateTour()
        {
            try
            {
                SelectedTour = IBusinessLayer.Instance.UpdateTour(SelectedTour);
                MessageBox.Show($"Tour {SelectedTour.Name} has been updated.");
                ReloadTours();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
            }
        }

        public void DeleteTour()
        {
            if (SelectedTour != null)
            {
                try
                {
                    SelectedTour = IBusinessLayer.Instance.RemoveTour(SelectedTour);
                    MessageBox.Show($"Tour {SelectedTour.Name} has been deleted.");
                    ReloadTours();
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.ToString());
                }
            }
        }

        public void PrintTourReport()
        {
            if (SelectedTour != null)
            {
                try
                {
                    SelectedTour = IBusinessLayer.Instance.PrintReportPDF(SelectedTour);
                    MessageBox.Show($"Tour {SelectedTour.Name}: Report has been printed.");
                    ReloadTours();
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.ToString());
                }
            }
        }

        public void PrintTourSummarizedReport()
        {
            if (SelectedTour != null)
            {
                try
                {
                    SelectedTour = IBusinessLayer.Instance.PrintSummarizedReportPDF(SelectedTour);
                    MessageBox.Show($" {SelectedTour.Name}: Summarized Report has been printed.");
                    ReloadTours();
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.ToString());
                }
            }
        }

        public void ExportTourToJson()
        {
            if (SelectedTour != null)
            {
                try
                {
                    SelectedTour = IBusinessLayer.Instance.ExportTourToJson(SelectedTour);
                    MessageBox.Show($"Tours has been exported.");
                    ReloadTours();
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.ToString());
                }
            }
        }

        public async Task ImportToursFromJson()
        {
            try
            {
                List<Tour> importedTours = await IBusinessLayer.Instance.ImportToursFromJson();
                MessageBox.Show($"{importedTours.Count} Tours have been imported.");
                ReloadTours();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
            }
        }

        public void ReloadTours()
        {
            try
            {
                Tour prevSelectedTour = null;
                if (SelectedTour != null)
                {
                    prevSelectedTour = new Tour(SelectedTour);
                }
                AllTours = new ObservableCollection<Tour>(IBusinessLayer.Instance.GetAllTours());

                //reapply filter
                SearchFilter = SearchFilter;

                if (prevSelectedTour != null)
                {
                    SelectedTour = FilteredTours.ToList().Find(t => t.Id == prevSelectedTour.Id);
                }

                if (SelectedTour == null || prevSelectedTour == null)
                {
                    SelectedTour = FilteredTours.FirstOrDefault();
                }

            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
            }
            finally
            {
                OnPropertyChanged(nameof(AllTours));
                SelectedTourChanged();
            }
        }

        private void DrawRoute()
        {
            if (!string.IsNullOrEmpty(SelectedTour.OSMjson))
            {
                var directionsContent = $"var directions = {SelectedTour.OSMjson};";
                string appDir = AppDomain.CurrentDomain.BaseDirectory;
                string filePath = Path.Combine(appDir, DirectionsFilePath);
                File.WriteAllText(filePath, directionsContent);
                if (UpdateMap != null)
                    UpdateMap();
            }
        }

        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
