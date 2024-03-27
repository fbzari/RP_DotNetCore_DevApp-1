using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Novell.Directory.Ldap;
using Syncfusion.EJ2.ImageEditor;

//namespace RP_DotNetCore_DevApp.AppCode
//{
//    public class LdapAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
//    {
//        private readonly LdapOptions _ldapOptions;

//        public LdapAuthenticationHandler(
//            IOptionsMonitor<AuthenticationSchemeOptions> options,
//            ILoggerFactory logger,
//            UrlEncoder encoder,
//            ISystemClock clock,
//            IOptions<LdapOptions> ldapOptions)
//            : base(options, logger, encoder, clock)
//        {
//            _ldapOptions = ldapOptions.Value;
//        }
//    }

//    protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
//    {
//        try
//        {
//            using (var ldapConnection = new LdapConnection())
//            {
//                ldapConnection.Connect(_ldapOptions.Server.Host, _ldapOptions.Server.Port);
//                ldapConnection.Bind(_ldapOptions.BindDn, _ldapOptions.BindCredentials);

//                // Use the authenticated user information to build claims
//                var claims = new[]
//                {
//                    new Claim(ClaimTypes.Name, "authenticated-username"),
//                    // Add other claims as needed
//                };

//                var identity = new ClaimsIdentity(claims, Scheme.Name);
//                var principal = new ClaimsPrincipal(identity);
//                var ticket = new AuthenticationTicket(principal, Scheme.Name);

//                return AuthenticateResult.Success(ticket);
//            }
//        }
//        catch (LdapException ex)
//        {
//            Logger.LogError(ex, "LDAP authentication failed.");
//            return AuthenticateResult.Fail("LDAP authentication failed.");
//        }
//    }
//}
