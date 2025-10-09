using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using WepAPI.Models.Domain;

namespace WepAPI.Data
{
    public class AppDbContext:DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Books> Books { get; set; }
        public DbSet<Authors> Authors { get; set; }
        public DbSet<Book_Author> Books_Author { get; set; }
        public DbSet<Publishers> Publishers { get; set; }
        public DbSet<Image> Images { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<Book_Author>()
                .HasOne(b => b.Books)
                .WithMany(ba => ba.Book_Author)
                .HasForeignKey(bi => bi.BookId);
            modelBuilder.Entity<Book_Author>()
                .HasOne(b => b.Authors)
                .WithMany(ba => ba.Book_Author)
                .HasForeignKey(bi => bi.AuthorId);
        }
 

    }
}
    

