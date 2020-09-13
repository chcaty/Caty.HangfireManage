using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Caty.ContextMaster.Models
{
    /// <summary>
    /// Rss源
    /// </summary>
    public class RssSource : BaseEntity
    {
        /// <summary>
        /// Rss名称
        /// </summary>
        public string RssName { get; set; }

        /// <summary>
        /// Rss链接
        /// </summary>
        public string RssUrl { get; set; }

        /// <summary>
        /// Rss分类
        /// </summary>
        public string Category { get; set; }
    }
}
