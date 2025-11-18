using Microsoft.EntityFrameworkCore;
using PortalNauchnyhPublikatsiy.Domain.Entities;
using System.Reflection;

namespace PortalNauchnyhPublikatsiy.Infrastructure.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<Department> Departments { get; set; }
        public DbSet<JournalsConferences> JournalsConferences { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<Publication> Publications { get; set; }
        public DbSet<ScientificDirection> ScientificDirections { get; set; }
        public DbSet<Teacher> Teachers { get; set; }
        public DbSet<PublicationAuthor> PublicationAuthors { get; set; }
        public DbSet<ProjectParticipant> ProjectParticipants { get; set; }
        public DbSet<PublicationProject> PublicationProjects { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<PublicationAuthor>().HasKey(pa => new { pa.PublicationId, pa.TeacherId });
            modelBuilder.Entity<ProjectParticipant>().HasKey(pp => new { pp.ProjectId, pp.TeacherId });
            modelBuilder.Entity<PublicationProject>().HasKey(pp => new { pp.PublicationId, pp.ProjectId });

            modelBuilder.Entity<Project>()
                .HasOne(p => p.Leader)
                .WithMany() 
                .HasForeignKey(p => p.LeaderId)
                .OnDelete(DeleteBehavior.Restrict); 

            modelBuilder.Entity<ProjectParticipant>()
                .HasOne(pp => pp.Teacher)
                .WithMany()
                .HasForeignKey(pp => pp.TeacherId)
                .OnDelete(DeleteBehavior.Restrict); 

            modelBuilder.Entity<ProjectParticipant>()
                .HasOne(pp => pp.Project)
                .WithMany()
                .HasForeignKey(pp => pp.ProjectId)
                .OnDelete(DeleteBehavior.Cascade); 
        }
    }
}