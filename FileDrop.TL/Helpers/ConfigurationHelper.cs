using System;
using System.Collections.Specialized;
using System.Configuration;
using System.Diagnostics;

namespace FileDrop.TL.Helpers
{
   public static class ConfigurationHelper
   {
      public static JWTokenDTO GetJWTokenSettings()
      {
         try
         {
            NameValueCollection jWTokensettings = (NameValueCollection)ConfigurationManager.GetSection("jwtSettings");

            if (jWTokensettings == null)
            {
               jWTokensettings = new NameValueCollection(0);
            }

            JWTokenDTO jwtokenDTO = new JWTokenDTO();
            int expirationMinutes = 5;
            foreach (string key in jWTokensettings)
            {
               switch (key)
               {
                  case "ExpirationDays":
                     if (!int.TryParse(jWTokensettings[key], out expirationMinutes))
                     {
                        throw new ArgumentException("Invalid key value.");
                     }
                     break;
                  case "SigningKey":
                     jwtokenDTO.SigningKey = jWTokensettings[key].ToString();
                     break;
                  case "EncryptyingSecurityKey":
                     jwtokenDTO.EncryptyingSecurityKey = jWTokensettings[key].ToString();
                     break;
                  case "Issuer":
                     jwtokenDTO.Issuer = jWTokensettings[key].ToString();
                     break;
                  case "Audience":
                     jwtokenDTO.Audience = jWTokensettings[key].ToString();
                     break;
               }
            }
            jwtokenDTO.ExpirationMinutes = expirationMinutes;
            return jwtokenDTO;
         }
         catch (Exception ex)
         {
            Trace.TraceError("Could not read config file: {0}", ex);
            return null;
         }
      }

      public static dynamic GetKey(string key, string section)
      {
         try
         {
            NameValueCollection configSection = (NameValueCollection)ConfigurationManager.GetSection(section);

            if (configSection == null)
            {
               configSection = new NameValueCollection(0);
            }

            return configSection[key];
         }
         catch
         {
            return null;
         }
      }
   }
}
