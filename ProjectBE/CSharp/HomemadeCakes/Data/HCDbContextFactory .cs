using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace HomemadeCakes.Data
{
    public class HCDbContextFactory : IDesignTimeDbContextFactory<HCDbContext>
    {
        public HCDbContext CreateDbContext(string[] args)
        {
            //lay thu muc goc ra 1 bac
            IConfigurationRoot configuration = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json").Build();

            var conectionString = configuration.GetConnectionString("MongoDB");
            var optionsBuilder = new DbContextOptionsBuilder<HCDbContext>();
           // optionsBuilder.UseSqlServer(optionsBuilder);

            return new HCDbContext(optionsBuilder.Options);
        }
    }
}