using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Builder;
using Microsoft.AspNet.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.IdentityModel.Tokens.Jwt;
using IT4GOV.WebApp.Extensions;

namespace IT4GOV.WebApp
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            // Set up configuration sources.
            var builder = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .AddEnvironmentVariables();


#if DEBUG
            URLS.Api_url = "http://localhost:58517/";
            URLS.Html_url = "http://localhost:7079/oauth.html";
            URLS.Idt_url = "http://localhost:58058/";
            URLS.Mvc_url = "http://localhost:7079/signin-oidc";
#endif

#if RELEASE
            

            URLS.Api_url = "http://it4govwebapi.azurewebsites.net/";
            URLS.Html_url = "http://it4govwebapp.azurewebsites.net/oauth.html";
            URLS.Idt_url = "http://it4govidentityserver.azurewebsites.net";
            URLS.Mvc_url = "http://it4govwebapp.azurewebsites.net/signin-oidc";
#endif

            if (env.IsDevelopment())
            {
                // This will push telemetry data through Application Insights pipeline faster, allowing you to view results immediately.
                builder.AddApplicationInsightsSettings(developerMode: true);
            }
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; set; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Add framework services.
            services.AddApplicationInsightsTelemetry(Configuration);

            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            app.UseApplicationInsightsRequestTelemetry();

            if (env.IsDevelopment())
            {
                app.UseBrowserLink();
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseIISPlatformHandler();

            app.UseApplicationInsightsExceptionTelemetry();

            app.UseStaticFiles();

            app.UseCookieAuthentication(options =>
            {
                options.AuthenticationScheme = "cookies";
                options.AutomaticAuthenticate = true;
            });

            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();
            app.UseOpenIdConnectAuthentication(options =>
            {
                options.AuthenticationScheme = "oidc";
                options.SignInScheme = "cookies";
                options.AutomaticChallenge = true;

                //Use IdentityServer
                options.Authority = URLS.Idt_url;
                options.RequireHttpsMetadata = false;

                options.ClientId = "WebApp";
                options.ResponseType = "id_token token";

                options.Scope.Add("profile");
                options.Scope.Add("email");
                //options.Scope.Add("roles");
                //options.Scope.Add("ContactInformation");
                options.Scope.Add("phone");
                options.Scope.Add("AMKAInformation");
                options.Scope.Add("AFMInformation");
                options.Scope.Add("ATInformation");

                //options.Scope.Add("PersonalInformation");

                options.Scope.Add("api1");

                options.TokenValidationParameters.NameClaimType = "name";
                options.TokenValidationParameters.RoleClaimType = "role";
            });

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });

        }

        // Entry point for the application.
        public static void Main(string[] args) => WebApplication.Run<Startup>(args);
    }
}
