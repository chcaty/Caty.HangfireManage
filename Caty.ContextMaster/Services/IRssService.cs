using Caty.ContextMaster.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Caty.ContextMaster.Services
{
    public interface IRssService
    {
        public RssFeed GetRssFeed(string rssUri);

        public Task<IList<RssFeed>> GetRssFeedListAsync();

    }
}
