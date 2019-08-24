using System.Collections.Generic;
using WebAPI.Entities;

namespace WebAPI.Services.Interfaces
{
    public interface IUserService
    {
        bool AuthenticateByEmail(string username, string authName);
        /*IEnumerable<UserToRegister> GetUsers();*/
        User GetById(string id);
        User GetByUsername(string username);
        UserToRegister Register(UserToRegister user);
        User CompleteRegistration(string id);
        //void Update(UserToRegister user, string password = null);
        void Delete(string id);
    }
}
