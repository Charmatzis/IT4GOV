using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace IT4GOV.IdentityServer.Extensions
{
    public class VerifyCodeViewModel
    {
        [Required]
        public string Provider { get; set; }

        [Required]
        public string Code { get; set; }

        [Required]
        public string RecievedCode { get; set; }

        

        public string ReturnUrl { get; set; }

        [Display(Name = "Να θυμάμαι αυτόν τον browser?")]
        public bool RememberBrowser { get; set; }

        [Display(Name = "Αποθήκευση?")]
        public bool RememberMe { get; set; }
    }
}
