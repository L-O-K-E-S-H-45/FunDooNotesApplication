using BusinessLayer.Interfaces;
using RepositoryLayer.Entities;
using RepositoryLayer.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLayer.Services
{
    public class LabelBusiness : ILabelBusiness
    {
        private readonly ILabelRepo labelRepo;
        public LabelBusiness(ILabelRepo labelRepo)
        {
            this.labelRepo = labelRepo;
        }

        public string AddLabelToNote(int UserId, int NotesId, string LabelName)
        {
            return labelRepo.AddLabelToNote(UserId, NotesId, LabelName);
        }

        public List<LabelEntity> GetLabelsByUser(int UserId)
        {
            return labelRepo.GetLabelsByUser(UserId);
        }

        public List<NotesEntity> GetNotesByLabelName(int UserId, string LabelName)
        {
            return labelRepo.GetNotesByLabelName((int)UserId, LabelName);
        }

        public string RenameLabel(int UserId, string OldLabelName, string NewLabelName)
        {
            return labelRepo.RenameLabel(UserId, OldLabelName, NewLabelName);
        }
        public string DeleteLabel(int UserId, int NotesId, string LabelName)
        {
            return labelRepo.DeleteLabel(UserId, NotesId, LabelName);
        }

        public object GetNotesWithLabels()
        {
            return labelRepo.GetNotesWithLabels();
        }

        public object GetUsersWithNotesAndNoteLabels()
        {
            return labelRepo.GetUsersWithNotesAndNoteLabels();
        }
    }
}
