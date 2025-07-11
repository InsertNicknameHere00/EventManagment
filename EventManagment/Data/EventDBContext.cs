using EventManagmentBackend.Models.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Reflection.Emit;

namespace EventManagmentBackend.Data
{
	public class EventDBContext : DbContext
	{
        public EventDBContext(DbContextOptions<EventDBContext> options)
             : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			if (!optionsBuilder.IsConfigured)
			{
				optionsBuilder.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=EventManagementDB;Trusted_Connection=true;MultipleActiveResultSets=true");
			}
		}

		public DbSet<User> Users { get; set; }
		public DbSet<Event> Events { get; set; }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);

			modelBuilder.Entity<User>(entity =>
			{
				entity.HasKey(user => user.Id);
				entity.HasIndex(user => user.Email).IsUnique();
				entity.Property(user => user.Email).IsRequired().HasMaxLength(256);
				entity.Property(user => user.PasswordHash).IsRequired();
				entity.Property(user => user.CreatedAt).HasDefaultValueSql("GETUTCDATE()");
			});

			modelBuilder.Entity<Event>(entity =>
			{
				entity.HasKey(entity => entity.Id);
				entity.Property(entity => entity.Name).IsRequired().HasMaxLength(200);
				entity.Property(entity => entity.Description).HasMaxLength(1000);
				entity.Property(entity => entity.Location).IsRequired().HasMaxLength(300);
				entity.Property(entity => entity.DateTime).IsRequired();
				entity.Property(entity => entity.CreatedAt).HasDefaultValueSql("GETUTCDATE()");
				entity.Property(entity => entity.UpdatedAt).HasDefaultValueSql("GETUTCDATE()");
				entity.HasOne(entity => entity.Creator)
					  .WithMany(user => user.Events)
					  .HasForeignKey(entity => entity.CreatedBy)
                       .HasPrincipalKey(user => user.Email)
                      .OnDelete(DeleteBehavior.Restrict);
			});
		}
	}
}
