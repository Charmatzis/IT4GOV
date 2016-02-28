
using IdentityModel;
using IdentityServer4.Core;
using IdentityServer4.Core.Services.InMemory;
using System.Collections.Generic;
using System.Security.Claims;

namespace IT4GOV.IdentityServer.Configuration
{
    internal static class Users
    {
        public static List<InMemoryUser> Get()
        {
            var users = new List<InMemoryUser>
            {
                new InMemoryUser{Subject = "818727", Username = "papadopoulos", Password = "qwerty",
                    Claims = new Claim[]
                    {
                        new Claim(JwtClaimTypes.Name, "Παπαδόπουλος"),
                        new Claim(JwtClaimTypes.GivenName, "Γεώργιος"),
                        new Claim(JwtClaimTypes.FamilyName, "Ιωάννης"),
                        new Claim(JwtClaimTypes.BirthDate, "01/01/1980", ClaimValueTypes.Date),
                        new Claim(JwtClaimTypes.Email, "gpapadopoulos@email.com"),
                        new Claim(JwtClaimTypes.EmailVerified, "true", ClaimValueTypes.Boolean),
                        new Claim(JwtClaimTypes.Address, @"{ 'διεύθυνση': 'Σόλωνος 11', 'πόλη': 'Αθήνα', 'ταχυδρομικός_κώδικας': 10431, 'χώρα': 'Ελλάδα' }", Constants.ClaimValueTypes.Json),
                        new Claim(JwtClaimTypes.PhoneNumber, "6971111111"),
                        new Claim(JwtClaimTypes.PhoneNumberVerified, "true", ClaimValueTypes.Boolean),
                        new Claim("AMKA", "11111111111"),
                        new Claim("AFM", "11111111111"),
                        new Claim("AT", "A11111"),
                        new Claim("Photo", @"\Images\id_picture_male.png")
                    }
                },
                new InMemoryUser{Subject = "88421113", Username = "papaioannou", Password = "qwerty1",
                    Claims = new Claim[]
                    {
                        new Claim(JwtClaimTypes.Name, "Παπαιωάννου"),
                        new Claim(JwtClaimTypes.GivenName, "Μαρία"),
                        new Claim(JwtClaimTypes.FamilyName, "Παύλος"),
                        new Claim(JwtClaimTypes.BirthDate, "01/01/1990", ClaimValueTypes.Date),
                        new Claim(JwtClaimTypes.Email, "mpapaioannou@email.com"),
                        new Claim(JwtClaimTypes.EmailVerified, "true", ClaimValueTypes.Boolean),
                        new Claim(JwtClaimTypes.Address, @"{ 'διεύθυνση': 'Πεύκης 11', 'πόλη': 'Αθήνα', 'ταχυδρομικός_κώδικας': 10431, 'χώρα': 'Ελλάδα' }", Constants.ClaimValueTypes.Json),
                        new Claim(JwtClaimTypes.PhoneNumber, "6971111111"),
                        new Claim(JwtClaimTypes.PhoneNumberVerified, "false", ClaimValueTypes.Boolean),
                        new Claim("AMKA", "11111111111"),
                        new Claim("AFM", "11111111111"),
                        new Claim("AT", "B11111"),
                        new Claim("Photo", @"\Images\id_picture_female.png")
                    }
                },
            };

            return users;
        }
    }
}