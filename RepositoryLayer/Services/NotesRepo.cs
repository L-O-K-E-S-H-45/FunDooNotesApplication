using ModelLayer.Models;
using RepositoryLayer.Context;
using RepositoryLayer.Entities;
using RepositoryLayer.Enums;
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

        public List<NotesEntity> GetNotesByUser(int UserId)
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

        //------------------------------------------

        public NotesEntity GetNoteByIds(int UserId, int NotesId)
        {
            return funDooDBContext.Notes.FirstOrDefault(notes => notes.NotesId == NotesId && notes.UserId == UserId);
        }

        public bool TogglePinNote(int UserId, int NotesId)
        {
            var note = GetNoteByIds(UserId, NotesId);
            if (note != null)
            {
                note.IsPin = !note.IsPin;
                note.UpdatedAt = DateTime.Now;
                funDooDBContext.SaveChanges();
                return true;
            }
            else
                throw new Exception("Note not found for requested id: " + NotesId);
        }

        public bool ToggleArchiveNote(int UserId, int NotesId)
        {
            var note = GetNoteByIds(UserId, NotesId);
            if (note != null)
            {
                if (note.IsPin) note.IsPin = false;
                note.IsArchive = !note.IsArchive;
                note.UpdatedAt = DateTime.Now;
                funDooDBContext.SaveChanges();
                return true;
            }
            else
                throw new Exception("Note not found for requested id: " + NotesId);
        }
        public bool ToogleTrashNote(int UserId, int NotesId)
        {
            var note = GetNoteByIds(UserId, NotesId);
            if (note != null)
            {
                if (note.IsPin) note.IsPin = false;
                note.IsTrash = !note.IsTrash;
                note.UpdatedAt = DateTime.Now;
                funDooDBContext.SaveChanges();
                return true;
            }
            else
                throw new Exception("Note not found for requested id: " + NotesId);
        }

        public string AddBackgroundColorToNote(int UserId, int NotesId, string InputColor)
        {
            var note = GetNoteByIds(UserId, NotesId);
            if (note != null)
            {
                Colors color;
                if (Enum.TryParse(InputColor, true, out color))
                {
                    note.Color = color.ToString();
                    note.UpdatedAt = DateTime.Now;
                    funDooDBContext.SaveChanges();
                    return InputColor;
                }
                else
                    throw new Exception("Invalid input color: " + InputColor);
            }
            else
                throw new Exception("Note not found for requested id: " + NotesId);
        }

        public bool AddImageToNote(int UserId, int NotesId, string ImagePath)
        {
            var note = GetNoteByIds(UserId, NotesId);
            if (note != null)
            {
                note.Image = ImagePath;
                note.UpdatedAt = DateTime.Now;
                funDooDBContext.SaveChanges();
                return true;
            }
            else
                throw new Exception("Note not found for requested id: " + NotesId);
        }

        public DateTime AddReminderToNote(int UserId, int NotesId, DateTime Reminder)
        {
            var note = GetNoteByIds(UserId, NotesId);
            if (note != null)
            {
                if (Reminder > DateTime.Now)
                {
                    note.Reminder = Reminder;
                    note.UpdatedAt = DateTime.Now;
                    funDooDBContext.SaveChanges();
                    return Reminder;
                }
                else
                    throw new Exception("Invalid reminder input: " + Reminder);
            }
            else
                throw new Exception("Note not found for requested id: " + NotesId);
        }


    }
}
