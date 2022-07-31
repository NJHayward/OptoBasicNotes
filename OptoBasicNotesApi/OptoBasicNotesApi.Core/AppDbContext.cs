using Microsoft.EntityFrameworkCore;
using OptoBasicNotesApi.Core.Models;

namespace OptoBasicNotesApi.Core
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions options)
            : base(options)
        {

        }

        public DbSet<Note> Notes { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<NoteCategory> NoteCategories { get; set; }


        /// <summary>
        /// handles any extra tasks when creating migrations for the DB.
        /// </summary>
        /// <param name="builder"></param>
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            //Ensure CategoryName is unique db level as we dont want duplicate values.
            builder.Entity<Category>(entity => {
                entity.HasIndex(c => c.CategoryName).IsUnique();
            });
        }
    }
}
