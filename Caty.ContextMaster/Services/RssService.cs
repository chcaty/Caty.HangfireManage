using Caty.ContextMaster.Common;
using Caty.ContextMaster.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Caty.ContextMaster.Services
{
    public class RssService
    {
        public RssFeed GetRssFeed(string rssUrl)
        {
            return RssCommon.ShowRss(rssUrl);
        }
    }
}
