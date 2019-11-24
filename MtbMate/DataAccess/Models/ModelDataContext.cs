﻿using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace DataAccess.Models
{
    public partial class ModelDataContext : DbContext
    {
        public ModelDataContext()
        {
        }

        public ModelDataContext(DbContextOptions<ModelDataContext> options)
            : base(options)
        {
        }

        public virtual DbSet<AccelerometerReading> AccelerometerReading { get; set; }
        public virtual DbSet<Jump> Jump { get; set; }
        public virtual DbSet<Location> Location { get; set; }
        public virtual DbSet<Ride> Ride { get; set; }
        public virtual DbSet<User> User { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer("Server=localhost\\SQLSERVER17;Database=MtbMateDev;Trusted_Connection=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AccelerometerReading>(entity =>
            {
                entity.Property(e => e.Time).HasColumnType("datetime");

                entity.Property(e => e.X).HasColumnType("decimal(5, 3)");

                entity.Property(e => e.Y).HasColumnType("decimal(5, 3)");

                entity.Property(e => e.Z).HasColumnType("decimal(5, 3)");

                entity.HasOne(d => d.Ride)
                    .WithMany(p => p.AccelerometerReading)
                    .HasForeignKey(d => d.RideId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_AccelerometerReading_Ride");
            });

            modelBuilder.Entity<Jump>(entity =>
            {
                entity.Property(e => e.Airtime).HasColumnType("decimal(5, 3)");

                entity.Property(e => e.Time).HasColumnType("datetime");

                entity.HasOne(d => d.Ride)
                    .WithMany(p => p.Jump)
                    .HasForeignKey(d => d.RideId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Jump_Ride");
            });

            modelBuilder.Entity<Location>(entity =>
            {
                entity.Property(e => e.AccuracyInMetres).HasColumnType("decimal(6, 3)");

                entity.Property(e => e.Altitude).HasColumnType("decimal(6, 3)");

                entity.Property(e => e.Latitude).HasColumnType("decimal(25, 20)");

                entity.Property(e => e.Longitude).HasColumnType("decimal(25, 20)");

                entity.Property(e => e.SpeedMetresPerSecond).HasColumnType("decimal(6, 3)");

                entity.Property(e => e.Timestamp).HasColumnType("datetime");

                entity.HasOne(d => d.Ride)
                    .WithMany(p => p.Location)
                    .HasForeignKey(d => d.RideId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Location_Ride");
            });

            modelBuilder.Entity<Ride>(entity =>
            {
                entity.Property(e => e.EndUtc).HasColumnType("datetime");

                entity.Property(e => e.Name).HasMaxLength(255);

                entity.Property(e => e.StartUtc).HasColumnType("datetime");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Ride)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Ride_User");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.HasIndex(e => e.GoogleUserId)
                    .HasName("UQ__User__437CD197C48C1DEE")
                    .IsUnique();

                entity.Property(e => e.GoogleUserId)
                    .IsRequired()
                    .HasMaxLength(255);

                entity.Property(e => e.Name).HasMaxLength(255);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
