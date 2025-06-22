using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace EntitiesFitness;

public partial class SqliteContext : DbContext
{
    public SqliteContext()
    {
    }

    public SqliteContext(DbContextOptions<SqliteContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Exercice> Exercices { get; set; }

    public virtual DbSet<Login> Logins { get; set; }

    public virtual DbSet<Muscle> Muscles { get; set; }

    public virtual DbSet<NomExercice> NomExercices { get; set; }

    public virtual DbSet<NomSeance> NomSeances { get; set; }

    public virtual DbSet<Routine> Routines { get; set; }

    public virtual DbSet<Seance> Seances { get; set; }

    public virtual DbSet<Serie> Series { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlite("Data Source=G:\\Github\\AppFitness\\DbFitness\\DbFitness.db");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Exercice>(entity =>
        {
            entity.ToTable("Exercice");

            entity.HasIndex(e => e.Id, "IX_Exercice_Id").IsUnique();

            entity.Property(e => e.Date).HasDefaultValue("00/00/0000");
            entity.Property(e => e.NomExerciceId).HasColumnName("NomExercice_Id");
            entity.Property(e => e.NumSerie).HasDefaultValue(1);
            entity.Property(e => e.SerieId)
                .HasDefaultValue(1)
                .HasColumnName("Serie_Id");

            entity.HasOne(d => d.NomExercice).WithMany(p => p.Exercices)
                .HasForeignKey(d => d.NomExerciceId)
                .OnDelete(DeleteBehavior.ClientSetNull);

            entity.HasOne(d => d.Serie).WithMany(p => p.Exercices)
                .HasForeignKey(d => d.SerieId)
                .OnDelete(DeleteBehavior.ClientSetNull);
        });

        modelBuilder.Entity<Login>(entity =>
        {
            entity.ToTable("Login");

            entity.HasIndex(e => e.Id, "IX_Login_Id").IsUnique();
        });

        modelBuilder.Entity<Muscle>(entity =>
        {
            entity.ToTable("Muscle");

            entity.HasIndex(e => e.Id, "IX_Muscle_Id").IsUnique();

            entity.HasIndex(e => e.Nom, "IX_Muscle_Nom").IsUnique();
        });

        modelBuilder.Entity<NomExercice>(entity =>
        {
            entity.ToTable("NomExercice");

            entity.HasIndex(e => e.Id, "IX_NomExercice_Id").IsUnique();

            entity.HasIndex(e => e.Nom, "IX_NomExercice_Nom").IsUnique();

            entity.Property(e => e.MuscleId).HasColumnName("Muscle_Id");

            entity.HasOne(d => d.Muscle).WithMany(p => p.NomExercices)
                .HasForeignKey(d => d.MuscleId)
                .OnDelete(DeleteBehavior.ClientSetNull);
        });

        modelBuilder.Entity<NomSeance>(entity =>
        {
            entity.ToTable("NomSeance");

            entity.HasIndex(e => e.Id, "IX_NomSeance_Id").IsUnique();
        });

        modelBuilder.Entity<Routine>(entity =>
        {
            entity.ToTable("Routine");

            entity.HasIndex(e => e.Id, "IX_Routine_Id").IsUnique();

            entity.Property(e => e.SeanceId).HasColumnName("Seance_Id");
            entity.Property(e => e.UserId).HasColumnName("User_Id");

            entity.HasOne(d => d.Seance).WithMany(p => p.Routines).HasForeignKey(d => d.SeanceId);

            entity.HasOne(d => d.User).WithMany(p => p.Routines)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull);
        });

        modelBuilder.Entity<Seance>(entity =>
        {
            entity.ToTable("Seance");

            entity.HasIndex(e => e.Id, "IX_Seance_Id").IsUnique();

            entity.Property(e => e.ExerciceId).HasColumnName("Exercice_Id");
            entity.Property(e => e.NomSeanceId).HasColumnName("NomSeance_Id");

            entity.HasOne(d => d.Exercice).WithMany(p => p.Seances)
                .HasForeignKey(d => d.ExerciceId)
                .OnDelete(DeleteBehavior.ClientSetNull);

            entity.HasOne(d => d.NomSeance).WithMany(p => p.Seances)
                .HasForeignKey(d => d.NomSeanceId)
                .OnDelete(DeleteBehavior.ClientSetNull);
        });

        modelBuilder.Entity<Serie>(entity =>
        {
            entity.ToTable("Serie");

            entity.HasIndex(e => e.Id, "IX_Serie_Id").IsUnique();

            entity.Property(e => e.Poids).HasDefaultValue(0.0);
            entity.Property(e => e.Rep).HasDefaultValue(0);
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.ToTable("User");

            entity.HasIndex(e => e.Id, "IX_User_Id").IsUnique();

            entity.HasIndex(e => e.Nom, "IX_User_Nom").IsUnique();

            entity.Property(e => e.LoginId).HasColumnName("Login_Id");

            entity.HasOne(d => d.Login).WithMany(p => p.Users)
                .HasForeignKey(d => d.LoginId)
                .OnDelete(DeleteBehavior.ClientSetNull);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
