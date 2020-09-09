using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Caty.ContextMaster.Models
{
    //https://medium.com/@ffimnsr/sending-email-using-mailkit-in-asp-net-core-web-api-71b946380442
    public class SmtpSettings
    {
        public string Server { get; set; }

        public int Port { get; set; }

        public string SenderName { get; set; }

        public string SenderEmail { get; set; }

        public string Username { get; set; }

        public string Password { get; set; }


    }
}
