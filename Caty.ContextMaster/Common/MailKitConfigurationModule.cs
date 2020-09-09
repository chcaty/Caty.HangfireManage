using MailKit.Net.Smtp;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Caty.ContextMaster.Common
{
    public static class MailKitConfigurationModule
    {
        public static SmtpClient GetSmtpClient(IConfiguration configuration)
        {
            var client = new SmtpClient();
            client.Connect(configuration["MailKit:ConnectionStr"], 587, false);
            client.Authenticate(configuration["MailKit:Authenticate:User"], configuration["MailKit:ConnectionStr"]);
            return client;
        }
    }
}
