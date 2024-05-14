using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;

public  class SecurityJWT
{
    private  readonly string Secret = "secret_generate_automatic";
    public ClaimsPrincipal GetPrincipal(string token)
    {
        try
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Convert.FromBase64String(Secret);
            var parameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = false,
                ValidateAudience = false
            };
            var principal = tokenHandler.ValidateToken(token, parameters, out _);
            return principal;
        }
        catch(Exception ex)
        {
            Console.WriteLine(ex.Message.ToString());
            return null;
        }
    }
}