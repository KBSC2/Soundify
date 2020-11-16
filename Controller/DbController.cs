using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Model.Data;
using Model.DbModels;

namespace Controller
{
    public abstract class DbController<T> where T : DbModel
    {
        protected DatabaseContext Context { get; set; }
        protected DbSet<T> Set { get; set; }

        protected DbController(DatabaseContext context, DbSet<T> set)
        {
            Context = context;
            Set = set;
        }

        // EXISTS
        private bool RowExists(int id)
        {
            return Set.Any(e => e.ID == id);
        }

        // READ
        public virtual T GetItem(int id)
        {
            if (!RowExists(id))
                throw new ArgumentOutOfRangeException();

            return Set.FirstOrDefault(x => x.ID == id);
        }

        // CREATE
        public virtual void CreateItem(T item)
        {
            Context.Add(item);
            Context.Entry(item).State = EntityState.Added;
            Context.SaveChanges();
        }

        // UPDATE
        public virtual void UpdateItem(T item)
        {
            var dbItem = GetItem(item.ID);
            dbItem = item;

            Context.Entry(dbItem).State = EntityState.Modified;
            Context.SaveChanges();
        }

        // DELETE
        public virtual void DeleteItem(int id)
        {
            var item = GetItem(id);
            Context.Entry(item).State = EntityState.Deleted;
            Set.Remove(item);
            Context.SaveChanges();
        }

        // GET LIST
        public virtual List<T> GetList()
        {
            return Set.ToList();
        }

        // GET LIST WITH FILTER
        public virtual List<T> GetFilteredList(Func<T, bool> filter)
        {
            return Set.Where(x => filter(x)).ToList();
        }
        public virtual T GetLastItem()
        {
            return Set.OrderBy(i => i.ID).Last();
        }
    }
}
