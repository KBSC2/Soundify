using Microsoft.EntityFrameworkCore;
using Model.DbModels;

namespace Model.Data
{
    public class DatabaseContext : DbContext
    {
        /*public DbSet<T : DbModel> <TableName> { get; set; }*/

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(@"Data Source=127.0.0.1;Initial Catalog=Soundify;User ID=SA;Password=Sterk_W@chtw00rd2");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // For each model required in the database, copy the line below and change both .Entity<<Model>> and .ToTable("<tableName>")
            /*modelBuilder.Entity<T : DbModel>().ToTable("<TableName>");*/
        }
    }
}
