using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Server.StatisticStorege
{
    public class StatisticContext:DbContext
    {
        public DbSet<StatisticItem> StatisticItems { get; set; }

        public StatisticContext(DbContextOptions<StatisticContext> options)
            : base(options)
        {
            Database.EnsureCreated();
        }
    }
}
