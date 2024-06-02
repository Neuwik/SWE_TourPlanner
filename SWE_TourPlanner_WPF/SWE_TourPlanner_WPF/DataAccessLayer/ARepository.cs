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
        public virtual List<R> GetAll() => Table.ToList();
        public virtual R GetOne(ID id) => Table.Find(id);
        public virtual int Add(R entity)
        {
            Table.Add(entity);
            return Context.SaveChanges();
        }
        public virtual int AddRange(IEnumerable<R> entities)
        {
            Table.AddRange(entities);
            return Context.SaveChanges();
        }
        public virtual int Delete(R entity)
        {
            Table.Remove(entity);
            return Context.SaveChanges();
        }
        public virtual int DeleteRange(IEnumerable<R> entities)
        {
            Table.RemoveRange(entities);
            return Context.SaveChanges();
        }
        public virtual int Update(R entity)
        {
            return Context.SaveChanges();
        }
        public virtual void Dispose() => Context.Dispose();
    }
}
