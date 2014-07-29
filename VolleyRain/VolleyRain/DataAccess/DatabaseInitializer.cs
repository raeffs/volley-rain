using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using VolleyRain.Models;

namespace VolleyRain.DataAccess
{
    public class DatabaseInitializer : DropCreateDatabaseIfModelChanges<DatabaseContext>
    {
        private Season Season;

        private Team Team;

        private Role Role_Admin;
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
            Role_Admin = new Role { IsBuiltIn = true, Name = "Administrator", Description = "Built-in administrator role" };
            Role_User = new Role { IsBuiltIn = true, Name = "User", Description = "Built-in user role" };
            context.Roles.Add(Role_Admin);
            context.Roles.Add(Role_User);
            context.SaveChanges();
        }

        private void SeedUsers(DatabaseContext context)
        {
            User_Admin = new User { Username = "Admin", Password = "12345", Email = "admin@volleyrain.ch", IsApproved = true };
            User_Admin.Roles.Add(Role_Admin);
            User_Admin.Roles.Add(Role_User);
            context.Users.Add(User_Admin);
            context.SaveChanges();

            var users = new List<User>
            {
                new User { Username = "Susi", Password = "12345", Email = "susi@volleyrain.ch", IsApproved = true },
                new User { Username = "Deborah", Password = "12345", Email = "deborah@volleyrain.ch", IsApproved = true },
                new User { Username = "Christina", Password = "12345", Email = "christina@volleyrain.ch", IsApproved = true },
                new User { Username = "Jessica", Password = "12345", Email = "jessica@volleyrain.ch", IsApproved = true },
                new User { Username = "Jennifer", Password = "12345", Email = "jennifer@volleyrain.ch", IsApproved = true },
                new User { Username = "Nathalie", Password = "12345", Email = "nathalie@volleyrain.ch", IsApproved = true },
                new User { Username = "Adriana", Password = "12345", Email = "adriana@volleyrain.ch", IsApproved = true },
                new User { Username = "Michelle", Password = "12345", Email = "michelle@volleyrain.ch", IsApproved = true },
                new User { Username = "Nicole", Password = "12345", Email = "nicole@volleyrain.ch", IsApproved = true },
                new User { Username = "Sophie", Password = "12345", Email = "sophie@volleyrain.ch", IsApproved = true },
            };
            users.ForEach(u =>
            {
                u.Roles.Add(Role_User);
                u.Teams.Add(Team);
                context.Users.Add(u);
            });
            context.SaveChanges();
        }

        private void SeedNewsArticles(DatabaseContext context)
        {
            var data = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Integer fringilla mollis risus dictum convallis. Phasellus semper velit dolor, eget vulputate ante consectetur at. Mauris a nunc malesuada quam tristique tincidunt. Ut purus leo, auctor eu fermentum nec, tempus id dolor. Cras facilisis, dui in laoreet sollicitudin, nisi tellus consectetur sapien.";
            var dataArray = data.Split(' ');
            for (var index = 0; index <= 100; index++)
            {
                context.NewsArticles.Add(new NewsArticle
                {
                    Title = string.Format("News Item {0}", index),
                    Content = string.Concat(dataArray.Skip(index % 25).Take(index % 10 + 10).Select(s => s + " ")),
                    PublishDate = DateTime.Now.AddDays(index * -1)
                });
            }
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

        private void SeedEvents(DatabaseContext context)
        {
            var events = new List<Event>
            {
                new Event { Name = "Before month", Start = new DateTime(2014, 6, 30, 10, 0, 0), End = new DateTime(2014, 6, 30, 21, 59, 59), Type = EventType_Match },
                new Event { Name = "End touching month", Start = new DateTime(2014, 6, 30, 10, 0, 0), End = new DateTime(2014, 6, 30, 23, 59, 59), Type = EventType_Match },
                new Event { Name = "End inside month", Start = new DateTime(2014, 6, 30, 10, 0, 0), End = new DateTime(2014, 7, 1, 21, 59, 59), Type = EventType_Match },
                new Event { Name = "Inside start touching month", Start = new DateTime(2014, 7, 1, 0, 0, 0), End = new DateTime(2014, 7, 1, 21, 59, 59), Type = EventType_Match },
                new Event { Name = "Inside month", Start = new DateTime(2014, 7, 4, 10, 0, 0), End = new DateTime(2014, 7, 4, 21, 59, 59), Type = EventType_Match },
                new Event { Name = "Inside end touching month", Start = new DateTime(2014, 7, 31, 10, 0, 0), End = new DateTime(2014, 7, 31, 23, 59, 59), Type = EventType_Match },
                new Event { Name = "Start inside month", Start = new DateTime(2014, 7, 31, 10, 0, 0), End = new DateTime(2014, 8, 1, 21, 59, 59), Type = EventType_Match },
                new Event { Name = "Start touching month", Start = new DateTime(2014, 8, 1, 0, 0, 0), End = new DateTime(2014, 8, 1, 21, 59, 59), Type = EventType_Match },
                new Event { Name = "After month", Start = new DateTime(2014, 8, 1, 10, 0, 0), End = new DateTime(2014, 8, 1, 21, 59, 59), Type = EventType_Match },
            };
            events.ForEach(e => context.Events.Add(e));

            for (var index = 0; index <= 10; index++)
            {
                context.Events.Add(new Event
                {
                    Name = string.Format("Training {0}", index),
                    Type = EventType_Training,
                    Start = DateTime.Today.AddDays(index * 7).AddHours(18),
                    End = DateTime.Today.AddDays(index * 7).AddHours(20),
                });
            }

            for (var index = 0; index <= 5; index++)
            {
                context.Events.Add(new Event
                {
                    Name = string.Format("Spiel {0}", index),
                    Type = EventType_Match,
                    Start = DateTime.Today.AddDays(2 + index * 14).AddHours(10),
                    End = DateTime.Today.AddDays(2 + index * 14).AddHours(14),
                });
            }

            context.SaveChanges();
        }

        private void SeedRankings(DatabaseContext context)
        {
            var rankings = new List<Ranking>
            {
                new Ranking { Rank = 1, Team = "VBC Willisau 2", NumberOfGames = 14, SetsWon = 35, SetsLost = 21, SetQuotient = 1.7M, BallsWon = 1213, BallsLost = 1113, BallQuotient = 1.1M, Points = 28 },
                new Ranking { Rank = 2, Team = "SVKT Rain", NumberOfGames = 14, SetsWon = 36, SetsLost = 23, SetQuotient = 1.6M, BallsWon = 1301, BallsLost = 1130, BallQuotient = 1.2M, Points = 28 },
                new Ranking { Rank = 3, Team = "VBC Ettiswil", NumberOfGames = 14, SetsWon = 31, SetsLost = 24, SetQuotient = 1.3M, BallsWon = 1186, BallsLost = 1126, BallQuotient = 1.1M, Points = 24 },
                new Ranking { Rank = 4, Team = "Gunzwil SVKT", NumberOfGames = 14, SetsWon = 31, SetsLost = 26, SetQuotient = 1.2M, BallsWon = 1186, BallsLost = 1133, BallQuotient = 1.1M, Points = 24 },
                new Ranking { Rank = 5, Team = "SV Sempach", NumberOfGames = 14, SetsWon = 30, SetsLost = 27, SetQuotient = 1.1M, BallsWon = 1215, BallsLost = 1189, BallQuotient = 1M, Points = 23 },
                new Ranking { Rank = 6, Team = "VBC Sursee 3", NumberOfGames = 14, SetsWon = 28, SetsLost = 30, SetQuotient = 0.9M, BallsWon = 1178, BallsLost = 1250, BallQuotient = 0.9M, Points = 20 },
                new Ranking { Rank = 7, Team = "VBC Luzern 2", NumberOfGames = 14, SetsWon = 27, SetsLost = 30, SetQuotient = 0.9M, BallsWon = 1212, BallsLost = 1202, BallQuotient = 1M, Points = 20 },
                new Ranking { Rank = 8, Team = "VBC Malters 3", NumberOfGames = 14, SetsWon = 5, SetsLost = 42, SetQuotient = 0.1M, BallsWon = 809, BallsLost = 1157, BallQuotient = 0.7M, Points = 1 },
            };
            rankings.ForEach(r => context.Rankings.Add(r));
            context.SaveChanges();
        }

        private void SeedAttendacneTypes(DatabaseContext context)
        {
            AttendanceType_Attending = new AttendanceType { Name = "Zusage", ShortName = "Zu", RepresentsAttendance = true, ColorCode = "#DFF0D8" };
            AttendanceType_Absent = new AttendanceType { Name = "Absage", ShortName = "Ab", RepresentsAttendance = false, ColorCode = "#F2DEDE" };
            var attendanceTypes = new List<AttendanceType>
            {
                new AttendanceType { Name = "Verletzt", ShortName = "V", RepresentsAttendance = false, ColorCode = "#D9EDF7" }
            };
            attendanceTypes.ForEach(a => context.AttendanceTypes.Add(a));
            context.SaveChanges();
        }

        private void SeedAttendances(DatabaseContext context)
        {
            var rand = new Random();
            foreach (var e in context.Events)
            {
                foreach (var u in context.Users)
                {
                    AttendanceType type = null;
                    switch (rand.Next(3))
                    {
                        case 0: continue;
                        case 1:
                            type = AttendanceType_Attending;
                            break;
                        case 2:
                            type = AttendanceType_Absent;
                            break;
                    }

                    context.Attendances.Add(new Attendance
                    {
                        Event = e,
                        User = u,
                        Type = type
                    });
                }
            }
            context.SaveChanges();
        }

        protected override void Seed(DatabaseContext context)
        {
            SeedSeasons(context);
            SeedTeams(context);
            SeedRoles(context);
            SeedUsers(context);
            SeedNewsArticles(context);
            SeedEventTypes(context);
            SeedEvents(context);
            SeedRankings(context);
            SeedAttendacneTypes(context);
            SeedAttendances(context);
        }
    }
}