using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;

namespace _03_BasicAuthentication
{
    public class BasicAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
    {
        public BasicAuthenticationHandler(IOptionsMonitor<AuthenticationSchemeOptions> options, ILoggerFactory logger, UrlEncoder encoder, ISystemClock clock) : base(options, logger, encoder, clock)
        {
        }

        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            var endPoint = Context.GetEndpoint();

            if (endPoint?.Metadata?.GetMetadata<IAllowAnonymous>() != null)  //kontrol yapmadan işlemi çalıştırır AllowAnonymous olması lazım!
            {
                return AuthenticateResult.NoResult();
            }

            if (Request.Headers.ContainsKey("Authorization") == false)  //kullanıcı kontrol edilir! Postman Header!!
            {
                return AuthenticateResult.Fail("Kullanıcı bilgilerine ulaşılamadı!");
            }

            var authHeader = AuthenticationHeaderValue.Parse(Request.Headers["Authorization"]); //içerisinde  postman Basic Auth içerisindeki şifreli bilgiler(Headers kısmındaki) mevcut
            var cridentialBytes = Convert.FromBase64String(authHeader.Parameter);  //şifreli bilgiyi stringe çeviriyor
            var cridential = Encoding.UTF8.GetString(cridentialBytes).Split(':', 2);

            var username = cridential[0];
            var password = cridential[1];

            bool result = (username == "isilay" && password == "123") ? true : false;
            if (result)
            {
                List<Claim> claims = new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier, "99"),
                    new Claim(ClaimTypes.Name, username),
                    new Claim(ClaimTypes.Role, "admin")
                };

                var identity = new ClaimsIdentity(claims, Scheme.Name);
                var principle = new ClaimsPrincipal(identity);
                var ticket = new AuthenticationTicket(principle, Scheme.Name);

                return AuthenticateResult.Success(ticket);
            }
            else
            {
                return AuthenticateResult.Fail("Kullanıcı bilgileri hatalı!");
            }
        }
    }
}
