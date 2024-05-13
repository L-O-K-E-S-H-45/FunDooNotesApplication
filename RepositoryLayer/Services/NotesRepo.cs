using ModelLayer.Models;
using RepositoryLayer.Context;
using RepositoryLayer.Entities;
using RepositoryLayer.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RepositoryLayer.Services
{
    public class NotesRepo : INotesRepo
    {
        private readonly FunDooDBContext funDooDBContext;
        public NotesRepo(FunDooDBContext funDooDBContext)
        {
            this.funDooDBContext = funDooDBContext;   
        }
        public NotesEntity CreateNote(int UserId, NotesModel notesModel)
        {
            NotesEntity notesEntity = new NotesEntity();
            notesEntity.Title = notesModel.Title;
            notesEntity.Description = notesModel.Description;
            notesEntity.CreatedAt = DateTime.Now;
            notesEntity.UpdatedAt = DateTime.Now;
            notesEntity.UserId = UserId;

            funDooDBContext.Notes.Add(notesEntity);
            funDooDBContext.SaveChanges();

            return notesEntity;
        }

        public List<NotesEntity> GetAllNotes()
        {
            var notes = funDooDBContext.Notes.ToList();
            return notes.Any() ? notes : throw new Exception("Notes list is empty");
        }

        public List<NotesEntity> GetNotes(int UserId)
        {
            var notes = funDooDBContext.Notes.ToList().FindAll(n  => n.UserId == UserId);
            return notes.Any() ? notes : throw new Exception("Notes list is empty for user id: " + UserId);
        }

        public NotesEntity UpdateNote(int UserId, int NotesId, NotesModel notesModel)
        {
            var note = funDooDBContext.Notes.FirstOrDefault(n => n.UserId == UserId && n.NotesId == NotesId);
            if (note != null)
            {
                note.Title = notesModel.Title;
                note.Description = notesModel.Description;
                note.UpdatedAt = DateTime.Now;

                funDooDBContext.SaveChanges();
                return note;
            }
            else
                throw new Exception("Note not found for requested notes id: " + NotesId);
        }
    }
}
