using FileDrop.BL.Helpers.Interfaces;
using FileDrop.TL;
using FileDrop.TL.Helpers;
using FileDrop.TL.Models;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace FileDrop.BL.Helpers.Classes
{
   public class JWTokenHelper : IJWTokenHelper
   {
      public ValidateTokenModel ValidateToken(string userToken)
      {
         TokenValidationParameters validationParameters = GetValidationParameters();
         return ValidateToken(userToken, validationParameters);
      }

      private static TokenValidationParameters GetValidationParameters()
      {
         JWTokenDTO jwtSettings = ConfigurationHelper.GetJWTokenSettings();

         return new TokenValidationParameters()
         {
            ValidateIssuer = true,
            ValidIssuer = jwtSettings.Issuer,
            ValidateAudience = true,
            ValidAudience = jwtSettings.Audience,
            RequireExpirationTime = true,
            RequireSignedTokens = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            TokenDecryptionKey = new SymmetricSecurityKey(Encoding.Default.GetBytes(jwtSettings.EncryptyingSecurityKey)), // The same encrypting key as the one that generate the token.
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.SigningKey)) // The same signing key as the one that generate the token.
         };
      }
      private ValidateTokenModel ValidateToken(string authToken, TokenValidationParameters validationParameters)
      {
         try
         {
            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();

            tokenHandler.ValidateToken(authToken, validationParameters, out SecurityToken validatedToken);

            ValidateTokenModel validateTokenModel = new ValidateTokenModel { IsValid = true, Token = validatedToken };

            return validateTokenModel;
         }
         catch (ArgumentException)
         {
            return new ValidateTokenModel { IsValid = false };
         }
         catch (SecurityTokenInvalidSignatureException)
         {
            return new ValidateTokenModel { IsValid = false };
         }
         catch (SecurityTokenInvalidAudienceException)
         {
            return new ValidateTokenModel { IsValid = false };
         }
         catch (SecurityTokenInvalidIssuerException)
         {
            return new ValidateTokenModel { IsValid = false };
         }
         catch (Exception)
         {
            return new ValidateTokenModel { IsValid = false };
         }
      }
   }
}
