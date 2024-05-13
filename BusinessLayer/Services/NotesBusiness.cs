using BusinessLayer.Interfaces;
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

        public List<NotesEntity> GetNotes(int UserId)
        {
            return notesRepo.GetNotes(UserId);
        }

        public NotesEntity UpdateNote(int UserId, int NotesId, NotesModel notesModel)
        {
            return notesRepo.UpdateNote(UserId, NotesId, notesModel);
        }
    }
}
