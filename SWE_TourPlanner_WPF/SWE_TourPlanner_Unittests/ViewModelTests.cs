using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using NUnit.Framework.Legacy;
using SWE_TourPlanner_WPF;

namespace SWE_TourPlanner_Unittests
{
    [TestFixture]
    public class ViewModelTests
    {
        [Test]
        public void AddTour_AddsTourToListAndClearsTempTour()
        {
            var viewModel = new ViewModel();
            var initialCount = viewModel.Tours.Count;

            viewModel.SelectedTour = new Tour {};
            viewModel.AddTour();

            ClassicAssert.AreEqual(initialCount + 1, viewModel.Tours.Count);
            ClassicAssert.IsNotNull(viewModel.SelectedTour);
        }

        [Test]
        public void DeleteTour_RemovesSelectedTourFromList()
        {
            var viewModel = new ViewModel();
            viewModel.Tours.Add(new Tour {});
            var initialCount = viewModel.Tours.Count;
            viewModel.SelectedTour = viewModel.Tours[0];

            viewModel.DeleteTour();

            ClassicAssert.AreEqual(initialCount - 1, viewModel.Tours.Count);
            ClassicAssert.IsNull(viewModel.SelectedTour);
        }
    }

}
