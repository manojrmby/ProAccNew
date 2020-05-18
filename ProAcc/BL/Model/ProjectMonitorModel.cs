using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProAcc.BL.Model
{
	public class ProjectMonitorModel
	{
		public int LocalID { get; set; }
		public Guid Id { get; set; }
		public Guid Instance { get; set; }
		public int PhaseId { get; set; }
		public string Task { get; set; }
		public int Sequence { get; set; }
		public bool Task_Other_Environment { get; set; }
		public bool Dependency { get; set; }
		public int PendingId { get; set; }
		public bool Delay_occurred { get; set; }
		public int TeamID { get; set; }
		public Guid ConsultantID { get; set; }
		public int StatusId { get; set; }
		public double EST_hours { get; set; }
		public double Actual_St_hours { get; set; }
		public DateTime Planed__St_Date { get; set; }
		public DateTime Actual_St_Date { get; set; }
		public DateTime Planed__En_Date { get; set; }
		public DateTime Actual_En_Date { get; set; }
		public string Notes { get; set; }
		public bool isActive { get; set; }
		public DateTime Cre_on { get; set; }
		public Guid Cre_By { get; set; }
		public DateTime Modified_On { get; set; }
		public Guid Modified_by { get; set; }
		public bool IsDeleted { get; set; }
	}
}