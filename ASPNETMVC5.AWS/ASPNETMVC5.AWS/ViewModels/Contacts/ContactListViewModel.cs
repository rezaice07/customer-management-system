using ASPNETMVC5.Core.Filters;
using ASPNETMVC5.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ASPNETMVC5.AWS.ViewModels.Contacts
{
    public class ContactListViewModel
    {
        public IEnumerable<Contact> Contacts { get; set; }

        public ContactSearchFilter SearchFilter { get; set; }

    }
}