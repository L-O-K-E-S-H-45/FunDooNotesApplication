using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
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
        private readonly IConfiguration configuration;

        public NotesRepo(FunDooDBContext funDooDBContext, IConfiguration configuration)
        {
            this.funDooDBContext = funDooDBContext;
            this.configuration = configuration;
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
            var notes = funDooDBContext.Notes.ToList().FindAll(n => n.UserId == UserId);
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

        public string AddImageToNote(string FilePath, int NotesId, int UserId)
        {
            try
            {
                var user = funDooDBContext.Users.Any(u => u.UserId == UserId);
                if (user)
                {
                    var note = GetNoteByIds(UserId, NotesId);
                    if (note != null)
                    {
                        Account account = new Account("dsuqerssy", "843783226137291", "AydGzGpLedkMTr1xTlZVT-LBL4o");
                        Cloudinary cloudinary = new Cloudinary(account);
                        ImageUploadParams imageUploadParams = new ImageUploadParams()
                        {
                            File = new FileDescription(FilePath),
                            PublicId = note.Title
                        };

                        ImageUploadResult imageUploadResult = cloudinary.Upload(imageUploadParams);

                        note.UpdatedAt = DateTime.Now;
                        note.Image = imageUploadResult.Url.ToString();
                        funDooDBContext.SaveChanges();
                        return FilePath;
                    }
                    else throw new Exception("Note not found for requested note id: " + NotesId);
                }
                else throw new Exception("User not found for requested user id: " + UserId);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            //var note = GetNoteByIds(UserId, NotesId);
            //if (note != null)
            //{
            //    note.Image = ImagePath;
            //    note.UpdatedAt = DateTime.Now;
            //    funDooDBContext.SaveChanges();
            //    return ImagePath;
            //}
            //else
            //    throw new Exception("Note not found for requested id: " + NotesId);
        }
        public ImageUploadResult UploadImage(IFormFile ImagePath, int NotesId, int UserId)
        {
            try
            {
                var user = funDooDBContext.Users.Any(u => u.UserId == UserId);
                if (user)
                {
                    var note = GetNoteByIds(UserId, NotesId);
                    if (note != null)
                    {
                        Account account = new Account(configuration["Cloudinary:CloudName"], configuration["Cloudinary:APIKey"], configuration["Cloudinary:APISecret"]);
                        Cloudinary cloudinary = new Cloudinary(account);
                        var uploadParams = new ImageUploadParams()
                        {
                            File = new FileDescription(ImagePath.FileName, ImagePath.OpenReadStream()),
                            PublicId = note.Title
                        };
                        var uploadImages = cloudinary.Upload(uploadParams);
                        if (uploadImages != null)
                        {
                            note.UpdatedAt = DateTime.Now;
                            note.Image = uploadImages.Url.ToString();
                            funDooDBContext.SaveChanges();
                            return uploadImages;
                        }
                        else throw new Exception("Image not uploaded for note id: " + NotesId);
                    }
                    else throw new Exception("Note not found for requested note id: " + NotesId);
                }
                else throw new Exception("User not found for requested user id: " + UserId);
            }
            catch (Exception ex)
            {
                throw ex;
            }
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

        public List<UserEntity> GetUsersByNoteTitle(string NotesTitle)
        {
            var users = (from n in funDooDBContext.Notes where n.Title == NotesTitle select n.NotesUser).ToList();
            return users.Any() ? users : throw new Exception("Users not found for requested title: " + NotesTitle);
        }

        public object GetUsersWithNotes()
        {
            var users = from UserEntity u in funDooDBContext.Users
                        join NotesEntity n in funDooDBContext.Notes on u.UserId equals n.UserId
                        orderby u.UserId, n.NotesId
                        select new
                        {
                            UserId = u.UserId,
                            FirstName = u.FirstName,
                            LastName = u.LastName,
                            Email = u.Email,
                            NotesId = n.NotesId,
                            NoteTitle = n.Title,
                            NoteDesc = n.Description
                        };

            //var users = from u in funDooDBContext.Users
            //         join n in funDooDBContext.Notes on u.UserId equals n.UserId
            //         //orderby u.UserId, n.NotesId
            //         into UsersList
            //         select new
            //         {

            //             UsersList
            //         };

            return users.Any() ? users : throw new Exception("Notes are not created");
        }

        // 8th Review Task -> 1st Problem

        public object GetUsersWithNotesCount()
        {
            var users = from note in funDooDBContext.Notes.ToList()
                         group note by note.UserId
                         into notesList
                         select new
                         {
                             UserId = notesList.Key,
                             Username = (from user in funDooDBContext.Users where user.UserId == notesList.Key select user.FirstName+" "+user.LastName).FirstOrDefault(),
                             Notescount = notesList.Count()
                         };

            return (users != null) ? users : throw new Exception("Notes are not present");
        }

        public object GetNotesByTitleAndDescription(string Title, string Description)
        {
            var notes = (from n in funDooDBContext.Notes
                         where n.Title == Title && n.Description == Description
                         select n).ToList();
            if (notes.Any())
            {
                return notes;
            }
            else
                throw new Exception("Notes not found for title & description");

        }


    }
}
