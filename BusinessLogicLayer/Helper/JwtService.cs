using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using DataLogicLayer.Interfaces;
using DataLogicLayer.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

public class JwtService
{
    private readonly string? _key;
    private readonly string? _issuer;
    private readonly string? _audience;
    private readonly IConfiguration configuration;
    private readonly IRolePermissionsRepository _rolePermissionsRepository;


    public JwtService(IConfiguration configuration, IRolePermissionsRepository rolePermissionsRepository)
    {
        _key = configuration["JwtConfig:Key"];
        _issuer = configuration["JwtConfig:Issuer"];
        _audience = configuration["JwtConfig:Audience"];
        this.configuration = configuration;
        _rolePermissionsRepository = rolePermissionsRepository;

    }

    /*-----------------------------------------------------------------------------------------------------------------------Generate Token
    -----------------------------------------------------------------------------------------------------------------------------------------*/

    #region Generate Token
    public async Task<string> GenerateJwtToken(string email, string role, string userName, long roleId)
    {
        JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
        byte[] key = Encoding.UTF8.GetBytes(_key); // Secret Code (Salt)

        List<Rolesandpermission> permissions = await _rolePermissionsRepository.GetRolesandpermissionsList(roleId);

        var claims = new List<Claim>
        {
            new Claim("email", email),
            new Claim("role", role),
            new Claim("userName", userName)
        };

        // Add permissions as claims
        foreach (var permission in permissions)
        {
            if (permission.Canview) claims.Add(new Claim("permission", $"{permission.Permission.Name}_View"));
            if (permission.Canaddedit) claims.Add(new Claim("permission", $"{permission.Permission.Name}_AddEdit"));
            if (permission.Candelete) claims.Add(new Claim("permission", $"{permission.Permission.Name}_Delete"));
        }

        SecurityTokenDescriptor tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddHours(5),
            Issuer = _issuer,
            Audience = _audience,
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256)
        };

        SecurityToken token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }

    #endregion

    /*-------------------------------------------------------------------------------------------------------------------Validate Token Methodd
    -----------------------------------------------------------------------------------------------------------------------------------------*/

    #region  Validate Token
    public bool ValidateToken(string token)
    {
        if (string.IsNullOrEmpty(token))
            return false;

        JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
        byte[] key = Convert.FromBase64String(_key); // Convert key to byte array

        try
        {
            tokenHandler.ValidateToken(token, new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = false, 
                ValidateAudience = false, 
                ValidateLifetime = true, 
                ClockSkew = TimeSpan.Zero 
            }, out _);

            return true; // Token is valid
        }
        catch
        {
            return false; //Token is invalid
        }
    }

    #endregion


    /*-------------------------------------------------------------------------------------------------------------Get Claim Value from Token
    -----------------------------------------------------------------------------------------------------------------------------------------*/

    #region Get Claim Value
    public ClaimsPrincipal? GetClaimsFromToken(string token)
    {
        JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();
        JwtSecurityToken jwtToken = handler.ReadJwtToken(token);
        ClaimsIdentity claims = new ClaimsIdentity(jwtToken.Claims);
        return new ClaimsPrincipal(claims);
    }

    // Retrieves a specific claim value from a JWT token.
    public string? GetClaimValue(string token, string claimType)
    {
        ClaimsPrincipal? claimsPrincipal = GetClaimsFromToken(token);
        // return claimsPrincipal?.FindFirst(claimType)?.Value;
        string? value = claimsPrincipal?.FindFirst(claimType)?.Value;
        return value;
    }
    #endregion
}