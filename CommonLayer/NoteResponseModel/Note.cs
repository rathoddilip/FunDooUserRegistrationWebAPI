using CommonLayer.ResponseModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace CommonLayer.NoteResponseModel
{
    public  class Note
    {
        public long NoteId { get; set; }
        public long? Id { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }
        public bool IsPin { get; set; }
        public bool IsArchive { get; set; }
        public bool IsTrash { get; set; }
        public DateTime? ReminderOn { get; set; }
        public string BackgroundColor { get; set; }
        public string BackgroundImage { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime ModificationDate { get; set; }

       
    }
}
