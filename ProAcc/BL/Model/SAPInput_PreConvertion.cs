using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProAcc.BL.Model
{
    public class SAPInput_PreConvertion
    {

		public int ID { get; set; }
		public Guid FileUploadID { get; set; }
		public int Relevance { get; set; }
		public int Last_Consistency_Result { get; set; }
		public int Exemption_Possible { get; set; }
		public string SAP_ID { get; set; }
		public string Title { get; set; }
		public string Lob_Technology {get;set;}
		public string Business_Area { get; set; }
		public string Catetory { get; set; }
		public string Component { get; set; }
		public string Status { get; set; }
		public string Note { get; set; }
		public string Application_Area { get; set; }
		public string Summary { get; set; }
		public int Test { get; set; }

	}
}