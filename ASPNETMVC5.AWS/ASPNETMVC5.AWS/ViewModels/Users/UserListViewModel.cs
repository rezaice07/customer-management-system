using ASPNETMVC5.Core.Filters;
using ASPNETMVC5.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ASPNETMVC5.AWS.ViewModels.Users
{
    public class UserListViewModel
    {
        public IEnumerable<User> Users { get; set; }

        public UserSearchFilter SearchFilter { get; set; }
    }
}