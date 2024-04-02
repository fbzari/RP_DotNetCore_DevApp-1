using Microsoft.AspNetCore.Builder;
// Add the necessary using statements
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Novell.Directory.Ldap;
using System.Security.Claims;

namespace RP_DotNetCore_DevApp
{
    public class Startup
    {

        public class CustomAuthorizationRequirement : IAuthorizationRequirement
        {
            // Add any properties or methods as needed
        }

        // Configure the custom authorization policy


        public void Configure(IApplicationBuilder app)
        {
            // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
            app.UseSession();
            app.UseAuthentication();
            app.UseAuthorization();
            // Configure
            
        }

        public void ConfigureServices(IServiceCollection services)
        {
            // Other ConfigureServices code...

            //services.Configure<LdapOptions>(Configuration.GetSection("LdapOptions"));
            //services.AddAuthentication(options =>
            //{
            //    options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            //})
            //    .AddScheme<AuthenticationSchemeOptions, LdapAuthenticationHandler>("Ldap", options => { });

            // Other ConfigureServices code...
            services.AddSession();
        }
    }
}
