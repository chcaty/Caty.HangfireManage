using Caty.ContextMaster.Common;
using Caty.ContextMaster.Common.Mail;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Serialization;
using System.Text.Json;

namespace Caty.ContextMaster.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RssController : ControllerBase
    {
        private readonly IMailer _mailer;

        public RssController(IMailer mailer)
        {
            _mailer = mailer;
        }

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
