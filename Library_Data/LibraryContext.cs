using Library_Domain.Modles;
using Microsoft.EntityFrameworkCore;

namespace Library_Data
{
    public class LibraryContext : DbContext
    {
        public DbSet<Auther> Authers { get; set; }
        public DbSet<Book> Books { get; set; }

        public LibraryContext(DbContextOptions<LibraryContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Book>()
                        .HasOne(b => b.Auther)
                        .WithMany(a => a.Books)
                        .HasForeignKey(b => b.AutherId)
                        .OnDelete(DeleteBehavior.Cascade);
        }
    }
}