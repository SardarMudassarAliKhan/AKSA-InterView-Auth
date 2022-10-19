using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JWTToken_Auth_DAL.Model
{
    public class RoleRequest
    {
        public string userid { get; set; }
        public string requestedrole { get; set; }
        public string ShopName { get; set; }
        public string username { get; set; }
    }
}
