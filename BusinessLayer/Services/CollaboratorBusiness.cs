using BusinessLayer.Interfaces;
using RepositoryLayer.Entities;
using RepositoryLayer.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLayer.Services
{
    public class CollaboratorBusiness : ICollaboratorBusiness
    {
        private readonly ICollaboratorRepo collaboratorRepo;

        public CollaboratorBusiness(ICollaboratorRepo collaboratorRepo)
        {
            this.collaboratorRepo = collaboratorRepo;
        }

        public string AddCollaborator(string Email, int NotesId, int UserId)
        {
            return collaboratorRepo.AddCollaborator(Email, NotesId, UserId);
        }

        public bool DeleteCollaborator(string Email, int NotesId, int UserId)
        {
            return collaboratorRepo.DeleteCollaborator(Email, NotesId, UserId);
        }

        public List<CollaboratorEntity> GetCollaboratorsByNoteId(int NotesId, int UserId)
        {
            return collaboratorRepo.GetCollaboratorsByNoteId(NotesId, UserId);
        }

        public object GetUsersWithNotesAndCollaborators()
        {
            return collaboratorRepo.GetUsersWithNotesAndCollaborators();
        }
    }
}
