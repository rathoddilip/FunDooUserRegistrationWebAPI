using CommonLayer.Helpers;
using CommonLayer.ResponseModel;
using Microsoft.IdentityModel.Tokens;
using RepositoryLayer.Helpers;
using RepositoryLayer.InterFace;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using AutoMapper;
using CommonLayer.Helpers;
using CommonLayer.ResponseModel;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using RepositoryLayer.Helpers;
using RepositoryLayer.InterFace;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
namespace RepositoryLayer.Services
{
        public class UserRL : IUserRL
        {
            private DataContext _context;
            private readonly AppSettings _appSettings;
        public UserRL(DataContext context, IOptions<AppSettings> appSettings)
            {
                _context = context;  
                _appSettings = appSettings.Value;
            
            }
            public User Authenticate(string emailAddress, string password)
            {
                if (string.IsNullOrEmpty(emailAddress) || string.IsNullOrEmpty(password))
                    return null;

                var user = _context.Users.SingleOrDefault(x => x.EmailAddress == emailAddress);

                // check if username exists
                if (user == null)
                    return null;

            // check if password is correct
            if (password == null)
                    return null;

                // authentication successful
                return user;
            }

            public IEnumerable<User> GetAll()
            {
                return _context.Users;
            }

            public User GetById(int id)
            {
                return _context.Users.Find(id);
            }

            public User Create(User user, string password)
            {
                // validation
                if (string.IsNullOrWhiteSpace(password))
                    throw new AppException("Password is required");

                if (_context.Users.Any(x => x.EmailAddress == user.EmailAddress))
                    throw new AppException("EmailAddress \"" + user.EmailAddress + "\" is already taken");

                _context.Users.Add(user);
                _context.SaveChanges();

                return user;
            }
        public User Login(LoginModel login)
        {
            LoginModel loginModel = new LoginModel
            {
                EmailAddress = login.EmailAddress,
                Password = login.Password,
            };

            User searchLogin = new User { EmailAddress = loginModel.EmailAddress, Password = loginModel.Password };
                return searchLogin;
            

        }
    
        public void ResetPassword(User userParam, string password = null)
            {
                var user = _context.Users.Find(userParam.Id);

                if (user == null)
                    throw new AppException("User not found");

                // update user properties if provided
                if (!string.IsNullOrWhiteSpace(userParam.FirstName))
                    user.FirstName = userParam.FirstName;

                if (!string.IsNullOrWhiteSpace(userParam.LastName))
                    user.LastName = userParam.LastName;

                // update password if provided
                if (!string.IsNullOrWhiteSpace(password))
                {
                    user.Password = password;
                }

                _context.Users.Update(user);
                _context.SaveChanges();
            }

            public void Delete(int id)
            {
                var user = _context.Users.Find(id);
                if (user != null)
                {
                    _context.Users.Remove(user);
                    _context.SaveChanges();
                }
            }
        public string GenerateToken(User login)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, login.Id.ToString())
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);

            return tokenString;
        }

        
    }
}
