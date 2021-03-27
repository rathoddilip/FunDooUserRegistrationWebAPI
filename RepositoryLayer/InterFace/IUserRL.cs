using CommonLayer.ResponseModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace RepositoryLayer.InterFace
{
    public interface IUserRL
    {
        User Authenticate(string emailAddress, string password);
        IEnumerable<User> GetAll();
        User GetById(int id);
        public User Login(LoginModel login);
        User Create(User user, string password);
        void ResetPassword(User user, string password = null);
        void Delete(int id);
        public string GenerateToken(User login);
    }
}
