using ModelLayer.Models;
using RepositoryLayer.Context;
using RepositoryLayer.Entities;
using RepositoryLayer.Interfaces;
using RepositoryLayer.Migrations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RepositoryLayer.Services
{
    public class CollaboratorRepo : ICollaboratorRepo
    {
        private readonly FunDooDBContext funDooDBContext;
        private readonly INotesRepo notesRepo;
        public CollaboratorRepo(FunDooDBContext funDooDBContext, INotesRepo notesRepo)
        {
            this.funDooDBContext = funDooDBContext;
            this.notesRepo = notesRepo;

        }

        public NotesEntity GetNoteByIds(int NotesId, int UserId)
        {
            return funDooDBContext.Notes.FirstOrDefault(notes => notes.NotesId == NotesId && notes.UserId == UserId);
        }

        private CollaboratorEntity GetCollaborator(string Email, int NotesId, int UserId)
        {
            return funDooDBContext.Collaborators.FirstOrDefault(c => c.UserId == UserId && c.NotesId == NotesId && c.Email == Email);
        }

        public string AddCollaborator(string Email, int NotesId, int UserId)
        {
            // check collaborator exist or not
            var CollaboratingUser = funDooDBContext.Users.FirstOrDefault(u => u.Email ==  Email);
            if (CollaboratingUser != null)
            {
                // get note
                var MyNote = GetNoteByIds(NotesId, UserId);
                if (MyNote != null)
                {
                    // check collaborator already exist or not
                    var collaborator = GetCollaborator(Email, NotesId, UserId);
                    if (collaborator == null)
                    {
                        CollaboratorEntity collaboratorEntity = new CollaboratorEntity();
                        collaboratorEntity.Email = Email;
                        collaboratorEntity.NotesId = NotesId;
                        collaboratorEntity.UserId = UserId;

                        // adding collaborator 
                        funDooDBContext.Collaborators.Add(collaboratorEntity);
                        funDooDBContext.SaveChanges();

                        // create & provide note for CollaboratingUser
                        // 1st way 
                        NotesEntity notesEntity = new NotesEntity()
                        {
                            UserId = CollaboratingUser.UserId,
                            Title = MyNote.Title,
                            Description = MyNote.Description,
                            Image = MyNote.Image,
                            CreatedAt = DateTime.Now,
                            UpdatedAt = DateTime.Now
                        };
                        funDooDBContext.Notes.Add(notesEntity);
                        funDooDBContext.SaveChanges();
                        // 2nd way
                        //NotesModel notesModel = new NotesModel() {Title = MyNote.Title, Description = MyNote.Description };
                        //notesRepo.CreateNote(CollaboratingUser.UserId, notesModel);

                        return "Collaborator added and also note created & privided for Collaborating User: " + Email;
                    }
                    else
                        throw new Exception("This " + Email + " email already added as collaborator");
                }
                else
                    throw new Exception("Note not found requested note id: " + NotesId);
            }
            else
                throw new Exception("User not found collaborator email: " + Email);
        }

        public bool DeleteCollaborator(string Email, int NotesId, int UserId)
        {
            var collaborator = GetCollaborator(Email, NotesId, UserId);
            if ( collaborator != null)
            {
                funDooDBContext.Remove(collaborator);
                funDooDBContext.SaveChanges();
                return true;
            }
            else
                throw new Exception("Collaborator not found for requested email: " + Email + " & note id: " + NotesId);
        }

        public List<CollaboratorEntity> GetCollaboratorsByNoteId(int NotesId, int UserId)
        {
            var collaborators = (from c in funDooDBContext.Collaborators 
                                 where c.NotesId == NotesId && c.UserId == UserId
                                 orderby UserId, NotesId
                                 select c).ToList();
            return collaborators.Any() ? collaborators : throw new Exception("Collaborators are not added for note id: " + NotesId);
        }

        public object GetUsersWithNotesAndCollaborators()
        {
            var users = from UserEntity u in funDooDBContext.Users
                        join NotesEntity n in funDooDBContext.Notes on u.UserId equals n.UserId
                        join LabelEntity l in funDooDBContext.Labels on n.NotesId equals l.NotesId
                        join CollaboratorEntity c in funDooDBContext.Collaborators on n.NotesId equals c.NotesId
                        orderby u.UserId, n.NotesId, l.LabelName
                        select new
                        {
                            u.UserId, u.FirstName, u.LastName, UserEmail = u.Email,
                            n.NotesId, n.Title, n.Description, n.Reminder,
                            l.LabelId, l.LabelName,
                            c.CollaboratorId, ColaboratorEmail = c.Email
                        };
            return users.Any() ? users : throw new Exception("Notes are not added for users: ");
        }
    }
}
