using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Caty.ContextMaster.Models
{
    public class RssDbContext : DbContext
    {
        public RssDbContext() { }

        public RssDbContext(DbContextOptions<RssDbContext> options) :base(options){ }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<RssFeed>()
                .HasMany(g => g.Items)
                .WithOne(s => s.Feed)
                .HasForeignKey(fk => fk.Id)
                .OnDelete(DeleteBehavior.NoAction);
        }

        public RssDbContext AddAuditInfo()
        {
            var entries = ChangeTracker.Entries().Where(x => x.Entity is BaseEntity && (x.State == EntityState.Added || x.State == EntityState.Modified));
            foreach (var entry in entries)
            {
                if (entry.State == EntityState.Added)
                {
                    ((BaseEntity)entry.Entity).CreateTime = DateTime.UtcNow;
                }
                ((BaseEntity)entry.Entity).UpdateTime = DateTime.UtcNow;
            }
            return this;
        }

        public async Task<int> SaveInfoAndChangesAsync()
        {
            AddAuditInfo();
            return await SaveChangesAsync().ConfigureAwait(false);
        }


        /// <summary>
        /// Rss聚合
        /// </summary>
        public virtual DbSet<RssFeed> RssFeeds { get; set; }

        /// <summary>
        /// Rss消息
        /// </summary>
        public virtual DbSet<RssItem> RssItems { get; set; }

        /// <summary>
        /// Rss源
        /// </summary>
        public virtual DbSet<RssSource> RssSources { get; set; }
    }
}
