using System;
using System.Collections.Generic;
using System.Text;

namespace CommonLayer.ResponseModel
{
   public class UserModel: TimeDateModel
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Username { get; set; }
        public string EmailAddress { get; set; }
        public string PhoneNumber { get; set; }
        public string Password { get; set; }
    }
}
