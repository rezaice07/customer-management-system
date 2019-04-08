using ASPNETMVC5.Core.Filters;
using ASPNETMVC5.Data.Models;
using ASPNETMVC5.Service.Services.Encryptions;
using PagedList;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using static ASPNETMVC5.Core.AppConstants;

namespace ASPNETMVC5.Service.Services.Contacts
{
    public class ContactService : IContactService
    {
        public IPagedList<Contact> GetListByFilter(ContactSearchFilter filter)
        {
            List<Contact> contactList = new List<Contact>();
            using (ClientManagementContext db = new ClientManagementContext())
            {
                var query = db.Contacts                    
                                .Where(f => 
                                (f.Status!=StatusConstants.Deleted) &&
                                (filter.UserRoleId ==null || filter.UserRoleId==0 || f.Users.Any(a=>a.RoleId == filter.UserRoleId)) &&
                                (filter.SearchTerm == string.Empty || f.FirstName.Contains(filter.SearchTerm.Trim())
                                                                   || f.Email.Contains(filter.SearchTerm.Trim())
                                                                   || f.CellPhone.Contains(filter.SearchTerm.Trim())));
                filter.TotalCount = query.Count();

                //sorting 
                Func<Contact, object> OrderByStringField = null;

                switch (filter.SortColumn)
                {
                    case "FirstName":
                        OrderByStringField = p => p.FirstName;
                        break;
                    case "Email":
                        OrderByStringField = p => p.Email;
                        break;
                    case "CellPhone":
                        OrderByStringField = p => p.CellPhone;
                        break;
                    default:
                        OrderByStringField = p => p.FirstName;
                        break;
                }
                //end sorting  

                var finalQuery = filter.SortDirection == "ASC" ? query.OrderBy(OrderByStringField) : query.OrderByDescending(OrderByStringField);

                contactList = finalQuery.Skip((filter.PageNumber - 1) * filter.PageSize)
                                            .Take(filter.PageSize)
                                            .AsParallel()
                                            .ToList();
            }

            return new StaticPagedList<Contact>(contactList, filter.PageNumber, filter.PageSize, filter.TotalCount);
        }      

        public Contact GetDetailsById(int id)
        {
            var filteredListing = new Contact();
            using (var db = new ClientManagementContext())
            {
                filteredListing = db.Contacts.FirstOrDefault(d=>d.Id==id);
            }

            return filteredListing;

        }

        public bool Add(Contact contact)
        {
            try
            {
                using (var db = new ClientManagementContext())
                {
                    db.Contacts.Add(contact);
                    db.SaveChanges();

                    var randomPass = EncryptionService.GenerateRandomPassword(8);
                    var salt = EncryptionService.CreateRandomSalt();
                    var passwordHash = EncryptionService.HashPassword(randomPass, salt);

                    var newUser = new User {
                        ContactId= contact.Id,
                        PasswordHash= passwordHash,
                        PasswordSalt= salt,
                        RoleId=UserRoleConstants.Customer,
                        Username=contact.Username,
                        Status=StatusConstants.Active,
                        CreatedDateUtc=contact.CreatedDate
                    };

                    db.Users.Add(newUser);
                    db.SaveChanges();

                    return true;
                }
            }
            catch(Exception ex)
            {
                return false;
            }
        }

        public bool Update(Contact contact)
        {
            try
            {
                using (var db = new ClientManagementContext())
                {
                    var upateContact = db.Contacts.FirstOrDefault(d => d.Id == contact.Id);

                    if (upateContact == null)
                        return false;

                    upateContact.FirstName = contact.FirstName;
                    upateContact.LastName = contact.LastName;
                    upateContact.Email = contact.Email;
                    upateContact.CellPhone = contact.CellPhone;
                    upateContact.Address = contact.Address;                   
                 
                    db.SaveChanges();

                    return true;
                }
            }
            catch(Exception ex)
            {
                return false;
            }
        }

        public bool Remove(Contact contact)
        {
            try
            {
                using (var db = new ClientManagementContext())
                {
                    var removeContact = db.Contacts.FirstOrDefault(d => d.Id == contact.Id);

                    if (removeContact == null)
                        return false;
                    removeContact.Status = contact.Status;

                    db.SaveChanges();


                    var removeUser = db.Users.FirstOrDefault(d => d.ContactId == contact.Id);

                    if (removeUser == null)
                        return false;
                    removeUser.Status = contact.Status;

                    db.SaveChanges();

                    return true;
                }
            }
            catch
            {
                return false;
            }
        }
    }
}
