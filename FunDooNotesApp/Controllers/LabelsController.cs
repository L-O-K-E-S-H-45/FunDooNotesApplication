using BusinessLayer.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ModelLayer.Models;
using RepositoryLayer.Entities;
using RepositoryLayer.Migrations;
using System;
using System.Collections.Generic;
using System.Security.Claims;

namespace FunDooNotesApplication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LabelsController : ControllerBase
    {
        private readonly ILabelBusiness labelBusiness;
        public LabelsController(ILabelBusiness labelBusiness)
        {
            this.labelBusiness = labelBusiness;
        }

        [Authorize]
        [HttpPost("AddLabel")]
        public IActionResult AddLabelToNote(int noteId, string labelName)
        {
            try
            {
                int userId = int.Parse(User.FindFirstValue("UserId"));
                var response = labelBusiness.AddLabelToNote(userId, noteId, labelName);
                return Ok(new ResponseModel<string> {IsSuccess = true, Message = "Label added successfully to note id: " + noteId, Data = labelName + " added" });
            }
            catch (Exception ex)
            {
                return BadRequest(new ResponseModel<string> { IsSuccess = false, Message = "Failed to add label", Data = ex.Message });
            }
        }

        [Authorize]
        [HttpGet("User")]
        public IActionResult GetLabelsByUser()
        {
            try
            {
                int userId = int.Parse(User.FindFirstValue("UserId"));
                var response = labelBusiness.GetLabelsByUser(userId);
                return Ok(new ResponseModel<List<LabelEntity>> { IsSuccess = true, Message = "Labels found successfully for user id: " + userId, Data = response });
            }
            catch (Exception ex)
            {
                return BadRequest(new ResponseModel<string> { IsSuccess = false, Message = "Failed to fetch labels", Data = ex.Message });
            }
        }

        [Authorize]
        [HttpGet("notes/{LabelName}")]
        public IActionResult GetNotesByLabelName(string LabelName)
        {
            try
            {
                int userId = int.Parse(User.FindFirstValue("UserId"));
                var response = labelBusiness.GetNotesByLabelName(userId, LabelName);
                return Ok(new ResponseModel<List<NotesEntity>> { IsSuccess = true, Message = "Notes found successfully for label name: " + LabelName, Data = response });
            }
            catch (Exception ex)
            {
                return BadRequest(new ResponseModel<string> { IsSuccess = false, Message = "Failed to fetch notes", Data = ex.Message });
            }
        }

        [Authorize]
        [HttpPatch("RenameLabel")]
        public IActionResult RenameLabel(int NotesId, string OldLabelName, string NewLabelName)
        {
            try
            {
                int userId = int.Parse(User.FindFirstValue("UserId"));
                var response = labelBusiness.RenameLabel(userId, NotesId, OldLabelName, NewLabelName);
                return Ok(new ResponseModel<string> { IsSuccess = true, Message = "Label rename successfully for notes id: " + NotesId, Data = OldLabelName + " renamed to " + response });
            }
            catch (Exception ex)
            {
                return BadRequest(new ResponseModel<string> { IsSuccess = false, Message = "Failed to rename Label", Data = ex.Message });
            }
        }

        [Authorize]
        [HttpDelete("label/{NotesId}/{LabelName}")]
        public IActionResult DeleteLabel(int NotesId, string LabelName)
        {
            try
            {
                int userId = int.Parse(User.FindFirstValue("UserId"));
                var response = labelBusiness.DeleteLabel(userId, NotesId, LabelName);
                return Ok(new ResponseModel<string> { IsSuccess = true, Message = "Label deleted successfully for notes id: " + NotesId, Data = response + " deleted" });
            }
            catch (Exception ex)
            {
                return BadRequest(new ResponseModel<string> { IsSuccess = false, Message = "Failed to delete Label", Data = ex.Message });
            }
        }


    }
}
