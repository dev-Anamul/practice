using Infrastructure.Contracts;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using System.Web;
using Utilities.Constants;
using Utilities.Encryptions;

namespace Api.AuthenticationHandler
{
    public class BasicAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
    {
        private readonly IUnitOfWork context;
        public BasicAuthenticationHandler(IOptionsMonitor<AuthenticationSchemeOptions> options,
            ILoggerFactory logger,
            UrlEncoder encoder,
            ISystemClock clock, IUnitOfWork context)
            : base(options, logger, encoder, clock)
        {
            this.context = context;
        }

        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            try
            {
                if (Request.Headers.ContainsKey("Authorization"))
                {
                    string authorizationHeader = Request.Headers["Authorization"];
                    string token = "";

                    token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");

                    var config = new ConfigurationBuilder().AddJsonFile(GeneralConstants.JsonFileName).Build();
                    var privateKey = config.GetValue<string>("PrivateKey");

                    token = HttpUtility.UrlDecode(token);

                    EncryptionHelpers encryptionHelpers = new EncryptionHelpers();
                    string decryptkey = encryptionHelpers.Decrypt(privateKey);

                    if (token == decryptkey)
                    {
                        var claims = new[] { new Claim(ClaimTypes.Name, "") };
                        var identity = new ClaimsIdentity(claims, Scheme.Name);
                        var principal = new ClaimsPrincipal(identity);
                        var ticket = new AuthenticationTicket(principal, Scheme.Name);

                        return AuthenticateResult.Success(ticket);
                    }

                    Response.Headers.Add("AuthenticationFailureReason", "Invalid Token");

                    return AuthenticateResult.Fail("Invalid Token");
                }
                else if (Request.Headers.ContainsKey("Header1") && Request.Headers.ContainsKey("Header2") && Request.Headers.ContainsKey("Header3") && Request.Headers.ContainsKey("Header4") && Request.Headers.ContainsKey("Header5"))
                {
                    var config = new ConfigurationBuilder().AddJsonFile(GeneralConstants.JsonFileName).Build();
                    string token = "";

                    token = Request.Headers["Header3"].ToString();
                    token += Request.Headers["Header5"].ToString();
                    token += Request.Headers["Header2"].ToString();

                    var privateKey = config.GetValue<string>("PrivateKey");

                    token = HttpUtility.UrlDecode(token);

                    EncryptionHelpers encryptionHelpers = new EncryptionHelpers();
                    string decryptkey = encryptionHelpers.Decrypt(privateKey);

                    if (token == decryptkey)
                    {
                        var claims = new[] { new Claim(ClaimTypes.Name, "") };
                        var identity = new ClaimsIdentity(claims, Scheme.Name);
                        var principal = new ClaimsPrincipal(identity);
                        var ticket = new AuthenticationTicket(principal, Scheme.Name);

                        return AuthenticateResult.Success(ticket);
                    }

                    Response.Headers.Add("AuthenticationFailureReason", "Invalid Token");

                    return AuthenticateResult.Fail("Invalid Token");
                }

                Response.Headers.Add("AuthenticationFailureReason", "Authorization header is missing");

                return AuthenticateResult.Fail("Authorization header is missing");
            }
            catch (Exception ex)
            {
                Response.Headers.Add("AuthenticationFailureReason", MessageConstants.SomethingWentWrong);

                return AuthenticateResult.Fail(MessageConstants.SomethingWentWrong);
            }
        }
    }
}