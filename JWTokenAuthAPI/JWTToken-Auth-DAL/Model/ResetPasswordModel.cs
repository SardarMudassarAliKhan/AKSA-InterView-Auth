using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JWTToken_Auth_DAL.Model
{
    public class ResetPasswordModel
    {
        public string userid { get; set; }
        public string token { get; set; }
        public string newpassword { get; set; }
    }
}
