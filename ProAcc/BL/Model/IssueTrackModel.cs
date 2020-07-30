using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ProAcc.BL.Model
{
    public class IssueTrackModel
    {
        public System.Guid Issuetrack_Id { get; set; }
        public int RunningID { get; set; }
        public string IssueName { get; set; }
        public int PhaseID { get; set; }
        public Nullable<int> TaskId { get; set; }
        public System.Guid ProjectInstance_Id { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public System.DateTime StartDate { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public System.DateTime EndDate { get; set; }
        public System.DateTime LastUpdatedDate { get; set; }
        public System.Guid AssignedTo { get; set; }
        public bool IsApproved { get; set; }
        public bool isActive { get; set; }
        public System.DateTime Cre_on { get; set; }
        public System.Guid Cre_By { get; set; }
        public Nullable<System.DateTime> Modified_On { get; set; }
        public Nullable<System.Guid> Modified_by { get; set; }
        public bool IsDeleted { get; set; }
        public String Status { get; set; }
        public String Comments { get; set; }
        public String Task { get; set; }
        public String IssueID { get; set; }
        public String Phase { get; set; }
    
        public virtual ProACC_DB.UserMaster UserMaster { get; set; }

        public virtual ProACC_DB.PhaseMaster phaseMaster { get; set; }


    }
}