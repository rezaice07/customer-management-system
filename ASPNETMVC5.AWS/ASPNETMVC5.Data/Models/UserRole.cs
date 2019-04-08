using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASPNETMVC5.Data.Models
{
    public class UserRole
    {
        [Key]
        public int Id { get; set; }
       
        public string RoleName { get; set; }
       
        public DateTime CreatedDate { get; set; }
    }
}
