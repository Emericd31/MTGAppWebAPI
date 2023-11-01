/* Copyright (C) 2023 Emeric Delacroix - All Rights Reserved
 * You may use, distribute and modify this code under the
 * terms of the MagicAppCopyright license.
 *
 * You should have received a copy of the MagicAppCopyright license with
 * this file. If not, please write to: delacroix.emeric@gmail.com
 */

using System.IdentityModel.Tokens.Jwt;
using System.Text;
using System.Security.Claims;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using MagicAppAPI.Models;
using MagicAppAPI.Context;

namespace Orchestre.API.Tools
{
  public static class TokenGenerator
  {
    private static IConfiguration InitConfiguration()
    {
      var config = new ConfigurationBuilder()
          .AddJsonFile("appsettings.json")
          .Build();
      return config;
    }

    private static IOptions<TokenSettings> GetTokenSettings()
    {
      TokenSettings tokenSettingsOptions = new TokenSettings();
      var config = InitConfiguration();
      config.GetSection("TokenSettings").Bind(tokenSettingsOptions);
      return Options.Create(tokenSettingsOptions);
    }

    public static string GenerateJwtToken(List<Claim> claims)
    {
      IOptions<TokenSettings> tokenSettings = GetTokenSettings();

      var securitykey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenSettings.Value.Key));
      var credentials = new SigningCredentials(securitykey, SecurityAlgorithms.HmacSha256);

      var jwtToken = new JwtSecurityToken(
          issuer: tokenSettings.Value.Issuer,
          audience: tokenSettings.Value.Audience,
          expires: DateTime.Now.ToLocalTime().AddDays(2),
          signingCredentials: credentials,
          claims: claims
      );
      return new JwtSecurityTokenHandler().WriteToken(jwtToken);
    }

    public static User GetUserFromTokenWithEmailClaim(String token, MagicAppContext context)
    {
      try
      {
        var handler = new JwtSecurityTokenHandler();
        var convertedToken = handler.ReadToken(token) as JwtSecurityToken;
        var userEmail = convertedToken?.Claims.First(claim => claim.Type == ClaimTypes.Email).Value;
        var user = context.Users.FirstOrDefault(user => user.Email.ToLower() == userEmail.ToLower());

        return user;
      }
      catch
      {
        return null;
      }
    }
  }
}
