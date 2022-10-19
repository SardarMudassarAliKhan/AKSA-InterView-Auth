using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace JWTToken_Auth_DAL.Dto
{
    public class RequestedRole
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key, Column(Order = 0)]
        public int Id { get; set; }
        public string UserID { get; set; }
        public string RequestRole { get; set; }
        public string CompanyName { get; set; }
        public string UserName { get; set; }

    }
}
