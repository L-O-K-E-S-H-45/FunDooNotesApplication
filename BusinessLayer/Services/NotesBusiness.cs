﻿using BusinessLayer.Interfaces;
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
    }
}
