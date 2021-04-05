using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace CommonLayer.NoteResponseModel
{
    public class NoteReminder
    {
        public long Id { get; set; }
        [Required]
        public long NoteId { get; set; }
        [Required]
        public DateTime ReminderOn { get; set; }
    }
}
