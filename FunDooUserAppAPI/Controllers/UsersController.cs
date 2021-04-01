
using BusinessLayer.InterFace;
using CommonLayer.Helpers;
using CommonLayer.ResponseModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using RepositoryLayer.Helpers;
using RepositoryLayer.InterFace;
using RepositoryLayer.Services;
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
        IUserBL userBL;
        private IConfiguration Configuration { get; }
        public UsersController(IUserBL userBL, IConfiguration configuration)
        {
            //to get an access of IUserBL
            this.userBL = userBL;
            Configuration = configuration;


        }

        /// <summary>
        /// Register New user
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost("Register")]
        //Here return type represents the result of an action method
        public IActionResult Registration(User user)
        {
            try
            {
                if (ModelState.IsValid)
                {

                    bool result = this.userBL.Registration(user);
                    if (result != false)
                    {
                        //this.Ok returns the data in json format
                        return this.Ok(new { Success = true, Message = "Register Record Successfully" });
                    }
                    else
                    {
                        return this.BadRequest(new { Success = false, Message = "Register Record Unsuccessfully" });
                    }
                }

                else
                {
                    throw new Exception("Model is not valid");
                }
            }
            catch(ApplicationException exception)
            {
                throw new ApplicationException("Registration failed please enter data carefully");
            }
        }

        /// <summary>
        /// Login 
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost("Login")]
        //Here return type represents the result of an action method
        public IActionResult Login(LoginModel user)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    User result = this.userBL.Login(user);
                    var tokenString = this.userBL.GenerateToken(result);
                    if (result != null)
                    {
                        //this.Ok returns the data in json format
                        return this.Ok(new
                        {
                            Success = true,
                            Message = "Login Successfully",
                            Id = result.Id,
                            EmailAddress = result.EmailAddress,
                            token = tokenString
                        });
                    }
                    else
                    {
                        return this.BadRequest(new { Success = false, Message = "Login failed" });
                    }
                }
                else
                {
                    throw new Exception("Model is not valid");
                }
            }
            catch (AppException exception)
            {
                throw new AppException("Please check emailAddress and password");
            }           

        }


        [Authorize]
        [HttpPost("Authorize")]
        public IActionResult Authorization()
        {

            var identity = User.Identity as ClaimsIdentity;
            if (identity != null)
            {
                var claims = identity.Claims;
                var name1 = claims.Where(x => x.Type == "Id").FirstOrDefault()?.Value;
                var name = claims.Where(x => x.Type == "EmailAddress").FirstOrDefault()?.Value;
                string[] list = { name, name1 };

                return this.Ok(new { Success = true, Message = "Authorization is  Successfull", Data = list });


            }
            return null;
        }

        /// <summary>
        /// Reset password
        /// </summary>
        /// <param name="resetPasswordModel"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPost("reset")]
        public IActionResult ResetPassword(ResetPasswordModel reset)
        {
            try
            {
                var identity = User.Identity as ClaimsIdentity;
                if (identity != null)
                {
                    IEnumerable<Claim> claims = identity.Claims;
                    var Id = claims.Where(x => x.Type == "Id").FirstOrDefault()?.Value;
                    var EmailAddress = claims.Where(p => p.Type == "EmailAddress").FirstOrDefault()?.Value;

                    bool result = this.userBL.ResetAccountPassword(reset, EmailAddress);
                    if (result)
                    {
                        return Ok(new { success = true, Message = "password is reset successfully" });
                    }
                }
                return BadRequest(new { success = false, Message = "password is change unsuccessfully" });
            }
            catch (Exception exception)
            {
                return BadRequest(new { success = false, exception.Message });
            }
        }

        /// <summary>
        /// Forget password 
        /// </summary>
        /// <param name="forgetPasswordModel"></param>
        /// <returns></returns>
        [HttpPost("forgetPassword")]
        public ActionResult ForgetPassword(ForgetPasswordModel forgetPasswordModel)
        {
            if (ModelState.IsValid)
            {
                ForgetClass result = userBL.ForgetPassword(forgetPasswordModel);                  
                var msmq = new MSMQ(Configuration);
                msmq.MSMQSender(result);
                if (result != null)
                {
                    return this.Ok(new { Success = true, Message = "Your password has been forget sucessfully now you can reset your password" }); 
                }

                else
                {
                    return this.Ok(new { Success = true, Message = "Other User is trying to login from your account" }); 
                }
            }
            else
            {
                throw new Exception("Model is not valid");
            }
        }
        /// <summary>
        /// Get Deatils of particular ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        //Here return type represents the result of an action method
        public IActionResult GetUser(int id)
        {
            try
            {
                //getting the data from BusinessLayer
                User result = userBL.Get(id);
                //this.Ok returns the data in json format
                return this.Ok(new { Success = true, Message = "Get Successful", Data = result });
            }

            catch (Exception e)
            {
                return this.BadRequest(new { Success = false, Message = e.Message });
            }
        }

    }
}
