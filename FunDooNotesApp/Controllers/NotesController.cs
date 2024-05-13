using BusinessLayer.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ModelLayer.Models;
using RepositoryLayer.Entities;
using RepositoryLayer.Migrations;
using System;
using System.Collections.Generic;

namespace FunDooNotesApplication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotesController : ControllerBase
    {

        private readonly INotesBusiness notesBusiness;

        public NotesController(INotesBusiness notesBusiness)
        {
            this.notesBusiness = notesBusiness;
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
        public IActionResult GetNotes()
        {
            try
            {
                int userId = int.Parse(User.FindFirst("UserId").Value);
                var response = notesBusiness.GetNotes(userId);
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


    }
}
