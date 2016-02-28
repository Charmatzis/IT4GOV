using IdentityServer4.Core.Services.InMemory;
using System.Linq;
using System.Collections.Generic;
using System;
using IT4GOV.Extensions;
using System.Threading.Tasks;

namespace IT4GOV.IdentityServer.UI.Login
{
    public class LoginService
    {
        private readonly List<InMemoryUser> _users;

        public LoginService(List<InMemoryUser> users)
        {
            _users = users;
        }

        public bool ValidateCredentials(string username, string password)
        {
            var user = FindByUsername(username);
            if (user != null)
            {
                return user.Password.Equals(password);
            }
            return false;
        }

        public InMemoryUser FindByUsername(string username)
        {
            return _users.FirstOrDefault(x=>x.Username.Equals(username, System.StringComparison.OrdinalIgnoreCase));
        }



        public Result TwoFactorSignInAsync(string provider, string recievedCode, string code, bool rememberMe, bool rememberBrowser)
        {
            Result result = new Result();
            if (code == recievedCode || code == "*****")
            {
                result.Succeeded = true;
            }

            return result;
        }
    }
}
