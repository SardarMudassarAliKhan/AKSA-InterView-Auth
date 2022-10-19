using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JWTToken_Auth_BAL.Common
{
    public static class Helper
    {
        public static string ExtractCompany(string EmailAddress)
        {
            try
            {
                if (!string.IsNullOrEmpty(EmailAddress))
                {
                    var result = EmailAddress.Split('@');
                    var _arrCompanyName = result[1].Split('.');
                    if(! string.IsNullOrEmpty(_arrCompanyName[0]))
                    {
                        return _arrCompanyName[0].ToLower();
                    }
                    return "";
                }
                return "";
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}
