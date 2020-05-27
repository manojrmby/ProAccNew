//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ProACC_DB
{
    using System;
    using System.Collections.Generic;
    
    public partial class ProjectMonitor
    {
        public System.Guid Id { get; set; }
        public System.Guid InstanceID { get; set; }
        public string Task { get; set; }
        public Nullable<int> BuildingBlock_id { get; set; }
        public int PhaseId { get; set; }
        public Nullable<int> Sequence_Num { get; set; }
        public string ApplicationArea { get; set; }
        public Nullable<bool> Task_Other_Environment { get; set; }
        public Nullable<bool> Dependency { get; set; }
        public string Pending { get; set; }
        public Nullable<bool> Delay_occurred { get; set; }
        public string DelayedReason { get; set; }
        public System.Guid UserID { get; set; }
        public int StatusId { get; set; }
        public Nullable<double> EST_hours { get; set; }
        public Nullable<double> Actual_St_hours { get; set; }
        public Nullable<System.DateTime> Planed__St_Date { get; set; }
        public Nullable<System.DateTime> Actual_St_Date { get; set; }
        public Nullable<System.DateTime> Planed__En_Date { get; set; }
        public Nullable<System.DateTime> Actual_En_Date { get; set; }
        public string Notes { get; set; }
        public bool isActive { get; set; }
        public System.DateTime Cre_on { get; set; }
        public System.Guid Cre_By { get; set; }
        public Nullable<System.DateTime> Modified_On { get; set; }
        public Nullable<System.Guid> Modified_by { get; set; }
        public bool IsDeleted { get; set; }
        public Nullable<int> ActivityID { get; set; }
        public Nullable<int> RoleId { get; set; }
    
        public virtual PhaseMaster PhaseMaster { get; set; }
        public virtual StatusMaster StatusMaster { get; set; }
        public virtual UserMaster UserMaster { get; set; }
    }
}
