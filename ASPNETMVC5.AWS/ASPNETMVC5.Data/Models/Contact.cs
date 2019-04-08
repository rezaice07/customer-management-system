using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASPNETMVC5.Data.Models
{
    [Table("Contact")]
    public class Contact
    {
        [Key]        
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string CellPhone { get; set; }
        public string Address { get; set; }
        public int Status { get; set; }
        public DateTime CreatedDate { get; set; }

        public virtual ICollection<User> Users { get; set; }

        //additional
        [NotMapped]
        public string Username { get; set; }

        [NotMapped]
        public string FullName => $"{FirstName} {LastName}";
    }
}
