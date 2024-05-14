﻿using ModelLayer.Models;
using RepositoryLayer.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace RepositoryLayer.Interfaces
{
    public interface INotesRepo
    {

        NotesEntity CreateNote(int UserId, NotesModel notesModel);
        List<NotesEntity> GetAllNotes();
        List<NotesEntity> GetNotes(int UserId);
        NotesEntity UpdateNote(int UserId,int NotesId, NotesModel notesModel);

        bool TogglePinNote(int NotesId);
        bool ToggleArchiveNote(int NotesId);
        bool ToogleTrashNote(int NotesId);


    }
}
