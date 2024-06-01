using Microsoft.EntityFrameworkCore;
using SWE_TourPlanner_WPF.Models;
using System.Linq;

namespace SWE_TourPlanner_WPF.DataAccessLayer
{
    public class TourLogRepository : ARepository<TourLog, int>
    {
        public TourLogRepository(DatabaseContext context) : base(context)
        {
        }
        
        protected override DbSet<TourLog> Table => Context.TourLogs;
        public TourLog GetLast() => Table.OrderBy(t => t.Id).LastOrDefault();
        public TourLog GetOneEager(int id) => Table.Include(t => t.Tour).ToList().Find(t => id == t.Id)!;
        public override int Update(TourLog newTourLog)
        {
            TourLog tourLog = GetOneEager(newTourLog.Id);
            tourLog.Update(newTourLog);
            return Context.SaveChanges();
        }
    }
}
