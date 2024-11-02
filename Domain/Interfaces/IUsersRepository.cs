using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public interface IUsersRepository
    {
        public IEnumerable<User> GetUsers();

        User? GetUserByUserName(string userName);
        User? Login(string userName);

        public User? GetUser(int idUser);
        void AddUser(User newUser);
        void DeleteUser(User user);
        bool EmailExists(string email);
        bool UserNameExists(string name);

        bool IsValidEmail(string email);
        bool SaveChanges();
        void Update(User user);
    }
}
