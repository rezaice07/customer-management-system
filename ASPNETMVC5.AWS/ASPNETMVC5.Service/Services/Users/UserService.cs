using ASPNETMVC5.Core.Filters;
using ASPNETMVC5.Data.Models;
using ASPNETMVC5.Service.Services.Encryptions;
using PagedList;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace ASPNETMVC5.Service.Services.Users
{
    public class UserService : IUserService
    {
        public IEnumerable<User> GetListByFilter(UserSearchFilter filter)
        {
            List<User> contactList = new List<User>();
            using (ClientManagementContext db = new ClientManagementContext())
            {
                var query = db.Users
                    .Include(f=>f.Contact)
                         .Include(f => f.UserRole)
                                .Where(f => (filter.SearchTerm == string.Empty || f.Username.Contains(filter.SearchTerm.Trim())
                                                                   || f.Contact.Email.Contains(filter.SearchTerm.Trim())
                                                                     || f.Contact.FirstName.Contains(filter.SearchTerm.Trim())
                                                                   || f.Contact.CellPhone.Contains(filter.SearchTerm.Trim())));
                filter.TotalCount = query.Count();

                //sorting 
                Func<User, object> OrderByStringField = null;

                switch (filter.SortColumn)
                {
                    case "Username":
                        OrderByStringField = p => p.Username;
                        break;
                    case "Contact.FirstName":
                        OrderByStringField = p => p.Contact.FirstName;
                        break;
                    case "Contact.Email":
                        OrderByStringField = p => p.Contact.Email;
                        break;
                    case "CellPhone":
                        OrderByStringField = p => p.Contact.CellPhone;
                        break;
                    default:
                        OrderByStringField = p => p.Contact.FirstName;
                        break;
                }
                //end sorting  

                var finalQuery = filter.SortDirection == "ASC" ? query.OrderBy(OrderByStringField) : query.OrderByDescending(OrderByStringField);

                contactList = finalQuery.Skip((filter.PageNumber - 1) * filter.PageSize)
                                            .Take(filter.PageSize)
                                            .AsParallel()
                                            .ToList();
            }

            return new StaticPagedList<User>(contactList, filter.PageNumber, filter.PageSize, filter.TotalCount);
        }      

        public User GetDetailsById(int id)
        {
            var filteredListing = new User();
            using (var db = new ClientManagementContext())
            {
                filteredListing = db.Users.Include(i=>i.Contact).FirstOrDefault(d=>d.Id==id);
            }

            return filteredListing;

        }

        public User GetDetailsByUsername(string username)
        {
            var filteredListing = new User();

            using (var db = new ClientManagementContext())
            {
                filteredListing = db.Users
                    .Include(i=>i.Contact)
                    .Include(i => i.UserRole)
                    .FirstOrDefault(d => d.Username == username);
            }

            return filteredListing;

        }

        public bool Add(User user)
        {
            try
            {
                using (var db = new ClientManagementContext())
                {

                    db.Users.Add(user);
                    db.SaveChanges();

                    return true;
                }
            }
            catch
            {
                return false;
            }
        }

        public bool Update(User user)
        {
            try
            {
                using (var db = new ClientManagementContext())
                {
                    var upateUser = db.Users.FirstOrDefault(d => d.Id == user.Id);

                    if (upateUser == null)
                        return false;

                    upateUser.Username = user.Username;              
                 
                    db.SaveChanges();

                    return true;
                }
            }
            catch
            {
                return false;
            }
        }

        public bool ResetPassword(User user)
        {
            try
            {
                using (var db = new ClientManagementContext())
                {
                    var upateUser = db.Users.FirstOrDefault(d => d.Id == user.Id);

                    if (upateUser == null)
                        return false;

                    var randomPass = EncryptionService.GenerateRandomPassword(8);
                    var salt = EncryptionService.CreateRandomSalt();
                    var passwordHash = EncryptionService.HashPassword(randomPass, salt);

                    upateUser.PasswordHash = passwordHash;
                    upateUser.PasswordSalt = salt;

                    db.SaveChanges();

                    return true;
                }
            }
            catch
            {
                return false;
            }
        }

        public bool Remove(User contact)
        {
            try
            {
                using (var db = new ClientManagementContext())
                {
                    var removeUser = db.Users.FirstOrDefault(d => d.Id == contact.Id);

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
