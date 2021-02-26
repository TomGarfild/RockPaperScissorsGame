using Microsoft.EntityFrameworkCore;

namespace Server.StatisticStorage
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
