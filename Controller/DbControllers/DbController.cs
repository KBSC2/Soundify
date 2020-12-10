using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
        public virtual Task<T> GetItem(int id)
        {
            if (!RowExists(id))
                throw new ArgumentOutOfRangeException();

            return Set.FirstOrDefaultAsync(x => x.ID == id);

        }

        // CREATE
        public virtual void CreateItem(T item)
        {
            Set.Add(item);

            // If the database is a mock one, do not use the context (not required)
            if (RealDatabase())
            {
                Context.Add(item);
                Context.Entry(item).State = EntityState.Added;
                Context.SaveChangesAsync();
            }
        }

        // UPDATE
        public virtual async void UpdateItem(T item)
        {
            var dbItem = await GetItem(item.ID);
            dbItem = item;

            // If the database is a mock one, do not use the context (not required)
            if (!RealDatabase()) return;

            Context.Entry(dbItem).State = EntityState.Modified;
            Context.SaveChangesAsync();
        }

        // DELETE
        public virtual async void DeleteItem(int id)
        {
            try
            {
                var item = await GetItem(id);
                Set.Remove(item);

                // If the database is a mock one, do not use the context (not required)
                if (!RealDatabase()) return;

                Context.Entry(item).State = EntityState.Deleted;
                Context.SaveChangesAsync();
            }
            catch (ArgumentOutOfRangeException)
            {
            }
        }

        // GET LIST
        public virtual Task<List<T>> GetList()
        {
            return Set.ToListAsync();
        }

        // GET LIST WITH FILTER
        public virtual Task<List<T>> GetFilteredList(Func<T, bool> filter)
        {
            return Set.Where(filter).AsQueryable().ToListAsync();
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
