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
    
    public partial class UserMaster
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public UserMaster()
        {
            this.ProjectMonitors = new HashSet<ProjectMonitor>();
        }
    
        public System.Guid UserId { get; set; }
        public string Name { get; set; }
        public string EMail { get; set; }
        public string Phone { get; set; }
        public string LoginId { get; set; }
        public string Password { get; set; }
        public int RoleID { get; set; }
        public int UserTypeID { get; set; }
        public Nullable<System.Guid> Customer_Id { get; set; }
        public bool isActive { get; set; }
        public System.DateTime Cre_on { get; set; }
        public System.Guid Cre_By { get; set; }
        public Nullable<System.DateTime> Modified_On { get; set; }
        public Nullable<System.Guid> Modified_by { get; set; }
        public bool IsDeleted { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ProjectMonitor> ProjectMonitors { get; set; }
        public virtual RoleMaster RoleMaster { get; set; }
        public virtual User_Type User_Type { get; set; }
        public virtual Customer Customer { get; set; }
    }
}
