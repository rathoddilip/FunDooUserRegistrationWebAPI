using CommonLayer.ResponseModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLayer.InterFace
{
   public interface IUserBL
    {
        public User Get(int id);
        public bool Registration(User user);
        public User Login(LoginModel user);
        public string GenerateToken(User login);
        public bool ResetAccountPassword(ResetPasswordModel user, string EmailAddress);
        public ForgetClass ForgetPassword(ForgetPasswordModel forgetPasswordModel);

    }
}
