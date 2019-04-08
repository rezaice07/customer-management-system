using ASPNETMVC5.Core.Filters;
using ASPNETMVC5.Data.Models;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASPNETMVC5.Service.Services.Users
{
    public interface IUserService
    {
        IEnumerable<User> GetListByFilter(UserSearchFilter filter);
        User GetDetailsById(int id);
        User GetDetailsByUsername(string username);
        bool ResetPassword(User user);
        bool Add(User user);
        bool Update(User user);
        bool Remove(User contact);
    }
}
