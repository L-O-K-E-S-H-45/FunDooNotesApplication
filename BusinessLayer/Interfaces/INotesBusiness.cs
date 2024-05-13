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
        List<NotesEntity> GetNotes(int UserId);
        NotesEntity UpdateNote(int UserId, int NotesId, NotesModel notesModel);
    }
}
