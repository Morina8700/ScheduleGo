using Microsoft.EntityFrameworkCore;
using WebApi.Models;

namespace WebApi.Services
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
        }


        public required DbSet<Event> Events { get; set; }

        public required DbSet<ToDoItem> ToDoItems { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<ToDoItem>()
                .HasOne(task => task.Event)
                .WithMany(evt => evt.Tasks)
                .HasForeignKey(task => task.EventId)
                .OnDelete(DeleteBehavior.SetNull);
        }

    }
}
