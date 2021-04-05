using CommonLayer.ResponseModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Claims;
using System.Text;

namespace CommonLayer.NoteResponseModel
{
    public class NoteModels : TimeDateModel
    {
        
        public static ClaimsIdentity Identity { get; set; }
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        public long NoteId { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }
        public bool IsPin { get; set; }
        public bool IsArchive { get; set; }
        public bool IsTrash { get; set; }
        public DateTime? ReminderOn { get; set; }
        public string BackgroundColor { get; set; }
        public string BackgroundImage { get; set; }
       
    }
}
