using Microsoft.EntityFrameworkCore;
using SWE_TourPlanner_WPF.Models;
using System.Collections.Generic;
using System.Linq;
namespace SWE_TourPlanner_WPF.DataAccessLayer
{
    public class TourRepository : ARepository<Tour, int>
    {
        public TourRepository(DatabaseContext context) : base(context) {}

        protected override DbSet<Tour> Table => Context.Tours;
        public virtual Tour GetLast() => Table.OrderBy(t => t.Id).LastOrDefault();
        public virtual Tour GetOneEager(int id) => Table.Include(t => t.TourLogs).ToList().Find(t => id == t.Id)!;
        public virtual List<Tour> GetAllEager() => Table.Include(t => t.TourLogs).ToList();
        public override int Update(Tour newTour)
        {
            Tour tour = GetOne(newTour.Id);
            tour.Update(newTour);
            return Context.SaveChanges();
        }
    }
}
