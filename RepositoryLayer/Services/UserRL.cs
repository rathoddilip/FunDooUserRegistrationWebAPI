using CommonLayer.ResponseModel;
using Microsoft.IdentityModel.Tokens;
using RepositoryLayer.InterFace;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;

namespace RepositoryLayer.Services
{
    public class UserRL : IUserRL
    {
        readonly DataContext userContext;
        private IConfiguration Configuration { get; }
        public UserRL(DataContext context, IConfiguration configuration)
        {
            userContext = context;
            Configuration = configuration;
        }


        public User Get(int id)
        {

            return userContext.Users
                  .FirstOrDefault(e => e.Id == id);
        }


        public bool Registration(User user)
        {
            User register = new User
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                Username=user.Username,
                EmailAddress = user.EmailAddress,
                Password = PasswordEncriptDecript.ConvertToEncrypt(user.Password),
                PhoneNumber = user.PhoneNumber
            };

            userContext.Users.Add(register);
            userContext.SaveChanges();

            if (userContext.Users
                  .FirstOrDefault(e => e.Id == register.Id) != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public User Login(LoginModel login)
        {
            User searchLogin = userContext.Users.Where(e => e.EmailAddress.Equals(login.EmailAddress) && e.Password.Equals(login.Password)).FirstOrDefault(e => e.EmailAddress == login.EmailAddress);


            if (searchLogin != null)
            {
                searchLogin = new User { Id = searchLogin.Id, EmailAddress = searchLogin.EmailAddress };
                return searchLogin;
            }
            else
            {
                return searchLogin;
            }

        }
        public string GenerateToken(User login)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[] {

                  new Claim("Id", login.Id.ToString()),
                  new Claim("EmailAddress", login.EmailAddress),

              };
            var token = new JwtSecurityToken(Configuration["Jwt:Issuer"],
               Configuration["Jwt:Issuer"],
               claims,
               expires: DateTime.Now.AddMinutes(120),
               signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
        public ForgetClass ForgetPassword(ForgetPasswordModel forgetPasswordModel)
        {
            ForgetModel forget = new ForgetModel();
            User login = new User();
            login.EmailAddress = forgetPasswordModel.EmailAddress;
            User validateEmail = userContext.Users
                  .Where(e => e.EmailAddress.Equals(login.EmailAddress)).FirstOrDefault(e => e.EmailAddress == login.EmailAddress);
            var jwt = GenerateToken(validateEmail);
            forget.JwtToken = jwt;


            if (validateEmail != null)
            {
                var model1 = new ForgetPasswordModel { EmailAddress = forgetPasswordModel.EmailAddress };
                var model2 = new ForgetModel { JwtToken = forget.JwtToken };
                var model = new ForgetClass { EmailAddress = model1.EmailAddress, JwtToken = model2.JwtToken };
                return model;
            }
            else
            {
                return null;
            }
        }

        public bool ResetAccountPassword(ResetPasswordModel reset, string EmailAddress)
        {

          
            ResetPasswordModel password = new ResetPasswordModel
            {
                NewPassword = reset.NewPassword,
                ConfirmPassword = reset.ConfirmPassword,
            };

            if (password.NewPassword == password.ConfirmPassword)
            {
                var dbPassword = userContext.Users.FirstOrDefault(u => u.EmailAddress.Equals(EmailAddress));
                var newPassword = PasswordEncriptDecript.ConvertToEncrypt(reset.ConfirmPassword);
                dbPassword.Password = newPassword;
                userContext.SaveChanges();

                return true;
            }
            else
            {
                return false;
            }

        }

    }
}
