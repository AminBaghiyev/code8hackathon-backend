using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Management.BL.Utilities;

public class JWTService
{
    readonly IConfiguration _configuration;

    public JWTService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public string GenerateToken(TimeSpan expiry, IEnumerable<Claim>? claims = null)
    {
        SymmetricSecurityKey securityKey = new(Encoding.UTF8.GetBytes(GetSecretKey()));
        SigningCredentials credentials = new(securityKey, SecurityAlgorithms.HmacSha256);

        JwtSecurityToken token = new(
            issuer: GetIssuer(),
            audience: GetAudience(),
            claims: claims,
            expires: DateTime.UtcNow.Add(expiry),
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    public string GetAudience() => _configuration["JWT:Audience"];

    public string GetIssuer() => _configuration["JWT:Issuer"];

    public string GetSecretKey() => _configuration["JWT:SecretKey"];

    public bool ValidateRefreshToken(string? refreshToken)
    {
        if (refreshToken == null) return false;

        TokenValidationParameters validationParameters = new()
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = GetIssuer(),
            ValidAudience = GetAudience(),
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(GetSecretKey())),
            ClockSkew = TimeSpan.Zero
        };

        try
        {
            new JwtSecurityTokenHandler().ValidateToken(refreshToken, validationParameters, out _);
            return true;
        }
        catch
        {
            return false;
        }
    }
}
