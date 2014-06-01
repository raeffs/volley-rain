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
                new NewsArticle { Title = "Kein Training heute!", Content = "Weil Vanessa krank...", PublishDate = DateTime.Now }
            };
            newsArticles.ForEach(a => context.NewsArticles.Add(a));
            context.SaveChanges();
        }
    }
}