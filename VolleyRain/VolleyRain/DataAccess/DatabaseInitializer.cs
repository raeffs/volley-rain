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
            var newsArticles = new List<NewsArticle>
            {
                new NewsArticle { Title = "Kein Training heute!", Content = "Weil Vanessa krank...", PublishDate = DateTime.Now },
            };
            newsArticles.ForEach(a => context.NewsArticles.Add(a));
            context.SaveChanges();

            var roles = new List<Role>
            {
                new Role { Name = "User" },
                new Role { Name = "Administrator" },
            };
            roles.ForEach(r => context.Roles.Add(r));
            context.SaveChanges();

            var users = new List<User>
            {
                new User { Username = "Admin", Password = "12345" },
                new User { Username = "Susi", Password = "12345" },
            };
            users.Single(u => u.Username == "Admin").Roles.Add(roles.Single(r => r.Name == "Administrator"));
            users.Single(u => u.Username == "Susi").Roles.Add(roles.Single(r => r.Name == "User"));
            users.ForEach(u => context.Users.Add(u));
            context.SaveChanges();
        }
    }
}