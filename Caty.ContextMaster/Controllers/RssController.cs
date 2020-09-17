using Caty.ContextMaster.Common;
using Caty.ContextMaster.Common.Mail;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Serialization;
using System.Text.Json;
using System.Threading.Tasks;

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

        [HttpGet]
        public IActionResult GetCnBlogAsync()
        {
            var feed = RssCommon.GetRssFeed("http://feed.cnblogs.com/blog/sitecateogry/108698/rss");
            return new JsonResult(feed);
        }

        [HttpGet("Export")]
        public async Task<IActionResult> ExportRssReport()
        {
            var feed = RssCommon.GetRssFeed("http://feed.cnblogs.com/blog/sitecateogry/108698/rss");
            await _mailer.SendMailAsync("1120873075@qq.com", "Rss Report", JsonSerializer.Serialize(feed));
            return Ok();
        }
    }
}
