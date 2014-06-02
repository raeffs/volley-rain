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

            var susi = new User { Username = "Susi", Password = "12345", Email = "susi@volleyrain.ch", IsApproved = true };
            susi.Roles.Add(userRole);
            context.Users.Add(susi);
            context.SaveChanges();
        }
    }
}