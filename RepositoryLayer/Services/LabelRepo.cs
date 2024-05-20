using RepositoryLayer.Context;
using RepositoryLayer.Entities;
using RepositoryLayer.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RepositoryLayer.Services
{
    public class LabelRepo : ILabelRepo
    {
        private readonly FunDooDBContext funDooDBContext;
        public LabelRepo(FunDooDBContext funDooDBContext)
        {
            this.funDooDBContext = funDooDBContext;
        }

        public NotesEntity GetNoteByIds(int UserId, int NotesId)
        {
            return funDooDBContext.Notes.FirstOrDefault(notes => notes.NotesId == NotesId && notes.UserId == UserId);
            //return  (from n in funDooDBContext.Notes where n.UserId == UserId && n.NotesId == NotesId select n).FirstOrDefault();
        }

        public string AddLabelToNote(int UserId, int NotesId, string LabelName)
        {
            var note = GetNoteByIds(UserId, NotesId);
            if (note != null)
            {
                var label = funDooDBContext.Labels.Any(l => l.LabelName == LabelName && l.NotesId == NotesId && l.UserId == UserId);
                if (!label)
                {
                    LabelEntity labelEntity = new LabelEntity();
                    labelEntity.LabelName = LabelName;
                    labelEntity.NotesId = NotesId;
                    labelEntity.UserId = UserId;
                    Console.WriteLine(note.Title + " -------------------------");
                    // No need to add note & user to fetch notes & user entities
                    //labelEntity.LabelNote = note;
                    //labelEntity.LabelUser = user;

                    funDooDBContext.Labels.Add(labelEntity);
                    funDooDBContext.SaveChanges();
                    Console.WriteLine(labelEntity.LabelNote.Title + " +++++++++++++");
                    return LabelName;
                }
                else
                    throw new Exception("label already exists for note id: " + NotesId);
            }
            else
                throw new Exception("Note not found for requested id: " + NotesId);
        }

        public List<LabelEntity> GetLabelsByUser(int UserId)
        {
            var labels = funDooDBContext.Labels.ToList().FindAll(l =>  l.UserId == UserId);
            if (labels.Any())
                return labels;
            else
                throw new Exception("Labels not found for requested user id: " + UserId);
        }

        public List<NotesEntity> GetNotesByLabelName(int UserId, string LabelName)
        {
            //var notes = funDooDBContext.Labels.Where(l => l.UserId == UserId && l.LabelName == LabelName)
            //            .Select(l => l.LabelNote).ToList();

            var notes = (from l in funDooDBContext.Labels where l.UserId == UserId && l.LabelName == LabelName
                         orderby l.NotesId
                         select l.LabelNote).ToList();

            Console.WriteLine(notes.Count);
            
            if (notes.Any()) return notes;
            else throw new Exception("Notes not found for requested label name: " + LabelName);
        }

        private LabelEntity GetLabel(string LabelName, int NotesId, int UserId)
        {
            return (from l in funDooDBContext.Labels where l.LabelName == LabelName && l.NotesId == NotesId && l.UserId == UserId select l).FirstOrDefault();
        }

        public string RenameLabel(int UserId, string OldLabelName, string NewLabelName)
        {
            var labels = (from l in funDooDBContext.Labels where l.LabelName ==  OldLabelName && l.UserId == UserId select l).ToList();
            if (labels.Any())
            {
                foreach (LabelEntity l in labels)
                {
                    if (GetLabel(NewLabelName, l.NotesId, UserId) == null)
                    {
                        Console.WriteLine("Rename: " + l.LabelId + " " + l.NotesId + " " + UserId);
                        l.LabelName = NewLabelName;
                    }
                    else
                    {
                        Console.WriteLine("Delete: " + l.LabelId + " " + l.NotesId + " " + UserId);
                        funDooDBContext.Labels.Remove(l);
                    }
                    funDooDBContext.SaveChanges();
                }

                return NewLabelName;
            }
            else throw new Exception("Label not found for requested label name: " + OldLabelName);
        }

        public string DeleteLabel(int UserId, int NotesId, string LabelName)
        {
            var label = GetLabel(LabelName, NotesId, UserId);
            if (label != null)
            {
                funDooDBContext.Remove(label);
                funDooDBContext.SaveChanges();
                return LabelName;
            }
            else throw new Exception("Label not found for requested label name: " + LabelName);
        }

        public object GetNotesWithLabels()
        {
            var notes = from NotesEntity n in funDooDBContext.Notes
                        join LabelEntity l in funDooDBContext.Labels on n.NotesId equals l.NotesId
                        orderby n.NotesId, l.LabelId
                        select new
                        {
                            n.NotesId,
                            n.Title,
                            n.Description,
                            n.Reminder,
                            l.LabelId,
                            l.LabelName
                        };
            return notes.Any() ? notes : throw new Exception("Labels not added for notes");
        }

        public object GetUsersWithNotesAndNoteLabels()
        {
            var users = from UserEntity u in funDooDBContext.Users
                        join NotesEntity n in funDooDBContext.Notes on u.UserId equals n.UserId
                        join LabelEntity l in funDooDBContext.Labels on n.NotesId equals l.NotesId
                        orderby u.UserId, n.NotesId, l.LabelName
                        select new
                        {
                           u.UserId,
                           u.FirstName,
                           u.LastName,
                           u.Email,
                           n.NotesId,
                           n.Title,
                           n.Description,
                           n.Reminder,
                           l.LabelId,
                           l.LabelName
                        };
            return users.Any() ? users : throw new Exception("Notes are not added for users");
        }
    }
}
