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

        /**
         * Check if an item by the id exists
         *
         * @param id The id to check
         *
         * @return bool : The id is found in the database
         */
        private bool RowExists(int id)
        {
            return Set.Any(e => e.ID == id);
        }

        /**
         * Get an item by it's id
         *
         * @param id The id of the item
         *
         * @throws ArgumentOutOfRangeExeption : item id does not exist
         *
         * @return T The Item by it's id
         */
        public virtual T GetItem(int id)
        {
            if (!RowExists(id))
                throw new ArgumentOutOfRangeException();

            return Set.FirstOrDefault(x => x.ID == id);
        }

        /**
         * Insert an item into the database
         *
         * @param item The item to insert
         *
         * @return T The created item
         */
        public virtual T CreateItem(T item)
        {
            Set.Add(item);

            // If the database is a mock one, do not use the context (not required)
            if (RealDatabase())
            {
                Context.Add(item);
                Context.Entry(item).State = EntityState.Added;
                Context.SaveChanges();
            }

            return item;
        }

        /**
         * Update the item in the database
         *
         * @param item The item to update
         */
        public virtual void UpdateItem(T item)
        {
            if (IsDetached(item))
            {
                Set.Attach(item);
            }


            // If the database is a mock one, do not use the context (not required)
            if (!RealDatabase()) return;
            
            Context.Entry(item).State = EntityState.Modified;
            Context.SaveChanges();
        }

        /**
         * Delete an item by it's id
         *
         * @param id Item id to remove
         */
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

        /**
         * Get all items from the table
         *
         * @return List<T> All items in the table
         */
        public virtual List<T> GetList()
        {
            return Set.ToList();
        }

        /**
         * Get all items from the table matching the filter
         *
         * @param filter The simple filter to check by
         *
         * @return List<T> All items in the table, matching the filter
         */
        public virtual List<T> GetFilteredList(Func<T, bool> filter)
        {
            return Set.Where(filter).ToList();
        }

        /**
         * Determines if the database is a real database, or a mock database
         *
         * @return bool : Database is real, and not a mock one
         */
        public bool RealDatabase()
        {
            return Context is DatabaseContext;
        }

        /**
         * Determines if the entity witch is given is already being tracked
         *
         * @return if the item is attached
         */
        private bool IsDetached(T entity)
        {
            var localEntity = Set.Local?.FirstOrDefault(x => Equals(x.ID, entity.ID));
            if (localEntity != null) // entity stored in local
                return false;

            return Context.Entry(entity).State == EntityState.Detached;
        }

    }
}
