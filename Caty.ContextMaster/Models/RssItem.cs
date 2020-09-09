using System;
using System.Text.Json.Serialization;

namespace Caty.ContextMaster.Models
{
    public class RssItem : BaseEntity
    {
        public string Author { get; set; }

        public string AuthorLink { get; set; }

        public string AuthorEmail { get; set; }

        public string Title { get; set; }

        public string Summary { get; set; }

        public DateTime LastUpdatedTime { get; set; }

        public string ItemId { get; set; }

        public string ContentLink { get; set; }

        [JsonIgnore]
        public string FeedId { get; set; }

        [JsonIgnore]
        public virtual RssFeed Feed { get; set; }
    }
}