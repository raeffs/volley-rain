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
        protected override void Seed(DatabaseContext context)
        {
            // built-in data
            var adminRole = new Role { IsBuiltIn = true, Name = "Administrator", Description = "Built-in administrator role" };
            var userRole = new Role { IsBuiltIn = true, Name = "User", Description = "Built-in user role" };
            context.Roles.Add(adminRole);
            context.Roles.Add(userRole);
            context.SaveChanges();

            var adminUser = new User { Username = "Admin", Password = "12345", Email = "admin@volleyrain.ch", IsApproved = true };
            adminUser.Roles.Add(adminRole);
            adminUser.Roles.Add(userRole);
            context.Users.Add(adminUser);
            context.SaveChanges();

            // test data
            var newsArticles = new List<NewsArticle>
            {
                new NewsArticle { Title = "Kein Training heute!", Content = "Weil Vanessa krank...", PublishDate = DateTime.Now },
            };
            newsArticles.ForEach(a => context.NewsArticles.Add(a));
            context.SaveChanges();

            var events = new List<Event>
            {
                new Event { Name = "Training 1", Date = new DateTime(2014, 6, 12) },
                new Event { Name = "Training 2", Date = new DateTime(2014, 6, 19) },
            };
            events.ForEach(e => context.Events.Add(e));
            context.SaveChanges();

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

            var susi = new User { Username = "Susi", Password = "12345", Email = "susi@volleyrain.ch", IsApproved = true };
            susi.Roles.Add(userRole);
            context.Users.Add(susi);
            context.SaveChanges();
        }
    }
}