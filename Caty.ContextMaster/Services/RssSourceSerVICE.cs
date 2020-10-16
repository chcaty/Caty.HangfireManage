using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Caty.ContextMaster.Models;
using Caty.ContextMaster.Repositorys;

namespace Caty.ContextMaster.Services
{
    public class RssSourceService : IRssSourceService
    {
        private readonly IGenericRepository<RssSource> _rssSourceRepository;

        public RssSourceService(IUnitOfWork<RssDbContext> unitOfWork)
        {
            _rssSourceRepository = unitOfWork.GetRepository<RssSource>();
        }

        public Task AddRssSource()
        {
            throw new NotImplementedException();
        }

        public Task UpdateRssSource()
        {
            throw new NotImplementedException();
        }

        public Task UpdateRssSourceIsEnabled()
        {
            throw new NotImplementedException();
        }
    }
}
