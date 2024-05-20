using RepositoryLayer.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLayer.Interfaces
{
    public interface ILabelBusiness
    {
        string AddLabelToNote(int UserId, int NotesId, string LabelName);
        List<LabelEntity> GetLabelsByUser(int UserId);
        List<NotesEntity> GetNotesByLabelName(int UserId, string LabelName);
        string RenameLabel(int UserId, string OldLabelName, string NewLabelName);
        string DeleteLabel(int UserId, int NotesId, string LabelName);

        object GetNotesWithLabels();
        object GetUsersWithNotesAndNoteLabels();
    }
}
