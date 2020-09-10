using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProAcc.BL.Model
{
    public class PMTaskModel
    {
        public System.Guid PMTaskId { get; set; }
        public string PMTaskName { get; set; }
        public int PMTaskCategoryID { get; set; }
        public String PMTaskCategory { get; set; }
        public Nullable<decimal> EST_hours { get; set; }
        public String EST_hours1 { get; set; }
        public bool isActive { get; set; }
        public System.DateTime Cre_on { get; set; }
        public System.Guid Cre_By { get; set; }
        public Nullable<System.DateTime> Modified_On { get; set; }
        public Nullable<System.Guid> Modified_by { get; set; }
        public bool IsDeleted { get; set; }

        public System.Guid Id { get; set; }
        public System.Guid PMTaskID { get; set; }
        public System.Guid ProjectId { get; set; }
        public Nullable<bool> Task_Other_Environment { get; set; }
        public Nullable<bool> Dependency { get; set; }
        public string Pending { get; set; }
        public Nullable<bool> Delay_occurred { get; set; }
        public string DelayedReason { get; set; }
        public int StatusId { get; set; }
        public String Actual_St_hours { get; set; }
        public Nullable<System.DateTime> Planed__St_Date { get; set; }
        public Nullable<System.DateTime> Actual_St_Date { get; set; }
        public Nullable<System.DateTime> Planed__En_Date { get; set; }
        public Nullable<System.DateTime> Actual_En_Date { get; set; }
        public string Notes { get; set; }
      
    }
}