namespace QuizBuilder.Business;

using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Interfaces;
using Microsoft.IdentityModel.Tokens;

public class TokenManager : ITokenManager
{
    private readonly string _audience;
    private readonly int _expiration;
    private readonly string _issuer;
    private readonly string _secret;

    public TokenManager(IConfiguration settings)
    {
        var tokenSettings = settings.GetSection("TokenSettings");
        _secret = tokenSettings["Secret"];
        _issuer = tokenSettings["Issuer"];
        _audience = tokenSettings["Audience"];
        _expiration = int.Parse(tokenSettings["Expiration"]);
    }

    public string CreateToken(string userId)
    {
        var key = Encoding.ASCII.GetBytes(_secret);
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[] { new Claim(ClaimTypes.NameIdentifier, userId) }),
            IssuedAt = DateTime.UtcNow,
            NotBefore = DateTime.UtcNow,
            Issuer = _issuer,
            Audience = _audience,
            Expires = DateTime.UtcNow.AddMinutes(_expiration),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }
}