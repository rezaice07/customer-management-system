using ASPNETMVC5.AWS.Infrastructure;
using ASPNETMVC5.AWS.ViewModels.Contacts;
using ASPNETMVC5.Core.Filters;
using ASPNETMVC5.Data.Models;
using ASPNETMVC5.Service.Services.Contacts;
using AutoMapper;
using System.Web.Mvc;
using static ASPNETMVC5.Core.AppConstants;

namespace ASPNETMVC5.AWS.Controllers
{    
    public class CustomerController : BaseController
    {
        #region Private Members
        private readonly IContactService _contactService;

        #endregion

        #region Ctor
        public CustomerController(IContactService contactService)
        {
            _contactService = contactService;
        }

        #endregion

        #region List

        public ActionResult List(int? pageNumber, string searchTerm, string sort = "FirstName", string sortdir = "ASC")
        {
            pageNumber = pageNumber ?? 1;

            var filter = new ContactSearchFilter
            {
                PageNumber = (int)pageNumber,
                SearchTerm = searchTerm ?? string.Empty,
                SortColumn = sort,
                SortDirection = sortdir,
                UserRoleId=UserRoleConstants.Customer
            };

            var contactList = _contactService.GetListByFilter(filter);

            var model = new ContactListViewModel
            {
                Contacts = contactList,
                SearchFilter = filter
            };

            return Request.IsAjaxRequest() ? (ActionResult)PartialView("_ContactList", model) : View(model);
        }

        [HttpPost]
        [ActionName("List")]
        [ValidateAntiForgeryToken]
        public ActionResult List(ContactSearchFilter filter)
        {
            filter.SearchTerm = filter.SearchTerm ?? string.Empty;
            filter.SortColumn = "FirstName";
            filter.SortDirection = "ASC";
            filter.UserRoleId = UserRoleConstants.Customer;

            var contactList = _contactService.GetListByFilter(filter);

            var model = new ContactListViewModel
            {
                Contacts = contactList,
                SearchFilter = filter
            };

            return View(model);
        }

        #endregion

        #region Customer Details

        [Route("customer/{id:int}/{title}")]
        public ActionResult Details(int id)
        {
            Contact contact = _contactService.GetDetailsById(id);

            if (contact == null)
            {
                return HttpNotFound();
            }

            var model = Mapper.Map<AddEditCustomerViewModel>(contact);

            return View(model);
        }

        #endregion

        #region Add
        public ActionResult Create()
        {
            var model = new AddEditCustomerViewModel();

            return View(model);
        }

        [HttpPost]
        [ActionName("Create")]
        [ValidateAntiForgeryToken]
        public ActionResult Create(AddEditCustomerViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var newContact = Mapper.Map<Contact>(model);

            var isAdded = _contactService.Add(newContact);

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
            Contact contact = _contactService.GetDetailsById(id);

            if (contact == null)
            {
                return HttpNotFound();
            }

            var model= Mapper.Map<AddEditCustomerViewModel>(contact);

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(AddEditCustomerViewModel model)
        {
            ModelState.Remove("Username");
            ModelState.Remove("Password");
            ModelState.Remove("ConfirmPassword");

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var updateContact = Mapper.Map<Contact>(model);

            var isUpdated = _contactService.Update(updateContact);

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
            var deleteContact = new Contact {
                Id = id,
                Status = StatusConstants.Deleted
            };

            var isDeleted = _contactService.Remove(deleteContact);

            return RedirectToAction("List");
        }

        #endregion        
    }
}