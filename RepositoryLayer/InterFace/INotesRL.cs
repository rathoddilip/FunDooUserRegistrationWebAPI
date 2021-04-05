using CommonLayer.NoteResponseModel;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryLayer.InterFace
{
    public interface INotesRL
    {
        public AddNotesModel AddUserNote(AddNotesModel noteModel);
        bool DeleteNote(long Id, long noteId);
        public ICollection<NoteModels> GetNotes(long Id, bool IsTrash, bool IsArchieve);
        NoteModels UpdateNote(NoteModels note);
        ICollection<NoteModels> GetReminderNotes(long Id);
        bool ToggleNotePin(long noteId, long Id);
        bool ToggleArchive(long noteId, long Id);
        bool ChangeBackgroundColor(long noteId, long Id, string colorCode);
        bool SetNoteReminder(NoteReminder reminder);
    }
}
