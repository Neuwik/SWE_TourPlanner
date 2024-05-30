using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWE_TourPlanner_WPF.DataBase
{
    class TourLogRepository : ARepository<TourLog, int>
    {
        public TourLogRepository(DatabaseContext context) : base(context)
        {
        }
        
        protected override DbSet<TourLog> Table => Context.TourLogs;
        public TourLog GetOneEager(int id) => Table.Include(t => t.Tour).ToList().Find(t => id == t.Id)!;
        public override int Update(TourLog newTourLog)
        {
            TourLog tourLog = GetOneEager(newTourLog.Id);
            tourLog.Update(newTourLog);
            return Context.SaveChanges();
        }
    }
}
