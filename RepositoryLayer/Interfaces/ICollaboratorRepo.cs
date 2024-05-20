using RepositoryLayer.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace RepositoryLayer.Interfaces
{
    public interface ICollaboratorRepo
    {
        public string AddCollaborator(string Email, int NotesId, int UserId);
        public bool DeleteCollaborator(string Email, int NotesId, int UserId);

        List<CollaboratorEntity> GetCollaboratorsByNoteId(int NotesId, int UserId);
        object GetUsersWithNotesAndCollaborators();
    }
}
