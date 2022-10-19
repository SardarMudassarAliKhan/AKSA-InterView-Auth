using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JWTToken_Auth_DAL.Model
{
    public class ChangePassword: IdentityUser
    {
        public string OldPassword { get; set; }
        public string NewPassword { get; set; }
    }
}
