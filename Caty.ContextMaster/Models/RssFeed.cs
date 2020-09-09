using System;
using System.Collections.Generic;

namespace Caty.ContextMaster.Models
{
    public class RssFeed : BaseEntity
    {
        public string Title { get; set; }

        public string Link { get; set; }

        public string Author { get; set; }

        public string Description { get; set; }

        public DateTime LastUpdatedTime { get; set; }

        public string Generator { get; set; }

        public string FeedId { get; set; }

        public virtual ICollection<RssItem> Items { get; set; }
    }
}