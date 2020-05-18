using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProAcc.BL.Model
{
    public class Common
    {

        public class GeneralList
        {

            public List<Lis> _List { get; set; }


        }
        public class Lis
        {
            public string Name { get; set; }

            public string Value { get; set; }
            public string linkID { get; set; }
            public int _Value { get; set; }
        }

        //public static Guid User_ID;
        //public static string User_Type;
        //public static string User_Name;
        //public static Guid InstanceId;
        //public static string Instance_Name;
        //public static string Project_Name;
    }
}