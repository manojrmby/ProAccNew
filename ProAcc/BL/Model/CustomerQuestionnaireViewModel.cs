using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProAcc.BL.Model
{
    public class CustomerQuestionnaireViewModel
    {
        public int User_ID { get; set; }
        public string First_Name { get; set; }
        public string Second_Name { get; set; }
        public string Email { get; set; }
        public string Phone_No { get; set; }
        public Boolean Need_Help { get; set; }
    }
}