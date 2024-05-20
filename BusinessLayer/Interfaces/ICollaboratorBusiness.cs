using RepositoryLayer.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLayer.Interfaces
{
    public interface ICollaboratorBusiness
    {
        string AddCollaborator(string Email, int NotesId, int UserId);
        bool DeleteCollaborator(string Email, int NotesId, int UserId);

        List<CollaboratorEntity> GetCollaboratorsByNoteId(int NotesId, int UserId);
        object GetUsersWithNotesAndCollaborators();
    }
}
