using ASPNETMVC5.Data.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ASPNETMVC5.AWS.ViewModels.Users
{
    public class UserAddEditViewModel
    {
        [Display(Name = "Id")]
        [Required(ErrorMessage = "{0} is Required")]
        public int Id { get; set; }

        [Display(Name = "Role Id")]
        [Required(ErrorMessage = "{0} is Required")]
        public int RoleId { get; set; }

        [Display(Name = "Contact Id")]
        [Required(ErrorMessage = "{0} is Required")]
        public int ContactId { get; set; }

        [Display(Name = "Username")]
        [Required(ErrorMessage = "{0} is Required")]
        [StringLength(50, ErrorMessage = "Maximum length is {1}")]
        public string Username { get; set; }

        [Display(Name = "Password Hash")]
        [Required(ErrorMessage = "{0} is Required")]
        [StringLength(150, ErrorMessage = "Maximum length is {1}")]
        public string PasswordHash { get; set; }

        [Display(Name = "Password Solt")]
        [Required(ErrorMessage = "{0} is Required")]
        [StringLength(150, ErrorMessage = "Maximum length is {1}")]
        public string PasswordSolt { get; set; }

        [Display(Name = "Status")]
        [Required(ErrorMessage = "{0} is Required")]
        public int Status { get; set; }
    }
    public class AccountResetPasswordViewModel
    {
        [Display(Name = "Password")]
        [Required(ErrorMessage = "{0} is Required")]
        [StringLength(50, ErrorMessage = "Maximum length is {1}")]
        public string Password { get; set; }

        [Display(Name = "Confirm Password")]
        [Required(ErrorMessage = "{0} is Required")]
        [StringLength(50, ErrorMessage = "Maximum length is {1}")]
        public string ConfirmPassword { get; set; }

        public string FullName { get; set; }
        public int UserId  { get; set; }
        public int RoleId { get; set; }
    }

}