using System;
using System.Data.Entity.Migrations;
using System.Linq;
using NLog;
using VolleyRain.DataAccess;
using VolleyRain.Models;

namespace VolleyRain.Migrations
{
    public sealed class Configuration : DbMigrationsConfiguration<DatabaseContext>
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(DatabaseContext context)
        {
            SeedRoles(context);
            SeedEventTypes(context);
            SeedAttendacneTypes(context);

            SeedSeasons(context);
            SeedTeams(context);
            SeedUsers(context);
        }

        private void SeedRoles(DatabaseContext context)
        {
            context.Roles.AddOrUpdate(
                r => r.Name,
                new Role { Name = "Administrator", Description = "Built-in role for administrators", IsBuiltIn = true, IsDefaultAdminRole = true },
                new Role { Name = "Team-Administrator", Description = "Built-in role for team administrators", IsBuiltIn = true, IsDefaultTeamAdminRole = true },
                new Role { Name = "User", Description = "Built-in role for users", IsBuiltIn = true, IsDefaultUserRole = true }
                );
            context.SaveChanges();
        }

        private void SeedEventTypes(DatabaseContext context)
        {
            if (context.EventTypes.Any()) return;

            context.EventTypes.AddOrUpdate(
                t => t.Name,
                new EventType { Name = "Training", ShortName = "TR", ColorCode = "#33B5E5" },
                new EventType { Name = "Heimmatch", ShortName = "MH", ColorCode = "#9933CC" },
                new EventType { Name = "Auswärtsmatch", ShortName = "MA", ColorCode = "#AA66CC" },
                new EventType { Name = "Turnier", ShortName = "TU", ColorCode = "#FFBB33" },
                new EventType { Name = "Ferien", ShortName = "F", ColorCode = "#BBBBBB" },
                new EventType { Name = "Spezialanlass", ShortName = "S", ColorCode = "#FE9A2E" }
                );
            context.SaveChanges();
        }

        private void SeedAttendacneTypes(DatabaseContext context)
        {
            if (context.AttendanceTypes.Any()) return;

            context.AttendanceTypes.AddOrUpdate(
                t => t.Name,
                new AttendanceType { Name = "Zusage", ShortName = "Zu", RepresentsAttendance = true, ColorCode = "#DFF0D8", IsUserSelectable = true },
                new AttendanceType { Name = "Absage", ShortName = "Ab", RepresentsAttendance = false, ColorCode = "#F2DEDE", IsUserSelectable = true },
                new AttendanceType { Name = "Verletzt", ShortName = "V", RepresentsAttendance = false, ColorCode = "#D9EDF7", IsUserSelectable = true },
                new AttendanceType { Name = "Unentschuldigt", ShortName = "U", RepresentsAttendance = false, ColorCode = "#D97472", IsUserSelectable = false },
                new AttendanceType { Name = "Teilnahme erfolgt", ShortName = "T", RepresentsAttendance = true, ColorCode = "#5C965D", IsUserSelectable = false }
                );
            context.SaveChanges();
        }

        private void SeedSeasons(DatabaseContext context)
        {
            try
            {
                var currentYear = DateTime.Now.Year;
                var seasons = new[]
                {
                    new Season { Name = $"Saison {currentYear - 1} / {currentYear}", Start = new DateTime(currentYear - 1, 4, 1), End = new DateTime(currentYear, 3, 31, 23, 59, 59) },
                    new Season { Name = $"Saison {currentYear} / {currentYear + 1}", Start = new DateTime(currentYear, 4, 1), End = new DateTime(currentYear + 1, 3, 31, 23, 59, 59) }
                };
                context.Seasons.AddOrUpdate(s => s.Name, seasons);
                context.SaveChanges();
            }
            catch (Exception e)
            {
                Logger.Error("Could not seed seasons!", e);
            }
        }

        private void SeedTeams(DatabaseContext context)
        {
            if (context.Teams.Any()) return;

            context.Teams.AddOrUpdate(
                t => t.Name,
                new Team { Name = "SVKT Rain", Season = context.Seasons.FirstOrDefault(), ExternalID = 23589, ExternalGroupID = 8297 }
                );
            context.SaveChanges();
        }

        private void SeedUsers(DatabaseContext context)
        {
            context.Users.AddOrUpdate(
                u => u.Email,
                new User { Email = "<removed>", Name = "<removed>", Surname = "<removed>", IsApproved = true, Salt = "<removed>", Password = "<removed>", Roles = context.Roles.ToList() }
                );
            context.SaveChanges();

            // Ensure full rights for admin even if removed for some reason (prior bug)
            var admin = context.Users.Single(u => u.Roles.Any(r => r.IsDefaultAdminRole));
            context.Roles.ToList().ForEach(r =>
            {
                if (!admin.Roles.Contains(r))
                {
                    admin.Roles.Add(r);
                }
            });
            context.SaveChanges();
        }
    }
}
