using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IT4GOV.IdentityServer.Extensions
{
    public interface ISmsSender
    {
        Task SendSmsAsync(string number, string message);
    }
}
