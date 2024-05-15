using RepositoryLayer.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace RepositoryLayer.Interfaces
{
    public interface ILabelRepo
    {
        string AddLabelToNote(int UserId, int NotesId, string LabelName);
        List<LabelEntity> GetLabelsByUser(int UserId);
        List<NotesEntity> GetNotesByLabelName(int UserId, string LabelName);
        string RenameLabel(int UserId, int NotesId, string OldLabelName, string NewLabelName);
        string DeleteLabel(int UserId, int NotesId, string LabelName);

    }
}
