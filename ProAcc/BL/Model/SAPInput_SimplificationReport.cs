using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProAcc.BL.Model
{
    public class SAPInput_SimplificationReport
    {
        public double S_No { get; set; }
        public string Title { get; set; }
        public string Category { get; set; }
        public string Relevance { get; set; }
        public string LoB_Technology { get; set; }
        public string Business_Area { get; set; }
        public string Consistency_Status { get; set; }
        public string Manual_Status { get; set; }
        public string SAP_Notes { get; set; }
        public string Relevance_Summary { get; set; }
    }
}