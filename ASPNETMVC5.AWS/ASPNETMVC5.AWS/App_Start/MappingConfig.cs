using ASPNETMVC5.AWS.ViewModels.Contacts;
using ASPNETMVC5.AWS.ViewModels.Users;
using ASPNETMVC5.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ASPNETMVC5.AWS.App_Start
{
    public static class MappingConfig
    {
        public static void RegisterMaps()
        {
            AutoMapper.Mapper.Initialize(config =>
            {
                config.CreateMap<UserAddEditViewModel, User>();
                config.CreateMap<User, UserAddEditViewModel>();

                config.CreateMap<AccountResetPasswordViewModel, User>();
                config.CreateMap<User, AccountResetPasswordViewModel>();

                config.CreateMap<AddEditCustomerViewModel, Contact>();
                config.CreateMap<Contact, AddEditCustomerViewModel>();
            });
        }
    }
}