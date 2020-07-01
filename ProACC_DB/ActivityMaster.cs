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
    
    public partial class ActivityMaster
    {
        public int Activity_ID { get; set; }
        public string Task { get; set; }
        public Nullable<System.Guid> BuildingBlock_id { get; set; }
        public int PhaseID { get; set; }
        public Nullable<int> Sequence_Num { get; set; }
        public int ApplicationAreaID { get; set; }
        public int RoleID { get; set; }
        public bool isActive { get; set; }
        public System.DateTime Cre_on { get; set; }
        public System.Guid Cre_By { get; set; }
        public Nullable<System.DateTime> Modified_On { get; set; }
        public Nullable<System.Guid> Modified_by { get; set; }
        public bool IsDeleted { get; set; }
    
        public virtual ApplicationAreaMaster ApplicationAreaMaster { get; set; }
        public virtual PhaseMaster PhaseMaster { get; set; }
        public virtual RoleMaster RoleMaster { get; set; }
    }
}
