﻿using System.Web;
using System.Web.Mvc;

namespace ASPNETMVC5.AWS
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
