using AutoMapper;
using CommonLayer.Helpers;
using CommonLayer.ResponseModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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

namespace FunDooUserAppAPI.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private IUserRL _userService;
        private IMapper _mapper;
        private readonly AppSettings _appSettings;
        //DateTime thisDay = DateTime.Today;
        DateTime baseDate = DateTime.Today;
        public UsersController(IUserRL userService, IMapper mapper, IOptions<AppSettings> appSettings)
        {
            _userService = userService;
            _mapper = mapper;
            _appSettings = appSettings.Value;
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public IActionResult Register(RegisterModel model)
        {
            // map model to entity
            var user = _mapper.Map<User>(model);
            try
            {
                // create user
                _userService.Create(user, model.Password);
                //return Ok();
                return this.Ok(new { Success = "True", Message = "Successfull" });
            }
            catch (AppException ex)
            {
                // return error message if there was an exception
                return BadRequest(new { message = "Failed" });
            }
        }
        [AllowAnonymous]
        [HttpPost("Login")]
        public IActionResult LoginUser(LoginModel model)
        {
                var user = _userService.Authenticate(model.EmailAddress, model.Password);

                if (user == null)
                    return BadRequest(new { message = "EmailAddress or password is incorrect" });

                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new Claim[]
                    {
                    new Claim(ClaimTypes.Name, user.Id.ToString())
                    }),
                    Expires = DateTime.UtcNow.AddDays(7),
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
                };
                var token = tokenHandler.CreateToken(tokenDescriptor);
                var tokenString = tokenHandler.WriteToken(token);

                // return basic user info and authentication token
                return Ok(new
                {
                    Id = user.Id,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Username = user.Username,
                    EmailAddress = user.EmailAddress,
                    Password = user.Password,
                    PhoneNumber = user.PhoneNumber,
                    StartDate = user.StartDate.AddDays(-(int)baseDate.DayOfWeek),
                    ModificationDate = user.ModificationDate.TimeOfDay,
                    Token = tokenString
                });
            if (ModelState.IsValid)
            {
                User result = this._userService.Login(model);
                if (result != null)
                {
                    //this.Ok returns the data in json format
                    return this.Ok(new { Success = true, Message = "Login Successfull" });
                }
                else
                {
                    return this.BadRequest(new { Success = false, Message = "Login Unsuccessfull" });
                }
            }

        }

        [Authorize]
        [HttpGet]
        public IActionResult GetAll()
        {
            var users = _userService.GetAll();
            var model = _mapper.Map<IList<UserModel>>(users);
            return Ok(model);
            
        }
        [Authorize]
        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var user = _userService.GetById(id);
            var model = _mapper.Map<UserModel>(user);
            return Ok(model);
        }

        [Authorize]
        [HttpPut("{id}")]
        public IActionResult ResetPassWord(int id, UpdateModel model)
        {
            // map model to entity and set id
            var user = _mapper.Map<User>(model);
            user.Id = id;

            try
            {
                // reset password 
                _userService.ResetPassword(user, model.Password);
                return this.Ok(new { Success = "True", Message = "Successfully reset password" });
            }
            catch (AppException ex)
            {
                // return error message if there was an exception
                return BadRequest(new { message = "Failed reset password" });
            }
        }
        [Authorize]
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            _userService.Delete(id);
            return this.Ok( new { Success = "True", Message = "Successfully delete user" });
        }
    }
}
