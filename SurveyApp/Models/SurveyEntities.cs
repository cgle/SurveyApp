using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Web;

namespace SurveyApp.Models
{
    public class SurveyEntities : DbContext
    {
        public DbSet<UserProfile> UserProfiles { get; set; }
        public DbSet<Question> Questions { get; set; }
        public DbSet<Response> Responses { get; set; }
        public DbSet<Survey> Surveys { get; set; }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<SurveyEntities, Configuration>());
            base.OnModelCreating(modelBuilder);
        }
        internal sealed class Configuration : DbMigrationsConfiguration<SurveyEntities>
        {
           public Configuration()
           {
              AutomaticMigrationsEnabled = true;
           }
        }
    }



<<<<<<< HEAD
}
=======
}
>>>>>>> 2nd commit
