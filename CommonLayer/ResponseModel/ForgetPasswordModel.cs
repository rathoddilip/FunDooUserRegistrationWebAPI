using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace CommonLayer.ResponseModel
{
    public class ForgetPasswordModel:TimeDateModel
    {
        [Required]
        public string EmailAddress { get; set; }
    }
}
