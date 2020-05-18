using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProAcc.BL.Model
{
    public partial class SAPInput_Activities
    {
        public double S_No { get; set; }
        public string Related_Simplification_Items { get; set; }
        public string Activities { get; set; }
        public string Phase { get; set; }
        public string Condition { get; set; }
        public string Additional_Information { get; set; }
        public System.Guid FileUploadID { get; set; }
        public System.DateTime Inserted_on { get; set; }
    }
}