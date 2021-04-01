using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace CommonLayer.ResponseModel
{
    public class RegisterModel: TimeDateModel
    {
        DateTime baseDate = DateTime.Today;
        DateTime now = DateTime.Now;
        /* [Required]
         public string Id { get; set; }*/
        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        public string Username { get; set; }

        [Required]
        public string Password { get; set; }
        [Required]
        public string EmailAddress { get; set; }
        [Required]
        public string PhoneNumber { get; set; }
        //[DataType(DataType.Date)]
        //[DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}")]
       
    }
}
