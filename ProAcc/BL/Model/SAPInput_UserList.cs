using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProAcc.BL.Model
{
    public class SAPInput_UserList
    {
        public int Row_Number { get; set; }
        public string Users { get; set; }
        public string User_Type { get; set; }
        public string Valid_From { get; set; }
        public string Valid_To { get; set; }
        public string Last_logon { get; set; }

        public string System { get; set; }
        public string Catergory { get; set; }

        public DateTime Last_logon_Date { get; set; }
    }
}