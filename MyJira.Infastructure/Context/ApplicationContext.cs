using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MyJira.Entity.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyJira.Infastructure.Context
{
    public class ApplicationContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options)
        {

        }

        public DbSet<Project> Projects { get; set; }
        public DbSet<Ticket> Tickets { get; set; }
        public DbSet<TicketBoard> TicketBoards { get; set; }
        public DbSet<ProjectMember> ProjectMembers { get; set; }
        public DbSet<Member> Members { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<TaskLog> TaskLogs { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Ignore<EntityBase>();
            modelBuilder.Entity<Project>(entity =>
            {
                entity.Property(e => e.CreatedAt)
                    .HasColumnType("timestamp without time zone");

                entity.Property(e => e.LastModifiedDate)
                    .HasColumnType("timestamp without time zone");
            });

            modelBuilder.Entity<Ticket>(entity =>
            {
                entity.Property(e => e.CreatedAt)
                    .HasColumnType("timestamp without time zone");

                entity.Property(e => e.LastModifiedDate)
                    .HasColumnType("timestamp without time zone");

                entity.Property(e => e.Start)
                    .HasColumnType("timestamp without time zone");

                entity.Property(e => e.End)
                    .HasColumnType("timestamp without time zone");
            });
            modelBuilder.Entity<TicketBoard>(entity =>
            {
                entity.Property(e => e.CreatedAt)
                    .HasColumnType("timestamp without time zone");

                entity.Property(e => e.LastModifiedDate)
                    .HasColumnType("timestamp without time zone");
            });
            modelBuilder.Entity<ProjectMember>(entity =>
            {
                entity.Property(e => e.CreatedAt)
                    .HasColumnType("timestamp without time zone");

                entity.Property(e => e.LastModifiedDate)
                    .HasColumnType("timestamp without time zone");
            });
            modelBuilder.Entity<Member>(entity =>
            {
                entity.Property(e => e.CreatedAt)
                    .HasColumnType("timestamp without time zone");

                entity.Property(e => e.LastModifiedDate)
                    .HasColumnType("timestamp without time zone");
            });
            modelBuilder.Entity<Comment>(entity =>
            {
                entity.Property(e => e.CreatedAt)
                    .HasColumnType("timestamp without time zone");

                entity.Property(e => e.LastModifiedDate)
                    .HasColumnType("timestamp without time zone");
            });
            modelBuilder.Entity<TaskLog>(entity =>
            {
                entity.Property(e => e.CreatedAt)
                    .HasColumnType("timestamp without time zone");

                entity.Property(e => e.LastModifiedDate)
                    .HasColumnType("timestamp without time zone");
            });
        }
    }
}
