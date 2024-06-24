using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrganizeApp.Application.Common.Interfaces
{
    public interface IEmail
    {
        Task Send(string subject, string body, string to, string attachmentPath = null);
    }
}
