using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProAcc.BL.Model
{
    public class SP_Assessment_Result
    {
        public Nullable<int> COMPLETE { get; set; }
        public Nullable<int> WIP { get; set; }
        public Nullable<int> ONHOLD { get; set; }
        public Nullable<int> YetToStart { get; set; }
    }
}