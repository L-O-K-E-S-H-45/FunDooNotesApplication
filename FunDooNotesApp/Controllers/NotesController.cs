using BusinessLayer.Interfaces;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using ModelLayer.Models;
using Newtonsoft.Json;
using RepositoryLayer.Context;
using RepositoryLayer.Entities;
using RepositoryLayer.Migrations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FunDooNotesApplication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotesController : ControllerBase
    {

        private readonly INotesBusiness notesBusiness;
        private readonly IDistributedCache distributedCache;
        private readonly FunDooDBContext funDooDBContext;

        public NotesController(INotesBusiness notesBusiness, IDistributedCache distributedCache, FunDooDBContext funDooDBContext)
        {
            this.notesBusiness = notesBusiness;
            this.distributedCache = distributedCache;
            this.funDooDBContext = funDooDBContext;
        }

        [Authorize]
        [HttpPost]
        [Route("CreateNote")]
        public ActionResult CreateNote(NotesModel notesModel)
        {
            try
            {
                int UserId = int.Parse(User.FindFirst("UserId").Value);
                NotesEntity notesEntity = notesBusiness.CreateNote(UserId, notesModel);
                if (notesEntity != null)
                    return Ok(new ResponseModel<NotesEntity> { IsSuccess = true, Message = "Note added successfully", Data = notesEntity});
                else
                    return BadRequest(new ResponseModel<string> { IsSuccess = false, Message = "Note did not added", Data = "Failed to add note" });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpGet("")]
        public IActionResult GetAllNotes()
        {
            try
            {
                var response = notesBusiness.GetAllNotes();
                return Ok(new ResponseModel<List<NotesEntity>> { IsSuccess = true, Message = "Notes found successfully", Data = response });
            }
            catch (Exception ex)
            {
                return BadRequest(new ResponseModel<string> { IsSuccess = false, Message = "Notes not found", Data = ex.Message });
            }
        }

        [Authorize]
        [HttpGet("user")]
        public IActionResult GetNotesByUser()
        {
            try
            {
                int userId = int.Parse(User.FindFirst("UserId").Value);
                var response = notesBusiness.GetNotesByUser(userId);
                return Ok(new ResponseModel<List<NotesEntity>> { IsSuccess = true, Message = "Notes successfully found for user id: " + userId, Data = response });
            }
            catch (Exception ex)
            {
                return BadRequest(new ResponseModel<string> { IsSuccess = false, Message = "Notes not found", Data = ex.Message });
            }
        }

        [Authorize]
        [HttpPut("{NotesId}")]
        public IActionResult UpdateNote(int NotesId, NotesModel notesModel)
        {
            try
            {
                int userId = int.Parse(User.FindFirst("UserId").Value);
                var response = notesBusiness.UpdateNote(userId, NotesId, notesModel);
                return Ok(new ResponseModel<NotesEntity> { IsSuccess = true, Message = "Note successfully updated for user id: " + userId, Data = response });
            }
            catch (Exception ex)
            {
                return BadRequest(new ResponseModel<string> { IsSuccess = false, Message = "Failed to update note", Data = ex.Message });
            }
        }

        [Authorize]
        [HttpPut("TogglePin")]
        public IActionResult TogglePinNote(int notesId)
        {
            try
            {
                int userId = int.Parse(User.FindFirst("UserId").Value);
                var response = notesBusiness.TogglePinNote(userId, notesId);
                return Ok(new ResponseModel<string> { IsSuccess = true, Message = "successfully toggled pin for note id: " + notesId, Data = "Toggled pin note" });
            }
            catch (Exception ex)
            {
                return BadRequest(new ResponseModel<string> { IsSuccess = false, Message = "Failed to toggle pin note", Data = ex.Message });
            }
        }

        [Authorize]
        [HttpPut("ToggleArchive")]
        public IActionResult ToggleArchiveNote(int notesId)
        {
            try
            {
                int userId = int.Parse(User.FindFirst("UserId").Value);
                var response = notesBusiness.ToggleArchiveNote(userId, notesId);
                return Ok(new ResponseModel<string> { IsSuccess = true, Message = "successfully toggled archive for note id: " + notesId, Data = "Toggled archive note" });
            }
            catch (Exception ex)
            {
                return BadRequest(new ResponseModel<string> { IsSuccess = false, Message = "Failed to toggle archive note", Data = ex.Message });
            }
        }

        [Authorize]
        [HttpPut("ToggleTrash")]
        public IActionResult ToggleTrashNote(int notesId)
        {
            try
            {
                int userId = int.Parse(User.FindFirst("UserId").Value);
                var response = notesBusiness.ToogleTrashNote(userId, notesId);
                return Ok(new ResponseModel<string> { IsSuccess = true, Message = "successfully toggled trash for note id: " + notesId, Data = "Toggled trash note" });
            }
            catch (Exception ex)
            {
                return BadRequest(new ResponseModel<string> { IsSuccess = false, Message = "Failed to toggle trash note", Data = ex.Message });
            }
        }

        [Authorize]
        [HttpPut("AddColor")]
        public IActionResult AddBackgroundColorToNote(int notesId, string colorName)
        {
            try
            {
                int userId = int.Parse(User.FindFirst("UserId").Value);
                var response = notesBusiness.AddBackgroundColorToNote(userId, notesId, colorName);
                return Ok(new ResponseModel<string> { IsSuccess = true, Message = "successfully added color to note id: " + notesId, Data = "Added color is: " + response });
            }
            catch (Exception ex)
            {
                return BadRequest(new ResponseModel<string> { IsSuccess = false, Message = "Failed to add color to note", Data = ex.Message });
            }
        }

        [Authorize]
        [HttpPut("AddReminder")]
        public IActionResult AddReminderToNote(int notesId, DateTime Reminder)
        {
            try
            {
                int userId = int.Parse(User.FindFirst("UserId").Value);
                var response = notesBusiness.AddReminderToNote(userId, notesId, Reminder);
                return Ok(new ResponseModel<string> { IsSuccess = true, Message = "successfully added reminder to note id: " + notesId, Data = "Added reminder at: " + response });
            }
            catch (Exception ex)
            {
                return BadRequest(new ResponseModel<string> { IsSuccess = false, Message = "Failed to add reminder to note", Data = ex.Message });
            }
        }

        [Authorize]
        [HttpPut("AddImage")]
        public IActionResult AddImageToNote(string FilePath, int NotesId)
        {
            try
            {
                int userId = int.Parse(User.FindFirst("UserId").Value);
                var response = notesBusiness.AddImageToNote(FilePath, NotesId, userId);
                return Ok(new ResponseModel<string> { IsSuccess = true, Message = "successfully added image to note id: " + NotesId, Data = "Added Image is: " + response });
            }
            catch (Exception ex)
            {
                return BadRequest(new ResponseModel<string> { IsSuccess = false, Message = "Failed to add image to note", Data = ex.Message });
            }
        }

        [Authorize]
        [HttpPut("UploadImage")]
        public IActionResult UploadImage(IFormFile ImagePath, int NotesId)
        {
            try
            {
                int userId = int.Parse(User.FindFirst("UserId").Value);
                var response = notesBusiness.UploadImage(ImagePath, NotesId, userId);
                return Ok(new ResponseModel<string> { IsSuccess = true, Message = "successfully added image to note id: " + NotesId, Data = "Added Image is: " + response });
            }
            catch (Exception ex)
            {
                return BadRequest(new ResponseModel<string> { IsSuccess = false, Message = "Failed to add image to note", Data = ex.Message });
            }
        }


        [HttpGet("{NotesTitle}/users")]
        public IActionResult GetUsersByNoteTitle(string NotesTitle)
        {
            try
            {
                var response = notesBusiness.GetUsersByNoteTitle(NotesTitle);
                return Ok(new ResponseModel<List<UserEntity>> { IsSuccess = true, Message = "successfully found users", Data = response });
            }
            catch (Exception ex)
            {
                return BadRequest(new ResponseModel<string> { IsSuccess = false, Message = "Failed to find users", Data = ex.Message });
            }
        }

        [HttpGet("users")]
        public IActionResult GetUsersWithNotes()
        {
            try
            {
                var response = notesBusiness.GetUsersWithNotes();
                return Ok(new ResponseModel<object> { IsSuccess = true, Message = "successfully found users with notes", Data = response });
            }
            catch (Exception ex)
            {
                return BadRequest(new ResponseModel<string> { IsSuccess = false, Message = "Failed to find users with notes", Data = ex.Message });
            }
        }

        [HttpGet("Redis")]
        public async Task<IActionResult> GetAllNotesUsingRedisCache()
        {
            var cacheKey = "NotesList";
            string SerializedNotesLst;
            var NotesList = new List<NotesEntity>();
            var RedisNotesList = await distributedCache.GetAsync(cacheKey);
            if (RedisNotesList != null)
            {
                SerializedNotesLst = Encoding.UTF8.GetString(RedisNotesList);
                NotesList = JsonConvert.DeserializeObject<List<NotesEntity>>(SerializedNotesLst);
            }
            else
            {
                NotesList = funDooDBContext.Notes.ToList();
                SerializedNotesLst = JsonConvert.SerializeObject(NotesList);
                RedisNotesList = Encoding.UTF8.GetBytes(SerializedNotesLst);
                var options = new DistributedCacheEntryOptions()
                    .SetAbsoluteExpiration(DateTime.Now.AddMinutes(20))
                    .SetSlidingExpiration(TimeSpan.FromMinutes(5));
                await distributedCache.SetAsync(cacheKey,RedisNotesList, options);
            }
            return Ok(NotesList);
        }

        // 
        [HttpGet("NotesCount")]
        public IActionResult GetUsersWithNotesCount()
        {
            try
            {
                var response = notesBusiness.GetUsersWithNotesCount();
                return Ok(new ResponseModel<object> { IsSuccess = true, Message = "successfully found notes count for users", Data = response });
            }
            catch (Exception ex)
            {
                return BadRequest(new ResponseModel<string> { IsSuccess = false, Message = "Failed to find notes count for users", Data = ex.Message });
            }
        }

        [HttpGet("NotesWithTitleAndDesc")]
        public IActionResult GetNotesByTitleAndDescription(string Title, string Description)
        {
            try
            {
                var response = notesBusiness.GetNotesByTitleAndDescription(Title, Description);
                return Ok(new ResponseModel<object> { IsSuccess = true, Message = "successfully found notes", Data = response });
            }
            catch (Exception ex)
            {
                return BadRequest(new ResponseModel<string> { IsSuccess = false, Message = "Failed to find notes", Data = ex.Message });
            }
        }


    }
}
