using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Text.Json.Serialization;

namespace RepositoryLayer.Entities
{
    public class LabelsLogEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int LabelsLogId { get; set; }

        [ForeignKey("LabelsLog")]
        public int LabelId { get; set; }

        [ForeignKey("LabelNote")]
        public int NotesId { get; set; }

        [ForeignKey("LabelUser")]
        public int UserId { get; set; }

        [JsonIgnore]
        public virtual LabelEntity LabelsLog { get; set; }
        [JsonIgnore]
        public virtual LabelEntity LabelNote { get; set; }
        [JsonIgnore]
        public virtual LabelEntity LabelUser { get; set; }

    }
}
