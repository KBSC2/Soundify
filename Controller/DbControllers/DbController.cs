using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Model.Database.Contexts;
using Model.DbModels;

namespace Controller
{
    public abstract class DbController<T> where T : DbModel
    {
        protected IDatabaseContext Context { get; set; }
        protected DbSet<T> Set { get; set; }

        protected DbController(IDatabaseContext context, DbSet<T> set)
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
            Set.Add(item);

            // If the database is a mock one, do not use the context (not required)
            if (!RealDatabase()) return;

            Context.Add(item);
            Context.Entry(item).State = EntityState.Added;
            Context.SaveChanges();
        }

        // UPDATE
        public virtual void UpdateItem(T item)
        {
            var dbItem = GetItem(item.ID);
            dbItem = item;

            // If the database is a mock one, do not use the context (not required)
            if (!RealDatabase()) return;

            Context.Entry(dbItem).State = EntityState.Modified;
            Context.SaveChanges();
        }

        // DELETE
        public virtual void DeleteItem(int id)
        {
            try
            {
                var item = GetItem(id);
                Set.Remove(item);

                // If the database is a mock one, do not use the context (not required)
                if (!RealDatabase()) return;

                Context.Entry(item).State = EntityState.Deleted;
                Context.SaveChanges();
            }
            catch (ArgumentOutOfRangeException)
            {
            }
        }

        // GET LIST
        public virtual List<T> GetList()
        {
            return Set.ToList();
        }

        // GET LIST WITH FILTER
        public virtual List<T> GetFilteredList(Func<T, bool> filter)
        {
            return Set.Where(filter).ToList();
        }

        /**
         * Determine if the database is a real databse, or a mock database
         */
        public bool RealDatabase()
        {
            return Context is DatabaseContext;
        }
    }
}
