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

namespace IT4GOV.WebAPI
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            // Set up configuration sources.
            var builder = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json");


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

            
                // This will push telemetry data through Application Insights pipeline faster, allowing you to view results immediately.
                builder.AddApplicationInsightsSettings(developerMode: true);
            

            builder.AddEnvironmentVariables();
            Configuration = builder.Build().ReloadOnChanged("appsettings.json");
        }

        public IConfigurationRoot Configuration { get; set; }

        // This method gets called by the runtime. Use this method to add services to the container
        public void ConfigureServices(IServiceCollection services)
        {
            // Add framework services.
            services.AddApplicationInsightsTelemetry(Configuration);

            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            app.UseIISPlatformHandler();

            app.UseApplicationInsightsRequestTelemetry();

            app.UseApplicationInsightsExceptionTelemetry();

            app.UseCors(policy =>
            {
                policy.WithOrigins("http://localhost:28895", "http://localhost:7017");
                policy.AllowAnyHeader();
                policy.AllowAnyMethod();
            });

            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();
            app.UseIdentityServerAuthentication(options =>
            {
                //Url of the IT4GOV.IdentityServer
                options.Authority = URLS.Idt_url;
                options.ScopeName = "api1";
                options.ScopeSecret = "secret";

                options.AutomaticAuthenticate = true;
                options.AutomaticChallenge = true;
            });

            app.UseMvc();
        }


       

        // Entry point for the application.
        public static void Main(string[] args) => WebApplication.Run<Startup>(args);
    }
}
