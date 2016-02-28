using IdentityServer4.Core.Services.InMemory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IT4GOV.IdentityServer.UI.SignUp
{
    public class SignUpService
    {

        private  List<InMemoryUser> _users;

        public SignUpService(List<InMemoryUser> users)
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
            return _users.FirstOrDefault(x => x.Username.Equals(username, System.StringComparison.OrdinalIgnoreCase));
        }


        public InMemoryUser FindUserPhoneNumber(string username)
        {
            return _users.FirstOrDefault(x => x.Username.Equals(username, System.StringComparison.OrdinalIgnoreCase));
        }


        public  bool Add(InMemoryUser inMemoryUser)
        {
            var user = FindByUsername(inMemoryUser.Username);
            if (user == null)
            {
                _users.Add(inMemoryUser);
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
