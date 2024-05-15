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

    }
}
