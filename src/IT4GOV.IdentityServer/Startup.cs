using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Builder;
using Microsoft.AspNet.Hosting;
using Microsoft.AspNet.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.PlatformAbstractions;
using System.Security.Cryptography.X509Certificates;
using System.IO;
using IT4GOV.IdentityServer.Configuration;
using IT4GOV.IdentityServer.Extensions;
using IT4GOV.IdentityServer.UI;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using IdentityServer4.Core.Configuration;

namespace IT4GOV.IdentityServer
{
    public class Startup
    {

        private readonly IApplicationEnvironment _environment;

        public Startup(IApplicationEnvironment environment, IHostingEnvironment env)
        {
            _environment = environment;
            var builder = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true);




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

            
                // For more details on using the user secret store see http://go.microsoft.com/fwlink/?LinkID=532709
                builder.AddUserSecrets();

                // This will push telemetry data through Application Insights pipeline faster, allowing you to view results immediately.
                builder.AddApplicationInsightsSettings(developerMode: true);
            

            builder.AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; set; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit http://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            var cert = new X509Certificate2(Path.Combine(_environment.ApplicationBasePath, "idsrv4test.pfx"), "idsrv3test");
            var builder = services.AddIdentityServer(options =>
        {
                options.SigningCertificate = cert;
            });
            
            builder.AddInMemoryClients(Clients.Get());
            builder.AddInMemoryScopes(Scopes.Get());
            builder.AddInMemoryUsers(Users.Get());

            builder.AddCustomGrantValidator<CustomGrantValidator>();


            // for the UI
            services
                .AddMvc()
                .AddRazorOptions(razor =>
                {
                    razor.ViewLocationExpanders.Add(new CustomViewLocationExpander());
                });
            services.AddTransient<UI.Login.LoginService>();
            services.AddTransient<UI.SignUp.SignUpService>();
            services.AddTransient<ISmsSender, MessageServices>();
            services.Configure<ASPmsSercetCredentials>(Configuration);



        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(LogLevel.Verbose);
            loggerFactory.AddDebug(LogLevel.Verbose);

            app.UseDeveloperExceptionPage();
            app.UseIISPlatformHandler();

            app.UseIdentityServer();

            app.UseStaticFiles();
            app.UseMvcWithDefaultRoute();

            
        }

        // Entry point for the application.
        public static void Main(string[] args) => WebApplication.Run<Startup>(args);
    }
}
