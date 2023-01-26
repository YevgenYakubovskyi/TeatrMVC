using Microsoft.AspNet.Identity.EntityFramework;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Net.Sockets;
using System.Runtime.Remoting.Contexts;
using Theatr.DAL.Entities;


namespace Theatr.DAL.EF
{
    public class DataContext : IdentityDbContext<User>
    {
        public DbSet<Author> Authors { get; set; }
        public DbSet<Performance> Performances { get; set; }
        public DbSet<Genre> Genres { get; set; }
        public DbSet<Ticket> Tickets { get; set; }
        public DbSet<ClientProfile> ClientProfiles { get; set; }
        public DataContext() : base("Theater")
        {

        }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            modelBuilder.Entity<Performance>().HasMany(s => s.Authors).WithMany(c => c.Perfomances).Map(cs =>
            {
                cs.MapLeftKey("PerfomenceId");
                cs.MapRightKey("AuthorId");
                cs.ToTable("PerfomanceAuthors");
            });

            modelBuilder.Entity<Performance>().HasMany(s => s.Genres).WithMany(c => c.Perfomances).Map(cs =>
            {
                cs.MapLeftKey("PerfomenceId");
                cs.MapRightKey("GenreId");
                cs.ToTable("PerfomanceGenres");
            });
            modelBuilder.Entity<Ticket>()
            .HasRequired(s => s.User)
            .WithMany(g => g.Tickets)
            .HasForeignKey(s => s.UserId);

            modelBuilder.Entity<Ticket>()
            .HasRequired(s => s.Perfomance)
            .WithMany(g => g.Tickets)
            .HasForeignKey(s => s.PerfomanceId);

            base.OnModelCreating(modelBuilder);
        }
    }
}