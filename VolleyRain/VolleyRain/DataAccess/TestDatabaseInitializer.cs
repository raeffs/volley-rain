using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using VolleyRain.Models;

namespace VolleyRain.DataAccess
{
    public class TestDatabaseInitializer : DropCreateDatabaseIfModelChanges<DatabaseContext>
    {
        private Season Season;

        private Team Team;

        private Role Role_Admin;
        private Role Role_TeamAdmin;
        private Role Role_User;

        private User User_Admin;

        private EventType EventType_Training;
        private EventType EventType_Match;

        private AttendanceType AttendanceType_Attending;
        private AttendanceType AttendanceType_Absent;

        private void SeedSeasons(DatabaseContext context)
        {
            Season = new Season { Name = "Saison 2014 / 2015", Start = new DateTime(2014, 7, 1), End = new DateTime(2015, 6, 30) };
            context.Seasons.Add(Season);
            context.SaveChanges();
        }

        private void SeedTeams(DatabaseContext context)
        {
            Team = new Team { Name = "SVKT Rain", Season = Season, ExternalID = 23589, ExternalGroupID = 8297 };
            context.Teams.Add(Team);
            context.SaveChanges();
        }

        private void SeedRoles(DatabaseContext context)
        {
            Role_Admin = new Role { IsBuiltIn = true, Name = "Administrator", Description = "Built-in administrator role", IsDefaultAdminRole = true };
            Role_TeamAdmin = new Role { IsBuiltIn = true, Name = "Team-Administrator", Description = "Built-in team administrator role", IsDefaultTeamAdminRole = true };
            Role_User = new Role { IsBuiltIn = true, Name = "User", Description = "Built-in user role", IsDefaultUserRole = true };
            context.Roles.Add(Role_Admin);
            context.Roles.Add(Role_TeamAdmin);
            context.Roles.Add(Role_User);
            context.SaveChanges();
        }

        private void SeedUsers(DatabaseContext context)
        {
            User_Admin = new User { Name = "Raphael", Surname = "Fleischlin", Password = "7562D3C24B642876D8C99C0D8D4FE836511BDC39BD71143BAD5A76A758E30640F1B97C4924E65796D30679A221C79E715D9F384159E5A9BAF47D9B3917D847CE", Salt = "E47CE43021D6A534DF8EE287FDC27ED67114BC1284C308156CCF8BCE89B0150689E4AA62F28F4AAC5A771C7D0568E9A859060B4BD724ED9C915439DFBEC1B4C0", Email = "admin@volleyrain.ch", IsApproved = true };
            User_Admin.Roles.Add(Role_Admin);
            User_Admin.Roles.Add(Role_TeamAdmin);
            User_Admin.Roles.Add(Role_User);
            context.Users.Add(User_Admin);
            context.SaveChanges();
        }

        private void SeedEventTypes(DatabaseContext context)
        {
            EventType_Training = new EventType { Name = "Training", ShortName = "TR", ColorCode = "#33B5E5" };
            EventType_Match = new EventType { Name = "Heimmatch", ShortName = "MH", ColorCode = "#9933CC" };
            var eventTypes = new List<EventType>
            {
                EventType_Training,
                EventType_Match,
                new EventType { Name = "Auswärtsmatch", ShortName = "MA", ColorCode = "#AA66CC" },
                new EventType { Name = "Turnier", ShortName = "TU", ColorCode = "#FFBB33" },
                new EventType { Name = "Ferien", ShortName = "F", ColorCode = "#FFFFFF" },
            };
            eventTypes.ForEach(e => context.EventTypes.Add(e));
            context.SaveChanges();
        }

        private void SeedAttendacneTypes(DatabaseContext context)
        {
            AttendanceType_Attending = new AttendanceType { Name = "Zusage", ShortName = "Zu", RepresentsAttendance = true, ColorCode = "#DFF0D8" };
            context.AttendanceTypes.Add(AttendanceType_Attending);
            AttendanceType_Absent = new AttendanceType { Name = "Absage", ShortName = "Ab", RepresentsAttendance = false, ColorCode = "#F2DEDE" };
            context.AttendanceTypes.Add(AttendanceType_Absent);
            var attendanceTypes = new List<AttendanceType>
            {
                new AttendanceType { Name = "Verletzt", ShortName = "V", RepresentsAttendance = false, ColorCode = "#D9EDF7" }
            };
            attendanceTypes.ForEach(a => context.AttendanceTypes.Add(a));
            context.SaveChanges();
        }

        protected override void Seed(DatabaseContext context)
        {
            SeedSeasons(context);
            SeedTeams(context);
            SeedRoles(context);
            SeedUsers(context);
            SeedEventTypes(context);
            SeedAttendacneTypes(context);
        }
    }
}