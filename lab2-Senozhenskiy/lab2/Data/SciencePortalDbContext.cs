using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using lab2.Models;

namespace lab2.Data;

public partial class SciencePortalDbContext : DbContext
{
    public SciencePortalDbContext()
    {
    }

    public SciencePortalDbContext(DbContextOptions<SciencePortalDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Department> Departments { get; set; }

    public virtual DbSet<JournalsConference> JournalsConferences { get; set; }

    public virtual DbSet<Project> Projects { get; set; }

    public virtual DbSet<Publication> Publications { get; set; }

    public virtual DbSet<PublicationsByAuthor> PublicationsByAuthors { get; set; }

    public virtual DbSet<PublicationsByDepartment> PublicationsByDepartments { get; set; }

    public virtual DbSet<Q1q2publication> Q1q2publications { get; set; }

    public virtual DbSet<ScientificDirection> ScientificDirections { get; set; }

    public virtual DbSet<Teacher> Teachers { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=	db28071.public.databaseasp.net;Database=db28071;User Id=db28071;Password=X?x78C-hM_c4;Encrypt=False;MultipleActiveResultSets=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.UseCollation("Cyrillic_General_CI_AS");

        modelBuilder.Entity<Department>(entity =>
        {
            entity.HasKey(e => e.DepartmentId).HasName("PK__Departme__B2079BCD581612C1");

            entity.Property(e => e.DepartmentId).HasColumnName("DepartmentID");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Profile)
                .HasMaxLength(200)
                .IsUnicode(false);
        });

        modelBuilder.Entity<JournalsConference>(entity =>
        {
            entity.HasKey(e => e.JournalConfId).HasName("PK__Journals__A90C4A1C7A7ED708");

            entity.Property(e => e.JournalConfId).HasColumnName("JournalConfID");
            entity.Property(e => e.IssnIsbn)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("ISSN_ISBN");
            entity.Property(e => e.Name)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.Publisher)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Rating)
                .HasMaxLength(20)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Project>(entity =>
        {
            entity.HasKey(e => e.ProjectId).HasName("PK__Projects__761ABED024818B93");

            entity.Property(e => e.ProjectId).HasColumnName("ProjectID");
            entity.Property(e => e.FundingOrg)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.LeaderId).HasColumnName("LeaderID");
            entity.Property(e => e.Name)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.Number)
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.HasOne(d => d.Leader).WithMany(p => p.Projects)
                .HasForeignKey(d => d.LeaderId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Projects__Leader__4E88ABD4");
        });

        modelBuilder.Entity<Publication>(entity =>
        {
            entity.HasKey(e => e.PublicationId).HasName("PK__Publicat__05E6DC58408A8386");

            entity.Property(e => e.PublicationId).HasColumnName("PublicationID");
            entity.Property(e => e.DirectionId).HasColumnName("DirectionID");
            entity.Property(e => e.DoiLink)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("DOI_Link");
            entity.Property(e => e.FilePath)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.JournalConfId).HasColumnName("JournalConfID");
            entity.Property(e => e.Keywords)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.Title)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.Type)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Year).HasColumnType("datetime");

            entity.HasOne(d => d.Direction).WithMany(p => p.Publications)
                .HasForeignKey(d => d.DirectionId)
                .HasConstraintName("FK__Publicati__Direc__571DF1D5");

            entity.HasOne(d => d.JournalConf).WithMany(p => p.Publications)
                .HasForeignKey(d => d.JournalConfId)
                .HasConstraintName("FK__Publicati__Journ__5629CD9C");

            entity.HasMany(d => d.Projects).WithMany(p => p.Publications)
                .UsingEntity<Dictionary<string, object>>(
                    "PublicationProject",
                    r => r.HasOne<Project>().WithMany()
                        .HasForeignKey("ProjectId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK__Publicati__Proje__5EBF139D"),
                    l => l.HasOne<Publication>().WithMany()
                        .HasForeignKey("PublicationId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK__Publicati__Publi__5DCAEF64"),
                    j =>
                    {
                        j.HasKey("PublicationId", "ProjectId").HasName("PK__Publicat__128777B5BE792A6D");
                        j.ToTable("PublicationProjects");
                        j.IndexerProperty<int>("PublicationId").HasColumnName("PublicationID");
                        j.IndexerProperty<int>("ProjectId").HasColumnName("ProjectID");
                    });

            entity.HasMany(d => d.Teachers).WithMany(p => p.Publications)
                .UsingEntity<Dictionary<string, object>>(
                    "PublicationAuthor",
                    r => r.HasOne<Teacher>().WithMany()
                        .HasForeignKey("TeacherId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK__Publicati__Teach__5AEE82B9"),
                    l => l.HasOne<Publication>().WithMany()
                        .HasForeignKey("PublicationId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK__Publicati__Publi__59FA5E80"),
                    j =>
                    {
                        j.HasKey("PublicationId", "TeacherId").HasName("PK__Publicat__5B39F9CCF219DCB7");
                        j.ToTable("PublicationAuthors");
                        j.IndexerProperty<int>("PublicationId").HasColumnName("PublicationID");
                        j.IndexerProperty<int>("TeacherId").HasColumnName("TeacherID");
                    });
        });

        modelBuilder.Entity<PublicationsByAuthor>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("PublicationsByAuthor");

            entity.Property(e => e.Author)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.JournalConference)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.Title)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.Type)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Year).HasColumnType("datetime");
        });

        modelBuilder.Entity<PublicationsByDepartment>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("PublicationsByDepartment");

            entity.Property(e => e.Department)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Title)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.Type)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Year).HasColumnType("datetime");
        });

        modelBuilder.Entity<Q1q2publication>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("Q1Q2Publications");

            entity.Property(e => e.JournalConference)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.Rating)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.Title)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.Type)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Year).HasColumnType("datetime");
        });

        modelBuilder.Entity<ScientificDirection>(entity =>
        {
            entity.HasKey(e => e.DirectionId).HasName("PK__Scientif__87684626F62E2FC1");

            entity.Property(e => e.DirectionId).HasColumnName("DirectionID");
            entity.Property(e => e.Description).HasColumnType("text");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Teacher>(entity =>
        {
            entity.HasKey(e => e.TeacherId).HasName("PK__Teachers__EDF25944DA6C0CA0");

            entity.Property(e => e.TeacherId).HasColumnName("TeacherID");
            entity.Property(e => e.Degree)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.DepartmentId).HasColumnName("DepartmentID");
            entity.Property(e => e.FullName)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Position)
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.HasOne(d => d.Department).WithMany(p => p.Teachers)
                .HasForeignKey(d => d.DepartmentId)
                .HasConstraintName("FK__Teachers__Depart__4BAC3F29");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
