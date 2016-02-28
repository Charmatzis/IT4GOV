using IdentityServer4.Core.Models;
using IT4GOV.IdentityServer.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IT4GOV.IdentityServer.Configuration
{
    public class Clients
    {
        public static IEnumerable<Client> Get()
        {
            return new List<Client>
            {
                

                ///////////////////////////////////////////
                // MVC Implicit Flow Samples
                //////////////////////////////////////////
                new Client
                {
                    ClientId = "WebApp",
                    ClientName = "IT4GOV.WebApp",
                    Flow = Flows.Implicit,
                    RedirectUris = new List<string>
                    {
                        URLS.Mvc_url
                    },

                    AllowedScopes = new List<string>
                    {
                        StandardScopes.OpenId.Name,
                        StandardScopes.Profile.Name,
                        StandardScopes.Email.Name,
                        //StandardScopes.Roles.Name,
                        StandardScopes.Phone.Name,

                        "api1", "api2",
                        //, "PersonalInformation"
                        //, "ContactInformation",
                        "ATInformation", "AFMInformation", "AMKAInformation"
                    }
                },

                ///////////////////////////////////////////
                // JS OAuth 2.0 Sample
                //////////////////////////////////////////
                new Client
                {
                    ClientId = "js_oauth",
                    ClientName = "JavaScript OAuth 2.0 Client",

                    Flow = Flows.Implicit,
                    RedirectUris = new List<string>
                    {
                        URLS.Html_url
                    },

                    AllowedScopes = new List<string>
                    {
                        "api1", "api2"
                    }
                }


            };
        }
    }
}
