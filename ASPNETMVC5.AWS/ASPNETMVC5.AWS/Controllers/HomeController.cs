using ASPNETMVC5.AWS.Infrastructure;
using ASPNETMVC5.AWS.ViewModels.Accounts;
using ASPNETMVC5.AWS.ViewModels.Contacts;
using ASPNETMVC5.Data.Models;
using ASPNETMVC5.Service.Services.Contacts;
using ASPNETMVC5.Service.Services.Encryptions;
using ASPNETMVC5.Service.Services.Users;
using AutoMapper;
using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using static ASPNETMVC5.Core.AppConstants;

namespace ASPNETMVC5.AWS.Controllers
{
    public class HomeController : Controller
    {
        #region Private Member

        private readonly IUserService _userService;
        private readonly IContactService _contactService;

        #endregion

        #region Ctor

        public HomeController(IUserService userService,
            IContactService contactService)
        {
            _userService = userService;
            _contactService = contactService;
        }

        #endregion

        #region Home page

        public ActionResult Index()
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Dashboard");
            }

            return View();
        }

        #endregion

        #region Login

        public ActionResult LogIn()
        {
            if (User.Identity.IsAuthenticated)
            {
                Session[User.Identity.Name] = null;
                return RedirectToAction("Logout");
            }

            var model = new UserLogInModel
            {
                Username = "admin",
                Password = "Google123"
            };
            return View(model);
        }

        [HttpPost]
        public ActionResult LogIn(UserLogInModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = _userService.GetDetailsByUsername(model.Username);

            if (user == null)
            {
                ViewBag.Message = $"<span class= 'text-danger'> Incorrect username or password. </span>";
                return View(model);
            }

            if (user.Status != StatusConstants.Active && user.PasswordHash != EncryptionService.HashPassword(model.Password, user.PasswordSalt))
            {
                ViewBag.Message = "Incorrect Username or Password, Please try again.";

                return View(model);
            }

            Session[user.Username] = user;
            SetCookie(user, false);

            return RedirectToAction("Dashboard", "Home");
        }

        #endregion

        #region Authorized Dashboard

        [ActionAuthorize]
        public ActionResult Dashboard()
        {
            if (User.Identity.IsAuthenticated)
            {
                var currentUser = (User)Session[User.Identity.Name];

                if (currentUser == null)
                    return RedirectToAction("Logout");

                if (currentUser.RoleId == UserRoleConstants.Customer)
                {
                    return Redirect("/smartcrm/customer/dashboard");
                }

                return Redirect("/smartcrm/admin/dashboard");
            }

            return RedirectToAction("Logout");
        }

        [ActionAuthorize]

        [Route("smartcrm/customer/dashboard")]
        public ActionResult CustomerDashboard()
        {
            return View();
        }

        [ActionAuthorize]
        [Route("smartcrm/admin/dashboard")]
        public ActionResult AdminDashboard()
        {

            return View();
        }

        #endregion

        #region Logout

        public ActionResult Logout()
        {
            Session[User.Identity.Name] = null;

            FormsAuthentication.SignOut();

            Session.Clear();
            Session.Abandon();
            return RedirectToAction("Login", "Home");
        }

        #endregion

        #region New Customer

        public ActionResult NewCustomer()
        {
            var model = new AddEditCustomerViewModel
            {

            };

            return View(model);
        }

        [HttpPost]
        public ActionResult NewCustomer(AddEditCustomerViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var newContact = Mapper.Map<Contact>(model);
            newContact.CreatedDate = DateTime.UtcNow;
            newContact.Status = StatusConstants.Active;

            var isAdded = _contactService.Add(newContact);

            if (isAdded)
            {
                return RedirectToAction("login");
            }

            return View(model);
        }

        #endregion

        #region My Profile

        [Route("smartcrm/myprofile")]
        public ActionResult MyProfile()
        {
            if (!User.Identity.IsAuthenticated)
                return RedirectToAction("Logout");

            var sessionCurrentUser = Session[User.Identity.Name];

            if (sessionCurrentUser == null)
                return RedirectToAction("Logout");

            var currentUser = (User)Session[User.Identity.Name];

            var user = _userService.GetDetailsById(currentUser.Id);

            if (user == null)
            {
                return HttpNotFound();
            }            

            return View(user);
        }

        #endregion

        #region Private Member

        private bool SetCookie(User user, bool rememberMe = false)
        {
            try
            {
                string role = "";
                string name = "";

                role = user.UserRole.RoleName;
                name = user.Username.Trim();

                var authTicket = new FormsAuthenticationTicket(1, name, DateTime.Now, DateTime.Now.AddDays(5), rememberMe, role, FormsAuthentication.FormsCookiePath);

                string cookieConents = FormsAuthentication.Encrypt(authTicket);

                var cookie = new HttpCookie(FormsAuthentication.FormsCookieName, cookieConents);
                Response.Cookies.Add(cookie);

                return true;
            }
            catch
            {
                return false;
            }
        }

        #endregion
    }
}