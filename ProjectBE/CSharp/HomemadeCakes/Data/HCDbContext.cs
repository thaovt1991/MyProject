using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HomemadeCakes.Data
{
    public class HCDbContext : DbContext
    {
        public HCDbContext(DbContextOptions options) : base (options)
        {

        }
    }
}
