using BookStore.Models.DataModels;
using BookStore.Models.Models;
using System.Collections.Generic;
using System.Linq;

namespace BookStore.Repository
{
    public class UserRepository : BaseRepository
    {
        public ListResponse<Role> GetRoles()
        {
            var query = _context.Roles.AsQueryable();
            var totalRecords = query.Count();
            IEnumerable<Role> role = query;
            return new ListResponse<Role>()
            {
                records = role,
                totalRecords = totalRecords
            };
        }
        public User Login(LoginModel model)
        {
            User user = _context.Users.FirstOrDefault(c => c.Email.Equals(model.Email.ToLower()));
            if (user == null)
                return null;
            return _context.Users.FirstOrDefault(c => c.Email.Equals(model.Email.ToLower()) && c.Password.Equals(model.Password));
        }

        public User Register(RegisterModel model)
        {
            User user = new User()
            {
                Email = model.Email,
                Password = model.Password,
                Firstname = model.Firstname,
                Lastname = model.Lastname,
                Roleid = model.Roleid,
            };
            var entry = _context.Users.Add(user);
            _context.SaveChanges();
            return entry.Entity;
        }

        public ListResponse<User> GetUsers(int pageIndex, int pageSize, string keyword)
        {
            keyword = keyword?.ToLower().Trim();
            var query = _context.Users.Where(c
                => keyword == null
                || c.Firstname.ToLower().Contains(keyword)
                || c.Lastname.ToLower().Contains(keyword)
            ).AsQueryable();

            int totalRecords = query.Count();
            IEnumerable<User> users = query.Skip((pageIndex - 1) * pageSize).Take(pageSize);

            return new ListResponse<User>()
            {
                records = users,
                totalRecords = totalRecords
            };
        }

        public User GetUser(int id)
        {
            if (id > 0)
            {
                return _context.Users.Where(w => w.Id == id).FirstOrDefault();
            }

            return null;
        }

        public bool UpdateUser(User model)
        {
            if (model.Id > 0)
            {
                _context.Update(model);
                _context.SaveChanges();

                return true;
            }

            return false;
        }

        public bool DeleteUser(int id)
        {
            User user = _context.Users.FirstOrDefault(w => w.Id == id);
            if (user == null)
                return false;

            _context.Users.Remove(user);
            _context.SaveChanges();

            return true;
        }
    }
}
