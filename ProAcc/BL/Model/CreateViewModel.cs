using ProACC_DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProAcc.BL.Model
{
    public class CreateViewModel
    {
        public Customer Customer { get; set; }
        public UserMaster User { get; set; }
    }
}