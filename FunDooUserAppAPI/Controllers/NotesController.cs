using BusinessLayer.InterFace;
using CommonLayer.NoteResponseModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace FunDooUserAppAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotesController : ControllerBase
    {
        INotesBL noteBL;
        private IConfiguration Configuration { get; }
        public NotesController(INotesBL noteBL, IConfiguration configuration)
        {
            //to get an access of IUserBL
            this.noteBL = noteBL;
            Configuration = configuration;
        }
        /// <summary>
        /// Add the notes
        /// </summary>
        /// <param name="note"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPost("AddNote")]
        public IActionResult AddUserNote(AddNotesModel note)
        {
            try
            {
                var identity = User.Identity as ClaimsIdentity;
                if (identity != null)
                {
                    IEnumerable<Claim> claims = identity.Claims;
                    long Id = Convert.ToInt64(claims.Where(p => p.Type == "Id").FirstOrDefault()?.Value);
                    note.Id = Id;
                    AddNotesModel result = noteBL.AddUserNote(note);
                    return Ok(new { success = true, Note = result });
                }
                return BadRequest(new { success = false, Message = "no user is active please login" });
            }
            catch (Exception exception)
            {
                return BadRequest(new { success = false, exception.Message });
            }
        }
        /// <summary>
        /// Get active notes
        /// </summary>
        /// <returns></returns>
        [Authorize]
        [HttpGet]
        public IActionResult GetActiveNotes()
        {
            try
            {
                var identity = User.Identity as ClaimsIdentity;
                if (identity != null)
                {
                    IEnumerable<Claim> claims = identity.Claims;
                    long Id = Convert.ToInt64(claims.Where(p => p.Type == "Id").FirstOrDefault()?.Value);
                    string EmailAddress = claims.Where(p => p.Type == "EmailAddress").FirstOrDefault()?.Value;

                    var result = noteBL.GetActiveNotes(Id);
                    return Ok(new { success = true, user = EmailAddress, Notes = result });
                }
                return BadRequest(new { success = false, Message = "no user is active please login" });
            }
            catch (Exception exception)
            {
                return BadRequest(new { success = false, exception.InnerException });
            }
        }
        /// <summary>
        /// Delete note by noteId
        /// </summary>
        /// <param name="NoteId"></param>
        /// <returns></returns>
        [Authorize]
        [HttpDelete("Delete/{NoteId}")]
        public IActionResult DeleteNote(long NoteId)
        {
            try
            {
                var identity = User.Identity as ClaimsIdentity;
                if (identity != null)
                {
                    IEnumerable<Claim> claims = identity.Claims;
                    long Id = Convert.ToInt64(claims.Where(p => p.Type == "Id").FirstOrDefault()?.Value);
                    string EmailAddress = claims.Where(p => p.Type == "EmailAddress").FirstOrDefault()?.Value;
                    bool result = noteBL.DeleteNote(Id,NoteId).Result;
                    return Ok(new { success = true, user = EmailAddress, Message = "note deleted" });
                }
                return BadRequest(new { success = false, Message = "no user is active please login" });
            }
            catch (Exception exception)
            {
                return BadRequest(new { success = false, exception.Message });
            }
        }
        /// <summary>
        /// archive the notes
        /// </summary>
        /// <returns></returns>
        [Authorize]
        [HttpGet("Archive")]
        public IActionResult GetArchivedNotes()
        {
            try
            {
                var identity = User.Identity as ClaimsIdentity;
                if (identity != null)
                {
                    IEnumerable<Claim> claims = identity.Claims;
                    long Id = Convert.ToInt64(claims.Where(p => p.Type == "Id").FirstOrDefault()?.Value);
                    string EmailAddress = claims.Where(p => p.Type == "EmailAddress").FirstOrDefault()?.Value;
                    var result = noteBL.GetActiveNotes(Id).Result;
                    return Ok(new { success = true, user = EmailAddress, Notes = result });
                }
                return BadRequest(new { success = false, Message = "no user is active please login" });
            }
            catch (Exception exception)
            {
                return BadRequest(new { success = false, exception.InnerException });
            }
        }
        /// <summary>
        /// trash the note
        /// </summary>
        /// <returns></returns>
        [Authorize]
        [HttpGet("Trash")]
        public IActionResult GetTrashNotes()
        {
            try
            {
                var identity = User.Identity as ClaimsIdentity;
                if (identity != null)
                {
                    IEnumerable<Claim> claims = identity.Claims;
                    long Id = Convert.ToInt64(claims.Where(p => p.Type == "Id").FirstOrDefault()?.Value);
                    string EmailAddress = claims.Where(p => p.Type == "EmailAddress").FirstOrDefault()?.Value;

                    var result = noteBL.GetTrashNotes(Id).Result;
                    return Ok(new { success = true, user = EmailAddress, Notes = result });
                }
                return BadRequest(new { success = false, Message = "no user is active please login" });
            }
            catch (Exception exception)
            {
                return BadRequest(new { success = false, exception.Message });
            }
        }
        /// <summary>
        /// Get the reminders notes
        /// </summary>
        /// <returns></returns>
        [Authorize]
        [HttpGet("Reminder")]
        public IActionResult GetReminderNotes()
        {
            try
            {
                var identity = User.Identity as ClaimsIdentity;
                if (identity != null)
                {
                    IEnumerable<Claim> claims = identity.Claims;
                    long Id = Convert.ToInt64(claims.Where(p => p.Type == "Id").FirstOrDefault()?.Value);
                    string EmailAddress = claims.Where(p => p.Type == "EmailAddress").FirstOrDefault()?.Value;

                    var result = noteBL.GetReminderNotes(Id);
                    return Ok(new { success = true, user = EmailAddress, Notes = result });
                }
                return BadRequest(new { success = false, Message = "no user is active please login" });
            }
            catch (Exception exception)
            {
                return BadRequest(new { success = false, exception.Message });
            }
        }
        /// <summary>
        /// Update the notes
        /// </summary>
        /// <param name="note"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPut("Update")]
        public IActionResult UpdateNote(NoteModels note)
        {
            try
            {
                var identity = User.Identity as ClaimsIdentity;
                if (identity != null)
                {
                    IEnumerable<Claim> claims = identity.Claims;
                    long Id = Convert.ToInt64(claims.Where(p => p.Type == "Id").FirstOrDefault()?.Value);
                    note.Id = Id;
                    NoteModels result = noteBL.UpdateNote(note).Result;
                    return Ok(new { success = true, Message = "note updated", Note = result });
                }
                return BadRequest(new { success = false, Message = "no user is active please login" });
            }
            catch (Exception exception)
            {
                return BadRequest(new { success = false, exception.Message });
            }
        }
        /// <summary>
        /// Pin the notes
        /// </summary>
        /// <param name="noteId"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPatch("Pin/{NoteId}")]
        public IActionResult TogglePin(long NoteId)
        {
            try
            {
                var identity = User.Identity as ClaimsIdentity;
                if (identity != null)
                {
                    IEnumerable<Claim> claims = identity.Claims;
                    long Id = Convert.ToInt64(claims.Where(p => p.Type == "Id").FirstOrDefault()?.Value);
                    bool result = noteBL.ToggleNotePin(NoteId, Id).Result;
                    return Ok(new { success = true, Message = "note pin toggled", Note = result });
                }
                return BadRequest(new { success = false, Message = "no user is active please login" });
            }
            catch (Exception exception)
            {
                return BadRequest(new { success = false, exception.Message });
            }
        }
        /// <summary>
        /// archive note by id
        /// </summary>
        /// <param name="noteId"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPatch("Archive/{NoteId}")]
        public IActionResult ToggleArchive(long NoteId)
        {
            try
            {
                var identity = User.Identity as ClaimsIdentity;
                if (identity != null)
                {
                    IEnumerable<Claim> claims = identity.Claims;
                    long Id = Convert.ToInt64(claims.Where(p => p.Type == "Id").FirstOrDefault()?.Value);
                    bool result = noteBL.ToggleArchive(NoteId, Id).Result;
                    return Ok(new { success = true, Message = "note archive toggled", Note = result });
                }
                return BadRequest(new { success = false, Message = "no user is active please login" });
            }
            catch (Exception exception)
            {
                return BadRequest(new { success = false, exception.Message });
            }
        }
        /// <summary>
        /// change the background color of notes
        /// </summary>
        /// <param name="NoteId"></param>
        /// <param name="colorCode"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPatch("Color/{NoteId}/{ColorCode}")]
        public IActionResult ChangeNoteBackgroundColor(long NoteId, string ColorCode)
        {
            try
            {
                var identity = User.Identity as ClaimsIdentity;
                if (identity != null)
                {
                    IEnumerable<Claim> claims = identity.Claims;
                    long Id = Convert.ToInt64(claims.Where(p => p.Type == "Id").FirstOrDefault()?.Value);
                    bool result = noteBL.ChangeBackgroundColor(NoteId, Id, ColorCode).Result;
                    return Ok(new { success = true, Message = "note background color changed", Note = result });
                }
                return BadRequest(new { success = false, Message = "no user is active please login" });
            }
            catch (Exception exception)
            {
                return BadRequest(new { success = false, exception.Message });
            }
        }
        /// <summary>
        /// set the reminder to note
        /// </summary>
        /// <param name="Reminder"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPatch("SetReminder")]
        public IActionResult SetNoteReminder(NoteReminder Reminder)
        {
            try
            {
                var identity = User.Identity as ClaimsIdentity;
                if (identity != null)
                {
                    IEnumerable<Claim> claims = identity.Claims;
                    long Id = Convert.ToInt64(claims.Where(p => p.Type == "Id").FirstOrDefault()?.Value);
                    Reminder.Id = Id;
                    bool result = noteBL.SetNoteReminder(Reminder).Result;
                    return Ok(new { success = true, Message = "note reminder added" });
                }
                return BadRequest(new { success = false, Message = "no user is active please login" });
            }
            catch (Exception exception)
            {
                return BadRequest(new { success = false, exception.Message });
            }
        }
    }
}
