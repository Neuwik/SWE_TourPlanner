using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using NUnit.Framework.Legacy;
using SWE_TourPlanner_WPF;
using SWE_TourPlanner_WPF.Models;
using SWE_TourPlanner_WPF.ViewLayer;

namespace SWE_TourPlanner_Unittests
{
    [TestFixture]
    public class ViewModelTests
    {
        [Test]
        public void AddTour_AddsTourToListAndClearsTempTour()
        {
            var viewModel = new ViewModel();
            var initialCount = viewModel.AllTours.Count;

            viewModel.SelectedTour = new Tour {};
            viewModel.AddTour();

            ClassicAssert.AreEqual(initialCount + 1, viewModel.AllTours.Count);
            ClassicAssert.IsNotNull(viewModel.SelectedTour);
        }

        [Test]
        public void DeleteTour_RemovesSelectedTourFromList()
        {
            var viewModel = new ViewModel();
            viewModel.AllTours.Add(new Tour {});
            var initialCount = viewModel.AllTours.Count;
            viewModel.SelectedTour = viewModel.AllTours[0];

            viewModel.DeleteTour();

            ClassicAssert.AreEqual(initialCount - 1, viewModel.AllTours.Count);
            ClassicAssert.IsNull(viewModel.SelectedTour);
        }
    }

}
