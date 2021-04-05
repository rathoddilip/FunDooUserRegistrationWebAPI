using CommonLayer.NoteResponseModel;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;
using RepositoryLayer.InterFace;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RepositoryLayer.Services
{
    public class NotesRL : INotesRL
    {
        readonly DataContext noteContext;
        private IConfiguration Configuration { get; }
        ICollection<NoteModels> NotesModels;
        
        public NotesRL(DataContext context, IConfiguration configuration)
        {
            noteContext = context;
            Configuration = configuration;
           
        }
        public AddNotesModel AddUserNote(AddNotesModel noteModel)
        {
            try
            {
                var NewNote = new Note
                {
                    Id=noteModel.Id,
                    Title = noteModel.Title,
                    Body = noteModel.Body,
                    ReminderOn = noteModel.ReminderOn,
                    BackgroundColor = noteModel.BackgroundColor,
                    BackgroundImage = noteModel.BackgroundImage,
                    IsArchive = noteModel.IsArchive,
                    IsPin = noteModel.IsPin,
                    IsTrash = noteModel.IsTrash,

                };
                noteContext.Notes.Add(NewNote);
                noteContext.SaveChanges();
               // return noteModel;
                var NewResponseNote = new AddNotesModel
                {
                    Id = (long)NewNote.Id,
                    Title = NewNote.Title,
                    Body = NewNote.Body,                   
                    ReminderOn = NewNote.ReminderOn,
                    BackgroundColor = NewNote.BackgroundColor,
                    BackgroundImage = NewNote.BackgroundImage,
                    IsArchive = NewNote.IsArchive,
                    IsPin = NewNote.IsPin,
                    IsTrash = NewNote.IsTrash,
                   
                };
                return NewResponseNote;

            }

            catch (Exception ex)
            {
                throw ex;
            }
        }
        public bool DeleteNote(long Id, long noteId)
        {
            try
            {
                if (noteContext.Notes.Any(n => n.NoteId == noteId && n.Id == Id))
                {
                    var note = noteContext.Notes.Find(noteId);
                    if (note.IsTrash)
                    {
                        noteContext.Entry(note).State = EntityState.Deleted;
                    }
                    else
                    {
                        note.IsTrash = true;
                        note.IsPin = false;
                        note.IsArchive = false;
                    }
                    noteContext.SaveChanges();
                    return true;
                }
                return false;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public ICollection<NoteModels> GetNotes(long Id, bool IsTrash, bool IsArchieve)
        {
           
            try
            {
                
                NotesModels = noteContext.Notes.Where(N => N.Id.Equals(Id)
                && N.IsTrash == IsTrash && N.IsArchive == IsArchieve).OrderBy(N => N.StartDate).Select(N =>
                    new NoteModels
                    {
                        Id = (long)N.Id,
                        Title = N.Title,
                        Body = N.Body,
                        ReminderOn = N.ReminderOn,
                        BackgroundColor = N.BackgroundColor,
                        BackgroundImage = N.BackgroundImage,
                        IsArchive = N.IsArchive,
                        IsPin = N.IsPin,
                        IsTrash = N.IsTrash,
                        StartDate=N.StartDate,
                        ModificationDate=N.ModificationDate,
                    }
                    ).ToList();
                return NotesModels;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public NoteModels UpdateNote(NoteModels note)
        {

            try
            {
               
                var updateNote = noteContext.Notes.FirstOrDefault(N => N.Id == note.Id && N.NoteId==note.NoteId);
                if (updateNote != null)
                {
                    updateNote.Title = note.Title;
                    updateNote.Body = note.Body;
                    updateNote.ReminderOn = note.ReminderOn;
                    updateNote.BackgroundColor = note.BackgroundColor;
                    updateNote.BackgroundImage = note.BackgroundImage;
                    updateNote.IsArchive = note.IsArchive;
                    updateNote.IsPin = note.IsPin;
                    updateNote.IsTrash = note.IsTrash;
                }
                noteContext.SaveChanges();
                
                var NewResponseNote = noteContext.Notes.Where(N => N.NoteId == note.NoteId).Select(N =>
                    new NoteModels
                    {
                        NoteId=N.NoteId,
                        Id = (long)N.Id,
                        Title = N.Title,
                        Body = N.Body,
                        ReminderOn = N.ReminderOn,
                        BackgroundColor = N.BackgroundColor,
                        BackgroundImage = N.BackgroundImage,
                        IsArchive = N.IsArchive,
                        IsPin = N.IsPin,
                        IsTrash = N.IsTrash,
                    }
                    ).ToList().First();
                return NewResponseNote;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public ICollection<NoteModels> GetReminderNotes(long Id)
        {
        try
        {

                NotesModels = noteContext.Notes.Where(N => N.Id.Equals(Id)
                && N.IsTrash == false && N.ReminderOn > DateTime.Now).OrderBy(N => N.ReminderOn).Select(N =>
                new NoteModels
                {
                    Id = (long)N.Id,
                    NoteId= N.NoteId,
                    Title = N.Title,
                    Body = N.Body,
                    ReminderOn = N.ReminderOn,
                    BackgroundColor = N.BackgroundColor,
                    BackgroundImage = N.BackgroundImage,
                    IsArchive = N.IsArchive,
                    IsPin = N.IsPin,
                    IsTrash = N.IsTrash,
                }
                ).ToList();
            return NotesModels;
        }
        catch (Exception)
        {
            throw;
        }
    }

        public bool ToggleNotePin(long noteId, long Id)
        {
            try
            {
                var note = noteContext.Notes.FirstOrDefault(N => N.NoteId == noteId && N.Id == Id);
                if (note.IsPin)
                {
                    note.IsPin = false;
                }
                else
                {
                    note.IsPin = true;
                }
                noteContext.SaveChanges();
                return true;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public bool ToggleArchive(long noteId, long Id)
        {
            try
            {
                var note = noteContext.Notes.FirstOrDefault(N => N.NoteId == noteId && N.Id == Id);
                if (note.IsArchive)
                {
                    note.IsArchive = false;
                }
                else
                {
                    note.IsArchive = true;
                }
                noteContext.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public bool ChangeBackgroundColor(long noteId, long Id, string colorCode)
        {
            try
             {
                var note = noteContext.Notes.FirstOrDefault(N => N.NoteId == noteId && N.Id == Id);
                if (colorCode != null)
                 {
                     note.BackgroundColor = "#" + colorCode;
                 }
                noteContext.SaveChanges();
                 return true;
              }
            catch (Exception)
            {

                throw;
            }
        }

        public bool SetNoteReminder(NoteReminder reminder)
        {
            try
            {
                noteContext.Notes.FirstOrDefault(
                    N => N.NoteId == reminder.NoteId && N.Id == reminder.Id).ReminderOn = reminder.ReminderOn;
                noteContext.SaveChanges();
                return true;
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
