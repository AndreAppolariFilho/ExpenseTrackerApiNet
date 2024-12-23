using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;

public class JwtTokenService
{
    private readonly IConfiguration _configuration;

    public JwtTokenService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public string GenerateToken(string username)
    {
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var claims = new ClaimsIdentity(
        [
            new Claim(JwtRegisteredClaimNames.Sub, username),
        ]);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Issuer = _configuration["Jwt:Issuer"],
            Audience = _configuration["Jwt:Audience"],
            Subject = claims,
            Expires = DateTime.Now.AddMinutes(60),
            SigningCredentials = credentials,
        };
        var handler = new JsonWebTokenHandler();
        string token = handler.CreateToken(tokenDescriptor);
        return token;
    }
}
