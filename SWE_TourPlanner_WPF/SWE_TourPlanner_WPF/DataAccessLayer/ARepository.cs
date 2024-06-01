using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SWE_TourPlanner_WPF.DataAccessLayer
{
    public abstract class ARepository<R, ID> : IDisposable
        where R : class
    {
        protected DatabaseContext Context { get; }
        protected abstract DbSet<R> Table { get; }
        public ARepository(DatabaseContext context)
        {
            Context = context;
        }
        public List<R> GetAll() => Table.ToList();
        public R GetOne(ID id) => Table.Find(id);
        public int Add(R entity)
        {
            Table.Add(entity);
            return Context.SaveChanges();
        }
        public int AddRange(IEnumerable<R> entities)
        {
            Table.AddRange(entities);
            return Context.SaveChanges();
        }
        public int Delete(R entity)
        {
            Table.Remove(entity);
            return Context.SaveChanges();
        }
        public int DeleteRange(IEnumerable<R> entities)
        {
            Table.RemoveRange(entities);
            return Context.SaveChanges();
        }
        public virtual int Update(R entity)
        {
            return Context.SaveChanges();
        }
        public void Dispose() => Context.Dispose();
    }
}
