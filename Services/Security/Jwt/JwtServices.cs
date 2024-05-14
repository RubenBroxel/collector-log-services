using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;

public class JwtServices: IJwtSecurity
{
    private readonly string Secret = "secret_generate_automatic";

    //Método para generar un JWT
    public string GenerateToken(string userName, string secret)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Convert.FromBase64String(Secret);
         var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[] { new Claim(ClaimTypes.Name, userName) }),
            Expires = DateTime.UtcNow.AddHours(1),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };
        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }

    public bool IsValidUser(UserCredentials credentials)
    {
        // Lógica para validar las credenciales del usuario
        // Aquí puedes realizar la autenticación contra una base de datos u otro método
        return credentials.Username == "usuario" && credentials.Password == "contrasena";
    }


}