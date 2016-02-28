using IdentityModel;
using IdentityServer4.Core;
using IdentityServer4.Core.Services.InMemory;
using IT4GOV.Extensions;
using IT4GOV.IdentityServer.Extensions;
using IT4GOV.IdentityServer.UI.Home;
using IT4GOV.IdentityServer.UI.Login;
using Microsoft.AspNet.Authorization;
using Microsoft.AspNet.Hosting;
using Microsoft.AspNet.Http;
using Microsoft.AspNet.Mvc;
using Microsoft.AspNet.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using Microsoft.Net.Http.Headers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace IT4GOV.IdentityServer.UI.SignUp
{
    public class SignUpController : Controller
    {
        private SignUpService _signupService;
        private readonly LoginService _loginService;
        private readonly ISmsSender _smsSender;
        private readonly ILogger _logger;
        private readonly IHostingEnvironment _hostingEnvironment;

        public SignUpController(SignUpService signUpService, LoginService loginService,
            ISmsSender smsSender, ILoggerFactory loggerFactory, IHostingEnvironment hostingEnvironment)
        {
            _signupService = signUpService;
            _loginService = loginService;
            _smsSender = smsSender;
            _logger = loggerFactory.CreateLogger<SignUpController>();
            _hostingEnvironment = hostingEnvironment;

        }


        [HttpGet]
        [Route("/UI/SignUp/Register")]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [Route("/UI/SignUp/Register")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(SignUpModel model)
        {
            if (ModelState.IsValid)
            {
                var file = model.ImageUpload;
                var uploads = Path.Combine(_hostingEnvironment.WebRootPath, "Images");
                var filePathName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
                var fileExtention = Path.GetExtension(filePathName);
                var fileName = Guid.NewGuid().ToString("N").Substring(0, 6) + fileExtention;
                var path = Path.Combine(uploads, fileName);
                await file.SaveAsAsync(path);




                InMemoryUser inMemoryUser = new InMemoryUser()
                {
                    Username = model.Username,
                    Password = model.Password,
                    Subject = Guid.NewGuid().ToString("N").Substring(0, 6),
                    Claims = new[]
                    {
                        new Claim(JwtClaimTypes.Name, model.Name),
                        new Claim(JwtClaimTypes.GivenName, model.GivenName),
                        new Claim(JwtClaimTypes.FamilyName, model.FamilyName),
                        new Claim(JwtClaimTypes.BirthDate, model.BirthDate.Date.ToString(), ClaimValueTypes.Date),
                        new Claim(JwtClaimTypes.Email, model.Email),
                        new Claim(JwtClaimTypes.EmailVerified, model.EmailVerified.ToString(), ClaimValueTypes.Boolean),
                        new Claim(JwtClaimTypes.Address, model.Address, Constants.ClaimValueTypes.Json),
                        new Claim(JwtClaimTypes.PhoneNumber, model.PhoneNumber),
                        new Claim(JwtClaimTypes.PhoneNumberVerified, model.PhoneNumberVerified.ToString(), ClaimValueTypes.Boolean),
                        new Claim("AMKA", model.AMKA),
                        new Claim("AFM", model.AFM),
                        new Claim("AT", model.AT),
                        new Claim("Photo", "Images" + "@\fileName")
                    }
                };
                bool result = _signupService.Add(inMemoryUser);
                if (result)
                {
                    var name = inMemoryUser.Claims.Where(x => x.Type == JwtClaimTypes.Name).Select(x => x.Value).FirstOrDefault() ?? inMemoryUser.Username;
                    var phoneNumber = inMemoryUser.Claims.Where(x => x.Type == JwtClaimTypes.PhoneNumber).Select(x => x.Value).FirstOrDefault();
                    var claims = new Claim[] {
                        new Claim(JwtClaimTypes.Subject, inMemoryUser.Subject),
                        new Claim(JwtClaimTypes.PhoneNumber, phoneNumber),
                        new Claim(JwtClaimTypes.Name, name),
                        new Claim(JwtClaimTypes.IdentityProvider, "idsvr"),
                        new Claim(JwtClaimTypes.AuthenticationTime, DateTime.UtcNow.ToEpochTime().ToString()),
                    };
                    var ci = new ClaimsIdentity(claims, "password", JwtClaimTypes.Name, JwtClaimTypes.Role);
                    var cp = new ClaimsPrincipal(ci);

                    await HttpContext.Authentication.SignInAsync(Constants.PrimaryAuthenticationType, cp);
                    return RedirectToAction("SendCode");
                }
                else
                {
                    ModelState.AddModelError("", "Μη έγκυρο όνομα χρήστη ή κωδικός.");
                }
            }

            // If we got this far, something failed, redisplay form
            return View();
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
            var factorOptions = new List<SelectListItem> { new SelectListItem() {  Text="Επαλήθευση αριθμού κινητού", Value= "Phone" } };
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
        public IActionResult VerifyCode(string recievedCode, string provider, bool rememberMe, string returnUrl = null)
        {
            // Require that the user has already logged in via username/password or external login
            //var user = await _signInManager.GetTwoFactorAuthenticationUserAsync();
            if (User == null)
            {
                return View("Error");
            }
            VerifyCodeViewModel vc = new VerifyCodeViewModel()
            {
                RecievedCode = recievedCode,
                Provider = provider,
                ReturnUrl = returnUrl,
                RememberMe = rememberMe
            };
            return View(vc);
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

            // The following code protects for brute force attacks against the two factor codes.
            // If a user enters incorrect codes for a specified amount of time then the user account
            // will be locked out for a specified amount of time.
            Result result =  _loginService.TwoFactorSignInAsync(model.Provider, model.RecievedCode, model.Code, model.RememberMe, model.RememberBrowser);
            if (result.Succeeded)
            {
                return RedirectToLocal(model.ReturnUrl);
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