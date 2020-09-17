using Caty.ContextMaster.Models;
using System.Collections.Generic;
using System.ServiceModel.Syndication;
using System.Threading.Tasks;
using System.Xml;

namespace Caty.ContextMaster.Common
{
    public static class RssCommon
    {
        public static RssFeed GetRssFeed(string rssUri)
        {
            SyndicationFeed sf = SyndicationFeed.Load(XmlReader.Create(rssUri));
            var feed = new RssFeed
            {
                Title = sf.Title.Text,
                FeedCode = sf.Id,
            };
            if (sf.Links.Count > 0)
            {
                feed.Link = $"{sf.Links[0].Uri}";
            }
            if (sf.Authors.Count > 0 && !string.IsNullOrEmpty(sf.Authors[0].Uri))
            {
                feed.Author = $"{sf.Authors[0].Uri}";
            }
            feed.LastUpdatedTime = sf.LastUpdatedTime.DateTime;
            var ItemList = new List<RssItem>();
            foreach (SyndicationItem it in sf.Items)
            {
                ItemList.Add(new RssItem()
                {
                    ItemId = it.Id,
                    Author = it.Authors[0].Name,
                    AuthorLink = it.Authors[0].Uri,
                    AuthorEmail = it.Authors[0].Email,
                    Title = it.Title.Text,
                    LastUpdatedTime = it.LastUpdatedTime.DateTime,
                    Summary = it.Summary.Text,
                    ContentLink = $"{it.Links[0].Uri}",
                    FeedId = feed.Id,
                    Feed = feed,
                });
            }
            feed.Items = ItemList;
            return feed;
        }

        public static IList<RssFeed> GetRssFeeds(List<string> rssUris)
        {
            var feedList = new List<RssFeed>();
            foreach(var str in rssUris)
            {
                feedList.Add(GetRssFeed(str));
            }
            return feedList;
        }
    }
}