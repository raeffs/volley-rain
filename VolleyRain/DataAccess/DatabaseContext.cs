﻿using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Web;
using VolleyRain.Models;

namespace VolleyRain.DataAccess
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext()
            : base("DatabaseContext")
        {
        }

        public DbSet<Attendance> Attendances { get; set; }

        public DbSet<AttendanceType> AttendanceTypes { get; set; }

        public DbSet<Document> Documents { get; set; }

        public DbSet<Event> Events { get; set; }

        public DbSet<EventType> EventTypes { get; set; }

        public DbSet<GameDetails> GameDetails { get; set; }

        public DbSet<NewsArticle> NewsArticles { get; set; }

        public DbSet<PasswordResetToken> PasswordResetTokens { get; set; }

        public DbSet<Ranking> Rankings { get; set; }

        public DbSet<Role> Roles { get; set; }

        public DbSet<Season> Seasons { get; set; }

        public DbSet<TeamMembership> TeamMemberships { get; set; }

        public DbSet<Team> Teams { get; set; }

        public DbSet<User> Users { get; set; }

        public DbSet<Log> Log { get; set; }

        public DbSet<Comment> Comments { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();

            // optional

            modelBuilder.Entity<User>()
                .HasMany(u => u.Roles)
                .WithMany(r => r.Users)
                .Map(m =>
                {
                    m.ToTable("UserRole");
                });

            //modelBuilder.Entity<Team>()
            //    .HasMany(t => t.Members)
            //    .WithMany(m => m.Teams)
            //    .Map(m =>
            //    {
            //        m.ToTable("TeamMembership");
            //    });

            modelBuilder.Entity<TeamMembership>()
                .HasKey(m => new { m.TeamID, m.UserID });
        }
    }
}