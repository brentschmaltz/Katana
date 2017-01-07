using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens;
using System.Security.Claims;
using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(KatanaOAuthBearer.Startup))]
namespace KatanaOAuthBearer
{
    public partial class Startup
    {
        public static string Base64SecurityKey { get; private set; }
        public static string JWT { get; private set; }

        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);

            // For more information on how to configure your application, visit http://go.microsoft.com/fwlink/?LinkID=316888
            GenerateJwtSecurityToken();
        }

        static void GenerateJwtSecurityToken()
        {
            var audience = "microsoft.com";
            var issuer = "contoso.com";
            Base64SecurityKey = "VbbbbmlbGJw8XH+ZoYBnUHmHga8/o/IduvU/Tht70iE=";

            var tokenHandler = new JwtSecurityTokenHandler();
            // ten days
            tokenHandler.TokenLifetimeInMinutes = 14400;
            var symmetricSecurityKey = new InMemorySymmetricSecurityKey(Convert.FromBase64String(Base64SecurityKey));
            var subject = new ClaimsIdentity(
                new List<Claim>
                {
                    new Claim(JwtRegisteredClaimNames.Aud, audience),
                    new Claim(JwtRegisteredClaimNames.Email, "bob@contoso.com"),
                    new Claim(JwtRegisteredClaimNames.GivenName, "bob"),
                    new Claim(JwtRegisteredClaimNames.Sub, "123456789")
                });

            var tokenDescriptor = new SecurityTokenDescriptor()
            {
                TokenIssuerName = issuer,
                SigningCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256Signature, SecurityAlgorithms.RsaSha256Signature),
                Subject = subject
            };

            var jwtSecurityToken = tokenHandler.CreateToken(tokenDescriptor);
            JWT = tokenHandler.WriteToken(jwtSecurityToken);
        }
    }
}
