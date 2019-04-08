using ASPNETMVC5.Core.Filters;
using ASPNETMVC5.Data.Models;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASPNETMVC5.Service.Services.Contacts
{
    public interface IContactService
    {
        IPagedList<Contact> GetListByFilter(ContactSearchFilter filter);

        Contact GetDetailsById(int id);
        bool Add(Contact contact);
        bool Update(Contact contact);
        bool Remove(Contact contact);
    }
}
