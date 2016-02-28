using Microsoft.AspNet.Mvc.Rendering;
using System.Collections.Generic;

namespace IT4GOV.IdentityServer.Extensions
{
    public class SendCodeViewModel
    {
        public string SelectedProvider { get; set; }

        public ICollection<SelectListItem> Providers { get; set; }

        public string ReturnUrl { get; set; }

        public bool RememberMe { get; set; }
    }
}
