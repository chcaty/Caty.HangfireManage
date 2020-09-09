using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Caty.ContextMaster.Common.Mail
{
    public interface IMailer
    {
        Task SendMailAsync(string email, string subject, string body);
    }
}
