using System.Security.Claims;
public interface IJwtSecurity
{
    string GenerateToken(string userName, string secret);
    bool IsValidUser(UserCredentials credentials);
}