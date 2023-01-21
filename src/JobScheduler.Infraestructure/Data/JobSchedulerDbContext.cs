using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobScheduler.Infraestructure.Data
{
    public class JobSchedulerDbContext : DbContext
    {
        public JobSchedulerDbContext(DbContextOptions options) : base(options) { }
        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    //optionsBuilder.UseLazyLoadingProxies().UseQueryTrackingBehavior(QueryTrackingBehavior.TrackAll)
        //    //    .UseSqlServer(_databaseSettings.ConnectionString, x => x.UseNetTopologySuite());
        //}

        public DbSet<RunHistory> RunHistory { get; set; } = null!;
    }
}
