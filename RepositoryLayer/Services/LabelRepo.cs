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
        }

        public string AddLabelToNote(int UserId, int NotesId, string LabelName)
        {
            var note = GetNoteByIds(UserId, NotesId);
            var user = funDooDBContext.Users.FirstOrDefault(u => u.UserId == UserId);
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
                    labelEntity.LabelNote = note;
                    labelEntity.LabelUser = user;

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
            var notes = funDooDBContext.Labels.Where(l => l.UserId == UserId && l.LabelName == LabelName)
                        .Select(l => l.LabelNote).ToList();
            Console.WriteLine(notes.Count);
            
            if (notes.Any()) return notes;
            else throw new Exception("Notes not found for requested label name: " + LabelName);
        }

        public string RenameLabel(int UserId, int NotesId, string OldLabelName, string NewLabelName)
        {
            var label = funDooDBContext.Labels.FirstOrDefault(l => l.UserId == UserId && l.NotesId == NotesId && l.LabelName == OldLabelName);
            if (label != null)
            {
                var label2 = funDooDBContext.Labels.FirstOrDefault(l => l.UserId == UserId && l.NotesId == NotesId && l.LabelName == NewLabelName);
                if (label2 == null)
                {
                    label.LabelName = NewLabelName;
                    funDooDBContext.SaveChanges();
                    return NewLabelName;
                }
                else throw new Exception("Label already exist with new label name: " + NewLabelName);
            }
            else throw new Exception("Label not found for requested label name: " + OldLabelName);
        }

        public string DeleteLabel(int UserId, int NotesId, string LabelName)
        {
            var label = funDooDBContext.Labels.FirstOrDefault(l => l.UserId == UserId && l.NotesId == NotesId && l.LabelName == LabelName);
            if (label != null)
            {
                funDooDBContext.Remove(label);
                funDooDBContext.SaveChanges();
                return LabelName;
            }
            else throw new Exception("Label not found for requested label name: " + LabelName);
        }

    }
}
