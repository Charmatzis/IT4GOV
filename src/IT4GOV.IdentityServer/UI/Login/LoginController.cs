using IdentityModel;
using IdentityServer4.Core;
using IdentityServer4.Core.Services;
using IT4GOV.Extensions;
using IT4GOV.IdentityServer.Extensions;
using IT4GOV.IdentityServer.UI.Home;
using Microsoft.AspNet.Authorization;
using Microsoft.AspNet.Mvc;
using Microsoft.AspNet.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace IT4GOV.IdentityServer.UI.Login
{
    public class LoginController : Controller
    {
        private readonly LoginService _loginService;
        private readonly SignInInteraction _signInInteraction;
        private readonly ISmsSender _smsSender;
        private readonly ILogger _logger;

        public LoginController(
            LoginService loginService, 
            SignInInteraction signInInteraction, ISmsSender smsSender, ILoggerFactory loggerFactory)
        {
            _loginService = loginService;
            _signInInteraction = signInInteraction;
            _smsSender = smsSender;
            _logger = loggerFactory.CreateLogger<LoginController>();
        }

        [HttpGet(Constants.RoutePaths.Login, Name = "Login")]
        public async Task<IActionResult> Index(string id)
        {
            var vm = new LoginViewModel();

            if (id != null)
            {
                var request = await _signInInteraction.GetRequestAsync(id);
                if (request != null)
                {
                    vm.Username = request.LoginHint;
                    vm.SignInId = id;
                }
            }

            return View(vm);
        }

        [HttpPost(Constants.RoutePaths.Login)]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(LoginInputModel model)
        {
            if (ModelState.IsValid)
            {
                if (_loginService.ValidateCredentials(model.Username, model.Password))
                {
                    var user = _loginService.FindByUsername(model.Username);
                    var phoneNumber = user.Claims.Where(x => x.Type == JwtClaimTypes.PhoneNumber).Select(x => x.Value).FirstOrDefault();

                    var name = user.Claims.Where(x => x.Type == JwtClaimTypes.Name).Select(x => x.Value).FirstOrDefault() ?? user.Username;

                    var claims = new Claim[] {
                        new Claim(JwtClaimTypes.Subject, user.Subject),
                        new Claim(JwtClaimTypes.PhoneNumber, phoneNumber),
                        new Claim(JwtClaimTypes.Name, name),
                        new Claim(JwtClaimTypes.IdentityProvider, "idsvr"),
                        new Claim(JwtClaimTypes.AuthenticationTime, DateTime.UtcNow.ToEpochTime().ToString()),
                    };
                    var ci = new ClaimsIdentity(claims, "password", JwtClaimTypes.Name, JwtClaimTypes.Role);
                    var cp = new ClaimsPrincipal(ci);

                    await HttpContext.Authentication.SignInAsync(Constants.PrimaryAuthenticationType, cp);

                    if (model.SignInId != null)
                    {
                        return RedirectToAction("SendCode", new { returnUrl = model.SignInId,  rememberMe = model.RememberLogin});
                        //return new SignInResult(model.SignInId);
                    }

                    return Redirect("~/");
                }

                ModelState.AddModelError("", "Μη έγκυρο όνομα χρήστη ή κωδικός.");
            }

            var vm = new LoginViewModel(model);
            return View(vm);
        }





        [Authorize]
        [HttpGet]
        public IActionResult SendCode( string returnUrl = null, bool rememberMe = false)
        {
            //if (_loginService.ValidateCredentials(model.Username, model.Password))
            //{
            //user = _loginService.FindByUsername(model.Username);
            if (User == null)
            {
                return View("Error");
            }
            //}
            //var user = await _signInManager.GetTwoFactorAuthenticationUserAsync();

            //var userFactors = await _userManager.GetValidTwoFactorProvidersAsync(user);
            var factorOptions = new List<SelectListItem> { new SelectListItem() { Text = "Επαλήθευση αριθμού κινητού", Value = "Phone" } };
            return View(new SendCodeViewModel { Providers = factorOptions, ReturnUrl = returnUrl, RememberMe = rememberMe });
        }

        //
        // POST: /Account/SendCode
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SendCode(SendCodeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }


            if (User == null)
            {
                return View("Error");
            }



            string code = Guid.NewGuid().ToString("N").Substring(0, 5);
            var message = "Ο κωδικός σας είναι: " + code;
            if (model.SelectedProvider == "Email")
            {

            }
            else if (model.SelectedProvider == "Phone")
            {
                var phoneNumber = User.Claims.Where(x => x.Type == JwtClaimTypes.PhoneNumber).Select(x => x.Value).FirstOrDefault();


                await _smsSender.SendSmsAsync(phoneNumber, message);
            }

            return RedirectToAction(nameof(VerifyCode), new { RecievedCode = code, Provider = model.SelectedProvider, ReturnUrl = model.ReturnUrl, RememberMe = model.RememberMe });
        }

        [Authorize]
        [HttpGet]
        public IActionResult VerifyCode(string code, string provider, bool rememberMe, string returnUrl = null)
        {
            
            if (User == null)
            {
                return View("Error");
            }
            return View(new VerifyCodeViewModel { RecievedCode = code, Provider = provider, ReturnUrl = returnUrl, RememberMe = rememberMe });
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult VerifyCode(VerifyCodeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            
            Result result = _loginService.TwoFactorSignInAsync(model.Provider, model.RecievedCode, model.Code, model.RememberMe, model.RememberBrowser);
            if (result.Succeeded)
            {
                return new SignInResult(model.ReturnUrl);
                //return RedirectToLocal(model.ReturnUrl);
            }
            if (result.IsLockedOut)
            {
                _logger.LogWarning(7, "Ο χρήστης έχει απενεργοποιηθεί");
                return View("Lockout");
            }
            else
            {
                ModelState.AddModelError("", "Λάθος κωδικός.");
                return View(model);
            }
        }




        private IActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction(nameof(HomeController.Index), "Home");
            }
        }
    }
}
