using System;
using System.IdentityModel.Tokens;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.Owin.Security.ActiveDirectory;
using Microsoft.Owin.Security.OAuth;
using Owin;

namespace KatanaOAuthBearer
{
    class MyJwtSecurityTokenHandler : JwtSecurityTokenHandler
    {
        public override SecurityToken ReadToken(string tokenString)
        {
            return base.ReadToken(tokenString);
        }

        public override ClaimsPrincipal ValidateToken(string securityToken, TokenValidationParameters validationParameters, out SecurityToken validatedToken)
        {
            return base.ValidateToken(securityToken, validationParameters, out validatedToken);
        }
    }

    class MyOAuthProvider : IOAuthBearerAuthenticationProvider
    {
        public Task ApplyChallenge(OAuthChallengeContext context)
        {
            return new Task<object>(null);
        }

        public Task RequestToken(OAuthRequestTokenContext context)
        {
            return new Task<object>(null);
        }

        public Task ValidateIdentity(OAuthValidateIdentityContext context)
        {
            throw new NotImplementedException();
        }
    }

    public partial class Startup
    {
        // For more information on configuring authentication, please visit http://go.microsoft.com/fwlink/?LinkId=301864
        public void ConfigureAuth(IAppBuilder app)
        {
            // can create token using
            // https://github.com/brentschmaltz/CodeSnips/blob/master/src/JwtTokens/CreateToken.cs#L18

            var symmetricKeyString = "VbbbbmlbGJw8XH+ZoYBnUHmHga8/o/IduvU/Tht70iE=";
            app.UseWindowsAzureActiveDirectoryBearerAuthentication(
                new WindowsAzureActiveDirectoryBearerAuthenticationOptions
                {
                    //Provider = new MyOAuthProvider(),
                    Tenant = "login.microsoftonline.com",
                    TokenValidationParameters = new TokenValidationParameters
                    {
                        AudienceValidator = (audiences, securityToken, validationParameters) =>
                        {
                            return true;
                        },
                        IssuerValidator = (issuer, securityToken, validationParameters) =>
                        {
                            return issuer;
                        },
                        IssuerSigningKey = new InMemorySymmetricSecurityKey(Convert.FromBase64String(symmetricKeyString)),
                        SaveSigninToken = true,

                    },
                    TokenHandler = new MyJwtSecurityTokenHandler()
                });
        }
    }
}