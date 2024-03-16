using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace SWE_TourPlanner_WPF
{
    public class ViewModel : INotifyPropertyChanged
    {
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

        public ViewModel() 
        {
            Tours = new ObservableCollection<Tour> { };
            SeedData();
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
            SelectedTour.ImagePath = "/route.png";
            Tours.Add(SelectedTour);
            OnPropertyChanged(nameof(Tours));
            OnPropertyChanged(nameof(SelectedTour));
            OnPropertyChanged(nameof(IsTourSelected));
        }

        public void DeleteTour()
        {
            if (SelectedTour != null)
            {
                Tours.Remove(SelectedTour);
                OnPropertyChanged(nameof(Tours));

                SelectedTour = null;
            }
        }

        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
        private void SeedData()
        {
            for (int i = 1; i < 6; i++)
            {
                SelectedTour = new Tour()
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
                AddTour();
            }

            SelectedTour = null;
            OnPropertyChanged(nameof(SelectedTour));
            OnPropertyChanged(nameof(Tours));
        }
    }
}
