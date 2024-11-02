using Domain.Entities;
using Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Data
{
    public class UsersRepository : IUsersRepository
    {
        private readonly SimplicityStoreContext _context;
        public UsersRepository(SimplicityStoreContext context)
        {
            _context = context;
        }

        public IEnumerable<User> GetUsers()
        {
            return _context.Users;
        }

        public User? Login(string userName)
        {
            return _context.Users.SingleOrDefault(p => p.UserName == userName);
        }

        public User? GetUserByUserName(string userName)
        {
            return _context.Users.SingleOrDefault(p => p.UserName == userName);
        }


        public User? GetUser(int idUser)
        {
            return _context.Users.FirstOrDefault(u => u.Id == idUser);
        }

        public void AddUser(User newUser)
        {
            _context.Users.Add(newUser);
        }

        public void Update(User user)
        {
            _context.Update(user);
        }

        public void DeleteUser(User user)
        {
            _context.Users.Remove(user);
        }

        public bool EmailExists(string email)
        {
            return _context.Users.Any(u => u.Email == email);
        }

        public bool UserNameExists(string name)
        {
            return _context.Users.Any(u => u.UserName == name);
        }

        public bool SaveChanges()
        {
            return (_context.SaveChanges() >= 0);
        }
        public bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch

            {
                return false;
            }
        }






    }

}

