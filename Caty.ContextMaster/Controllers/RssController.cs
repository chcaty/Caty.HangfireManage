using Caty.ContextMaster.Common;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Serialization;
using System.Text.Json;

namespace Caty.ContextMaster.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RssController : ControllerBase
    {
        public IActionResult GetCnBlog()
        {
            var feed = RssCommon.ShowRss("http://feed.cnblogs.com/blog/sitecateogry/108698/rss");
            var options = new JsonSerializerOptions
            {
                WriteIndented = true,
            };
            return new JsonResult(feed);
        }
    }
}
