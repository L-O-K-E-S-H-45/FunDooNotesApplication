using BusinessLayer.Interfaces;
using BusinessLayer.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ModelLayer.Models;
using RepositoryLayer.Entities;
using System;
using System.Collections.Generic;
using System.Security.Claims;

namespace FunDooNotesApplication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CollaboratorsController : ControllerBase
    {

        private readonly ICollaboratorBusiness collaboratorBusiness;

        public CollaboratorsController(ICollaboratorBusiness collaboratorBusiness)
        {
            this.collaboratorBusiness = collaboratorBusiness;
        }

        [Authorize]
        [HttpPost("AddCollaborator")]
        public IActionResult AddCollaborator(string Email, int NotesId)
        {
            try
            {
                int UserId = int.Parse(User.FindFirstValue("UserId"));
                var response = collaboratorBusiness.AddCollaborator(Email, NotesId, UserId);
                return Ok(new ResponseModel<string> { IsSuccess = true, Message = "Collaborator added successfully to note id: " + NotesId, Data = response });
            }
            catch (Exception ex)
            {
                return BadRequest(new ResponseModel<string> { IsSuccess = false, Message = "Failed to add collaborator", Data = ex.Message });
            }
        }

        [Authorize]
        [HttpDelete("{Email}/{NotesId}")]
        public IActionResult DeleteCollaborator(string Email, int NotesId)
        {
            try
            {
                int userId = int.Parse(User.FindFirstValue("UserId"));
                var response = collaboratorBusiness.DeleteCollaborator(Email, NotesId, userId);
                return Ok(new ResponseModel<string> { IsSuccess = true, Message = "Collaborator deleted successfully for notes id: " + NotesId, Data = Email + " deleted" });
            }
            catch (Exception ex)
            {
                return BadRequest(new ResponseModel<string> { IsSuccess = false, Message = "Failed to delete collaborator", Data = ex.Message });
            }
        }


        [Authorize]
        [HttpGet("{NotesId}")]
        public IActionResult GetCollaboratorsByNoteId(int NotesId)
        {
            try
            {
                int userId = int.Parse(User.FindFirstValue("UserId"));
                var response = collaboratorBusiness.GetCollaboratorsByNoteId(NotesId, userId);
                return Ok(new ResponseModel<List<CollaboratorEntity>> { IsSuccess = true, Message = "Collaborators successfully found for note id: " + NotesId, Data = response });
            }
            catch (Exception ex)
            {
                return BadRequest(new ResponseModel<string> { IsSuccess = false, Message = "Failed to find collaborators  for note id: " + NotesId, Data = ex.Message });
            }
        }


        [HttpGet("Labels/Notes/Users")]
        public object GetUsersWithNotesAndCollaborators()
        {
            try
            {
                var response = collaboratorBusiness.GetUsersWithNotesAndCollaborators();
                return Ok(new ResponseModel<object> { IsSuccess = true, Message = "users details successfully found", Data = response });
            }
            catch (Exception ex)
            {
                return BadRequest(new ResponseModel<string> { IsSuccess = false, Message = "Failed to find users details", Data = ex.Message });
            }
        }

    }
}
