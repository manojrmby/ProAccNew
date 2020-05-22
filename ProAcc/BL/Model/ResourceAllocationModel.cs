using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProAcc.BL.Model
{
    public class ResourceAllocationModel
    {
        public Guid RA_ID { get; set; }
        public String  ProjectID { get; set; }
        public String CustomertID { get; set; }
        public String UserId { get; set; }
        public String ProjName { get; set; }
        public String CustomerName { get; set; }
        public String UserName { get; set; }
    }
}