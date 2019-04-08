using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASPNETMVC5.Data.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }
        
        public int RoleId { get; set; }
        
        public int ContactId { get; set; }
        
        public string Username { get; set; }
        
        public string PasswordHash { get; set; }
        
        public string PasswordSalt { get; set; }
        
        public int Status { get; set; }
        
        public DateTime CreatedDateUtc { get; set; }

        [ForeignKey("ContactId")]
        public Contact Contact { get; set; }

        [ForeignKey("RoleId")]
        public UserRole UserRole { get; set; }
    }
}
