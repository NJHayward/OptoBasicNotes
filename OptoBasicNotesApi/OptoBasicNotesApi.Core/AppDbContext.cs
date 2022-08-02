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

            //seed data here as this is a small app
            builder.Entity<Category>().HasData(
                new Category
                {
                    Id = 1,
                    CategoryName = "Some category 1"
                },
                new Category
                {
                    Id = 2,
                    CategoryName = "Some category 2"
                },
                new Category
                {
                    Id = 3,
                    CategoryName = "Some category 3"
                },
                new Category
                {
                    Id = 4,
                    CategoryName = "Some category 4"
                },
                new Category
                {
                    Id = 5,
                    CategoryName = "Some category 5"
                }
            );

            builder.Entity<Note>().HasData(
                new Note
                {
                    Id = 1,
                    NoteBody = "## My Seeded Note \n With a test script <script>alert('inject')</script>"
                }
            );

            builder.Entity<NoteCategory>().HasData(
                new NoteCategory
                {
                    Id = 1,
                    NoteId = 1,
                    CategoryId = 1
                },
                new NoteCategory
                {
                    Id = 2,
                    NoteId = 1,
                    CategoryId = 2
                }
            );
        }
    }
}
