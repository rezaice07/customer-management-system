using ASPNETMVC5.AWS.Infrastructure;
using ASPNETMVC5.AWS.ViewModels.Users;
using ASPNETMVC5.Core.Filters;
using ASPNETMVC5.Data.Models;
using ASPNETMVC5.Service.Services.Users;
using AutoMapper;
using System.Web.Mvc;
using static ASPNETMVC5.Core.AppConstants;

namespace ASPNETMVC5.AWS.Controllers
{    
    public class UserController : BaseController
    {
        #region Private Members
        private readonly IUserService _userService;

        #endregion

        #region Ctor
        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        #endregion

        #region List

        public ActionResult List(int? pageNumber, string searchTerm, string sort = "Username", string sortdir = "ASC")
        {
            pageNumber = pageNumber ?? 1;

            var filter = new UserSearchFilter
            {
                PageNumber = (int)pageNumber,
                SearchTerm = searchTerm ?? string.Empty,
                SortColumn = sort,
                SortDirection = sortdir
            };

            var userList = _userService.GetListByFilter(filter);

            var model = new UserListViewModel
            {
                Users = userList,
                SearchFilter = filter
            };

            return Request.IsAjaxRequest() ? (ActionResult)PartialView("_UserList", model) : View(model);
        }

        [HttpPost]
        [ActionName("List")]
        [ValidateAntiForgeryToken]
        public ActionResult List(UserSearchFilter filter)
        {
            filter.SearchTerm = filter.SearchTerm ?? string.Empty;
            filter.SortColumn = "Username";
            filter.SortDirection = "ASC";

            var userList = _userService.GetListByFilter(filter);

            var model = new UserListViewModel
            {
                Users = userList,
                SearchFilter = filter
            };

            //QueryStringHelper.Add("pageNumber", filter.PageNumber);
            //QueryStringHelper.Add("searchTerm", filter.SearchTerm.ToString());
            //QueryStringHelper.Add("status", filter.Status.ToString());

            return View(model);
        }

        #endregion

        #region Add
        public ActionResult Create()
        {
            var model = new UserAddEditViewModel();

            return View(model);
        }

        [HttpPost]
        [ActionName("Create")]
        [ValidateAntiForgeryToken]
        public ActionResult Create(UserAddEditViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var newUser = Mapper.Map<User>(model);

            var isAdded = _userService.Add(newUser);

            if (!isAdded)
            {
                return View(model);
            }

            return RedirectToAction("List");
        }

        #endregion

        #region Edit
        public ActionResult Edit(int id)
        {
           
            User user = _userService.GetDetailsById(id);

            if (user == null)
            {
                return HttpNotFound();
            }

            var model= Mapper.Map<UserAddEditViewModel>(user);

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(UserAddEditViewModel model)
        {


            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var updateUser = Mapper.Map<User>(model);

            var isUpdated = _userService.Update(updateUser);

            if (!isUpdated)
            {
                return View(model);
            }

            return RedirectToAction("List");
        }

        #endregion

        #region Delete        
        
        public ActionResult Delete(int id)
        {
            var deleteContact = new User {
                Id = id,
                Status = StatusConstants.Deleted
            };

            var isDeleted = _userService.Remove(deleteContact);

            return RedirectToAction("All");
        }

        #endregion

        #region Reset Password
        public ActionResult ResetPassword()
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

            var model = new AccountResetPasswordViewModel {
                FullName=user.Contact.FullName,
                UserId=user.Id,
                RoleId=user.RoleId
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ResetPassword(AccountResetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            if(model.Password!=model.ConfirmPassword)
                return View(model);

            var currentUser = (User)Session[User.Identity.Name];
            if (currentUser == null)
                return RedirectToAction("Logout");

            var updateUser = Mapper.Map<User>(model);
            updateUser.Id = currentUser.Id;

            var isReset = _userService.ResetPassword(updateUser);

            if (!isReset)
            {
                return View(model);
            }

            return Redirect("/Home/Dashboard");
        }

        #endregion
    }
}