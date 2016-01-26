using Boxing.Core.Sql.Configurations;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Boxing.Core.Sql
{
    public class BoxingContext : DbContext
    {
        public BoxingContext()
            : base("User ID=sa;Password=123x456;Initial Catalog = BoxingApp; Server=KONSTANTIN-HP")
        //: base("User ID=kztodorov;Password=X123x4567;Initial Catalog = boxing; Server=boxingserver.database.windows.net")
        {
            this.Configuration.LazyLoadingEnabled = false;
        }

        public DbSet<MatchEntity> Matches { get; set; }
        public DbSet<BoxerEntity> Boxers { get; set; }
        public DbSet<UserEntity> Users { get; set; }
        public DbSet<LoginEntity> Logins { get; set; }
        public DbSet<PredictionEntity> Predictions { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Configurations.Add(new BoxerConfiguration());
            modelBuilder.Configurations.Add(new MatchConfiguration());
            modelBuilder.Configurations.Add(new UserConfiguration());
            modelBuilder.Configurations.Add(new LoginConfiguration());
            modelBuilder.Configurations.Add(new PredictionConfiguration());
        }

        public static void SetInitializer()
        {
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<BoxingContext, Migrations.Configuration>());
        }
    }
}
