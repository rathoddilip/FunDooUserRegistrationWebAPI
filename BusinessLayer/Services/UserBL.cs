using BusinessLayer.InterFace;
using CommonLayer.Helpers;
using CommonLayer.ResponseModel;
using RepositoryLayer.InterFace;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLayer.Services
{
    public class UserBL : IUserBL

    {
        IUserRL userRL;

        public UserBL(IUserRL userRL)
        {
            this.userRL = userRL;
        }


        public User Get(int id)
        {
            try
            {

                return this.userRL.Get(id);
            }

            catch (Exception e)
            {
                throw e;
            }
        }

        public bool Registration(User user)
        {
            try
            {

                return this.userRL.Registration(user);                 
            }

            catch (AppException exception)
            {
                throw new AppException("Please check registration model");
            }
        }

        public string GenerateToken(User login)
        {
            try
            {

                return this.userRL.GenerateToken(login);                 
            }

            catch (AppException exception)
            {
                throw new AppException("Please check emailAddress and password");
            }
        }

        public ForgetClass ForgetPassword(ForgetPasswordModel forgetPasswordModel)
        {
            try
            {

                return this.userRL.ForgetPassword(forgetPasswordModel);                 
            }

            catch (AppException exception)
            {
                throw new AppException("Please check mailaddress");
            }
        }
        public User Login(LoginModel login)
        {
            try
            {

                return this.userRL.Login(login);                 
            }

            catch (AppException exception)
            {
                throw new AppException("Please check emailAddress and password");
            }
        }


        /// <summary>
        /// Resets the account password when password is known
        /// </summary>
        /// <param name="user">The user.</param>
        /// <returns></returns>
        /// <exception cref="UserDetailException">New and comfirm password do not match</exception>
        public bool ResetAccountPassword(ResetPasswordModel user, string EmailAddress)
        {
            try
            {

                {
                    return userRL.ResetAccountPassword(user, EmailAddress);
                }
            }
            catch (AppException exception)
            {
                throw new AppException("Please check tokan claim and password");
            }

        }



    }


}

