using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using SampleCkWebApp.Application.Users.Interfaces;

namespace SampleCkWebApp.Infrastructure.Security;


public sealed class JwtTokenGenerator : IJwtTokenGenerator
{
    
 private readonly IConfiguration _configuration;

 public JwtTokenGenerator(IConfiguration configuration)
    {
        
        _configuration = configuration;

    }

    public string GenerateToken (int userId, string email, string role)
    {
        

        var key = _configuration["Jwt:Key"]!;
        var issuer = _configuration["Jwt:Issuer"]!;
        var audience = _configuration["Jwt:Audience"]!;

        var claims = new List<Claim>
        {
            
        new(ClaimTypes.NameIdentifier, userId.ToString()), 
        new(ClaimTypes.Role, role),
        new(JwtRegisteredClaimNames.Email, email),

        };

        var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
        var creds = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);
        

        var token = new JwtSecurityToken(

            issuer: issuer,
            audience: audience,
            claims: claims,
            expires:DateTime.UtcNow.AddHours(2),
            signingCredentials: creds

        );
        return new JwtSecurityTokenHandler().WriteToken(token);

    }



}