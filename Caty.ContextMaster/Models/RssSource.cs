using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Caty.ContextMaster.Models
{
    public class RssSource : BaseEntity
    {

        public string RssName { get; set; }

        public string RssUrl { get; set; }

        public string Category { get; set; }
    }
}
