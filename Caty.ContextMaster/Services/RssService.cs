using Caty.ContextMaster.Common;
using Caty.ContextMaster.Models;
using Caty.ContextMaster.Repositorys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Caty.ContextMaster.Services
{
    public class RssService : IRssService
    {
        private readonly IGenericRepository<RssSource> _rssSourceRepository;

        public RssService(IUnitOfWork<RssDbContext> unitOfWork)
        {
            _rssSourceRepository = unitOfWork.GetRepository<RssSource>();
        }

        public RssFeed GetRssFeed(string rssUri)
        {
            return RssCommon.GetRssFeed(rssUri);
        }

        public async Task<IList<RssFeed>> GetRssFeedListAsync()
        {
            var rssSourceList = (await _rssSourceRepository.List(t => t.IsEnabled == true)).Select(t => t.RssUrl).ToList();
            return RssCommon.GetRssFeeds(rssSourceList);
        }
    }
}
