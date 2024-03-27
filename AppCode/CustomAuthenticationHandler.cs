using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using Syncfusion.EJ2.ImageEditor;
using System.Security.Claims;
using System.Text.Encodings.Web;

namespace RP_DotNetCore_DevApp.AppCode
{
    //public class CustomAuthenticationHandler
    //{
    //    public CustomAuthenticationHandler(
    //    IOptionsMonitor<AuthenticationSchemeOptions> options,
    //    ILoggerFactory logger,
    //    UrlEncoder encoder,
    //    ISystemClock clock)
    //    : base(options, logger, encoder, clock)
    //    {
    //    }

    //    protected override Task<AuthenticateResult> HandleAuthenticateAsync()
    //    {
    //        // Check your custom condition here
    //        if (Context.Request.Query["customCondition"] == "1")
    //        {
    //            var claims = new[]
    //            {
    //            new Claim(ClaimTypes.Name, "authenticated-username"),
    //                // Add other claims as needed
    //        };
                    
    //            var identity = new ClaimsIdentity(claims, Scheme.Name);
    //            var principal = new ClaimsPrincipal(identity);
    //            var ticket = new AuthenticationTicket(principal, Scheme.Name);

    //            return Task.FromResult(AuthenticateResult.Success(ticket));
    //        }

    //        return Task.FromResult(AuthenticateResult.Fail("Custom authentication failed.")); 
    //    }
    //}
}
