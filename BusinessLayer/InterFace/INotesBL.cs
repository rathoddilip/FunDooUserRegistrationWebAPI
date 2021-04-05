using CommonLayer.NoteResponseModel;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.InterFace
{
   public interface INotesBL
    {
        public AddNotesModel AddUserNote(AddNotesModel note);
        Task<bool> DeleteNote(long Id, long noteId);
        Task<ICollection<NoteModels>> GetTrashNotes(long Id);
        public Task<ICollection<NoteModels>> GetActiveNotes(long Id);
        Task<NoteModels> UpdateNote(NoteModels note);
        Task<ICollection<NoteModels>> GetReminderNotes(long Id);
        Task<bool> ToggleNotePin(long noteId, long Id);
        Task<bool> ToggleArchive(long noteId, long Id);
        Task<bool> ChangeBackgroundColor(long noteId, long Id, string colorCode);
        Task<bool> SetNoteReminder(NoteReminder reminder);
    }
}
