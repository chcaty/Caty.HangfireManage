using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Caty.ContextMaster.Models
{
    public class RssDbContextFactory:DesignTimeDbContextFactoryBase<RssDbContext>
    {
        protected override RssDbContext CreateNewInstance(DbContextOptions<RssDbContext> options)
        {
            return new RssDbContext(options);
        }
    }
}
