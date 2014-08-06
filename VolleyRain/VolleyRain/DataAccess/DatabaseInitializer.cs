using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using VolleyRain.Migrations;

namespace VolleyRain.DataAccess
{
    public class DatabaseInitializer : MigrateDatabaseToLatestVersion<DatabaseContext, Configuration>
    {
    }
}