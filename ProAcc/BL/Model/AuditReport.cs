using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProAcc.BL.Model
{
    public class AuditReport
    {
		public class ProjectMonitorModel
		{

			public int Id { get; set; }
			public string InstaceName { get; set; }
			public string Task { get; set; }
			public string PhaseName { get; set; }
			public string RoleName { get; set; }

			public string Operation { get; set; }
			public DateTime OperationAt { get; set; }

			public string By { get; set; }





			
			public int ActivityID { get; set; }
			public Guid Instance { get; set; }


			public int BuldingBlockID { get; set; }
			public int PhaseId { get; set; }
			public int SequenceNum { get; set; }

			public string ApplicationArea { get; set; }

			public bool Task_Other_Environment { get; set; }
			public bool Dependency { get; set; }
			public string Pending { get; set; }
			public bool Delay_occurred { get; set; }

			public string Delayed_Reas { get; set; }
			public int RoleID { get; set; }
			public Guid UserID { get; set; }
			public int StatusId { get; set; }
			public double EST_hours { get; set; }
			public double Actual_St_hours { get; set; }

			public DateTime Planed__St_Date { get; set; }

			public DateTime Actual_St_Date { get; set; }

			public DateTime Planed__En_Date { get; set; }
			public DateTime Actual_En_Date { get; set; }
			public string Notes { get; set; }
			public Guid Cre_By { get; set; }
			public DateTime Modified_On { get; set; }
			public Guid Modified_by { get; set; }

		}
	}
}