using IdentityServer4.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IT4GOV.IdentityServer.Configuration
{
    public class Scopes
    {
        public static IEnumerable<Scope> Get()
        {
            return new List<Scope>
            {
                StandardScopes.OpenId,
                StandardScopes.ProfileAlwaysInclude,
                StandardScopes.EmailAlwaysInclude,
                StandardScopes.OfflineAccess,
               // StandardScopes.RolesAlwaysInclude,
                StandardScopes.PhoneAlwaysInclude,

                // new Scope
                //{
                //    Name = "PersonalInformation",
                //    DisplayName = "Προσωπικά στοιχεία",
                //    Description = "Το επώνυμο, όνομα και πατρώνυμο του χρήστη",
                //    Type = ScopeType.Identity,
                //    Claims =  new List<ScopeClaim>
                //    {
                //        new ScopeClaim("Name"),
                //         new ScopeClaim("GivenName"),
                //          new ScopeClaim("FamilyName"),
                //           new ScopeClaim("BirthDate")
                //    }
                //},


                //new Scope
                //{
                //    Name = "ContactInformation",
                //    DisplayName = "Στοιχεία επικοινωνίας",
                //    Description = "Το κινητό, η διεύθυνση, το email του χρήστη",
                //    Type = ScopeType.Identity,
                //    Claims =  new List<ScopeClaim>
                //    {
                //        new ScopeClaim("Email"),
                //         new ScopeClaim("Address"),
                //          new ScopeClaim("PhoneNumber")
                //    }
                //},
                  new Scope
                {
                    Name = "AFMInformation",
                    DisplayName = "ΑΦΜ",
                    Description = "Το ΑΦΜ του χρήστη",
                    Type = ScopeType.Identity,
                    Claims =  new List<ScopeClaim>
                    {
                        new ScopeClaim("AFM")
                    }
                },

                 new Scope
                {
                    Name = "AMKAInformation",
                    DisplayName = "AMKA",
                    Description = "Το AMKA του χρήστη",
                    Type = ScopeType.Identity,
                    Claims =  new List<ScopeClaim>
                    {
                        new ScopeClaim("AMKA")
                    }
                },




                   new Scope
                {
                    Name = "ATInformation",
                    DisplayName = "Αριθμός αστυνομικής ταυτότητας",
                    Description = "O ΑT του χρήστη",
                    Type = ScopeType.Identity,
                    Claims =  new List<ScopeClaim>
                    {
                        new ScopeClaim("AT")
                    }
                },










                new Scope
                {
                    Name = "api1",
                    DisplayName = "API 1",
                    Description = "Μία άλλη υπηρεσία του δημοσίου όπου είναι Web API",
                    Type = ScopeType.Resource,

                    ScopeSecrets = new List<Secret>
                    {
                        new Secret("secret".Sha256())
                    },
                    Claims = new List<ScopeClaim>
                    {
                        new ScopeClaim("role")
                    }
                },
                new Scope
                {
                    Name = "api2",
                    DisplayName = "API 2",
                    Description = "Μία άλλη υπηρεσία του δημοσίου όπου είναι Web API",
                    Type = ScopeType.Resource
                }
            };
        }
    }
}
