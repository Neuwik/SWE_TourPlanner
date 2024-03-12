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

        private Tour _selectedTour;
        public Tour SelectedTour
        {
            get { return _selectedTour; }
            set
            {
                if (_selectedTour != value)
                {
                    _selectedTour = value;
                    OnPropertyChanged("SelectedTour");
                }
            }
        }
        private Tour _tempTour;
        public Tour TempTour
        {
            get { return _tempTour; }
            set
            {
                if (_tempTour != value)
                {
                    _tempTour = value;
                    OnPropertyChanged("TempTour");
                }
            }
        }
        public ObservableCollection<Tour> Tours { get; set; }

        public ViewModel() 
        {
            Tours = new ObservableCollection<Tour> { };
            TempTour = new Tour();
        }

        public void AddTour()
        {
            Tours.Add(new Tour(TempTour));
            OnPropertyChanged(nameof(Tours));

            TempTour = new Tour();
            OnPropertyChanged(nameof(TempTour));
        }

        public void DeleteTour()
        {
            if (SelectedTour != null)
            {
                Tours.Remove(SelectedTour);
                OnPropertyChanged(nameof(Tours));

                SelectedTour = null;
                OnPropertyChanged(nameof(SelectedTour));
            }
        }

        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
