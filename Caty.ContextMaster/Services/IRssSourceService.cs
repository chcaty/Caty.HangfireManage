using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Caty.ContextMaster.Services
{
    public interface IRssSourceService
    {
        Task AddRssSource();

        Task UpdateRssSource();

        Task UpdateRssSourceIsEnabled();
    }
}
