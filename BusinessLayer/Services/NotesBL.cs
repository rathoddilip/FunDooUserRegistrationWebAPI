using BusinessLayer.InterFace;
using CommonLayer.Helpers;
using CommonLayer.NoteResponseModel;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using RepositoryLayer.InterFace;
using RepositoryLayer.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Services
{
    public class NotesBL : INotesBL
    {
        INotesRL notesRL;
        private readonly IDistributedCache distributedCache;
        RedisCacheServiceBL redis;
        public NotesBL(INotesRL notesRL, IDistributedCache distributedCache)
        {
            this.notesRL = notesRL;
            this.distributedCache = distributedCache;
            redis = new RedisCacheServiceBL(this.distributedCache);
        }
        public AddNotesModel AddUserNote(AddNotesModel note)
        {
            try
            {

                return AddUserNoteTask(note).Result;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<AddNotesModel> AddUserNoteTask(AddNotesModel note)
        {
            try
            {
                if (note.IsTrash || note.IsArchive)
                {
                    note.IsPin = false;
                }
                await redis.RemoveNotesRedisCache(note.Id);
                return notesRL.AddUserNote(note);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<bool> DeleteNote(long Id, long noteId)
        {
            try
            {
                await redis.RemoveNotesRedisCache(Id);
                bool result = notesRL.DeleteNote(Id, noteId);
                return result;
                //return await Task.FromResult(result);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<ICollection<NoteModels>> GetTrashNotes(long Id)
        {

            var cacheKey = "ReminderNotes:" + Id.ToString();
            string serializedNotes;
            ICollection<NoteModels> Notes;
            try
            {
                var redisNoteCollection = await distributedCache.GetAsync(cacheKey);
                if (redisNoteCollection != null)
                {
                    serializedNotes = Encoding.UTF8.GetString(redisNoteCollection);
                    Notes = JsonConvert.DeserializeObject<List<NoteModels>>(serializedNotes);
                }
                else
                {  
                    Notes = notesRL.GetNotes(Id, true, false);
                    await redis.AddRedisCache(cacheKey, Notes);
                }
                return Notes;
            }

            catch (Exception)
            {
                throw;
            }
        }
        public async Task<ICollection<NoteModels>> GetActiveNotes(long Id)
        {
            var cacheKey = "ActiveNotes:" + Id.ToString();
            string serializedNotes;
            ICollection<NoteModels> Notes;
            try
            {
                var redisNoteCollection = await distributedCache.GetAsync(cacheKey);
                if (redisNoteCollection != null)
                {
                    serializedNotes = Encoding.UTF8.GetString(redisNoteCollection);
                    Notes = JsonConvert.DeserializeObject<List<NoteModels>>(serializedNotes);
                }
                else
                {
                    Notes = notesRL.GetNotes(Id, false, false);
                    await redis.AddRedisCache(cacheKey, Notes);
                }
                return Notes;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public async Task<ICollection<NoteModels>> GetArchiveNotes(long Id)
        {
            var cacheKey = "ArchiveNotes:" + Id.ToString();
            string serializedNotes;
            ICollection<NoteModels> Notes;
            try
            {
                var redisNoteCollection = await distributedCache.GetAsync(cacheKey);
                if (redisNoteCollection != null)
                {
                    serializedNotes = Encoding.UTF8.GetString(redisNoteCollection);
                    Notes = JsonConvert.DeserializeObject<List<NoteModels>>(serializedNotes);
                }
                else
                {
                    Notes = notesRL.GetNotes(Id, false, true);
                    await redis.AddRedisCache(cacheKey, Notes);
                }
                return Notes;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public async Task<ICollection<NoteModels>> GetReminderNotes(long Id)
        {
            var cacheKey = "ReminderNotes:" + Id.ToString();
            string serializedNotes;
            ICollection<NoteModels> Notes;
            try
            {
                var redisNoteCollection = await distributedCache.GetAsync(cacheKey);
                if (redisNoteCollection != null)
                {
                    serializedNotes = Encoding.UTF8.GetString(redisNoteCollection);
                    Notes = JsonConvert.DeserializeObject<List<NoteModels>>(serializedNotes);
                }
                else
                {
                    Notes = notesRL.GetReminderNotes(Id);
                    await redis.AddRedisCache(cacheKey, Notes);
                }
                return Notes;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public async Task<bool> ToggleArchive(long noteId, long Id)
        {
            try
            {
                await redis.RemoveNotesRedisCache(Id);
                return notesRL.ToggleArchive(noteId, Id);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<bool> ToggleNotePin(long noteId, long Id)
        {
            try
            {
                await redis.RemoveNotesRedisCache(Id);
                return notesRL.ToggleNotePin(noteId, Id);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<bool> ChangeBackgroundColor(long noteId, long Id, string colorCode)
        {
            try
            {
                await redis.RemoveNotesRedisCache(Id);
                return notesRL.ChangeBackgroundColor(noteId, Id, colorCode);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<bool> SetNoteReminder(NoteReminder reminder)
        {
            try
            {
                await redis.RemoveNotesRedisCache(reminder.Id);
                if (reminder.ReminderOn < DateTime.Now)
                {
                    throw new Exception("Time is passed");
                }
                if (reminder.NoteId == default)
                {
                    throw new Exception("NoteID missing");
                }
                return notesRL.SetNoteReminder(reminder);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<NoteModels> UpdateNote(NoteModels note)
        {
            try
            {
                await redis.RemoveNotesRedisCache(note.Id);
                if (note.IsTrash || note.IsArchive)
                {
                    note.IsPin = false;
                }
                return notesRL.UpdateNote(note);
            }
            catch (Exception)
            {
                throw;
            }
        }

        
    }
}
