using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;
using ModelLayer.Models;
using RepositoryLayer.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLayer.Interfaces
{
    public interface INotesBusiness
    {
        NotesEntity CreateNote(int UserId, NotesModel notesModel);
        List<NotesEntity> GetAllNotes();
        List<NotesEntity> GetNotesByUser(int UserId);
        NotesEntity UpdateNote(int UserId, int NotesId, NotesModel notesModel);
        bool TogglePinNote(int UserId, int NotesId);
        bool ToggleArchiveNote(int UserId, int NotesId);
        bool ToogleTrashNote(int UserId, int NotesId);

        string AddBackgroundColorToNote(int UserId, int NotesId, string InputColor);
        DateTime AddReminderToNote(int UserId, int NotesId, DateTime Reminder);
        string AddImageToNote(string FilePath, int NotesId, int UserId);
        ImageUploadResult UploadImage(IFormFile ImagePath, int NotesId, int UserId);

        List<UserEntity> GetUsersByNoteTitle(string NotesTitle);
        object GetUsersWithNotes();


        //
        public object GetUsersWithNotesCount();
        public object GetNotesByTitleAndDescription(string Title, string Description);

    }
}
