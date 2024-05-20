using BusinessLayer.Interfaces;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;
using ModelLayer.Models;
using RepositoryLayer.Entities;
using RepositoryLayer.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLayer.Services
{
    public class NotesBusiness : INotesBusiness
    {

        private readonly INotesRepo notesRepo;

        public NotesBusiness(INotesRepo notesRepo)
        {
            this.notesRepo = notesRepo;
        }

        public NotesEntity CreateNote(int UserId, NotesModel notesModel)
        {
            return notesRepo.CreateNote(UserId, notesModel);
        }

        public List<NotesEntity> GetAllNotes()
        {
            return notesRepo.GetAllNotes();
        }

        public List<NotesEntity> GetNotesByUser(int UserId)
        {
            return notesRepo.GetNotesByUser(UserId);
        }
        
        public NotesEntity UpdateNote(int UserId, int NotesId, NotesModel notesModel)
        {
            return notesRepo.UpdateNote(UserId, NotesId, notesModel);
        }

        public bool TogglePinNote(int UserId, int NotesId)
        {
            return notesRepo.TogglePinNote(UserId, NotesId);
        }

        public bool ToggleArchiveNote(int UserId, int NotesId)
        {
            return notesRepo.ToggleArchiveNote(UserId, NotesId);
        }

        public bool ToogleTrashNote(int UserId, int NotesId)
        {
            return notesRepo.ToogleTrashNote(UserId, NotesId);
        }

        public string AddBackgroundColorToNote(int UserId, int NotesId, string InputColor)
        {
            return notesRepo.AddBackgroundColorToNote(UserId, NotesId, InputColor);
        }

        public DateTime AddReminderToNote(int UserId, int NotesId, DateTime Reminder)
        {
            return notesRepo.AddReminderToNote(UserId, NotesId, Reminder);
        }
        public string AddImageToNote(string FilePath, int NotesId, int UserId)
        {
            return notesRepo.AddImageToNote(FilePath, NotesId, UserId);
        }
        public ImageUploadResult UploadImage(IFormFile ImagePath, int NotesId, int UserId)
        {
            return notesRepo.UploadImage(ImagePath, NotesId, UserId);
        }


        public List<UserEntity> GetUsersByNoteTitle(string NotesTitle)
        {
            return notesRepo.GetUsersByNoteTitle(NotesTitle);
        }

        public object GetUsersWithNotes()
        {
            return notesRepo.GetUsersWithNotes();
        }

        //
        public object GetUsersWithNotesCount()
        {
            return notesRepo.GetUsersWithNotesCount();
        }
        public object GetNotesByTitleAndDescription(string Title, string Description)
        {
            return notesRepo.GetNotesByTitleAndDescription(Title, Description);
        }
    }
}
