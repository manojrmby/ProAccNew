using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace ProAcc.BL
{
    public class GetQuestionary
    {
        public async Task StartGQPULL()
        {
            mysql();
        }
        //private static void mysql()
        //{

        //    string MyConnectionstring = "Server= sql12.freemysqlhosting.net;Database=sql12353260;Uid=sql12353260;Pwd= qHCWfvIhS7;";
        //    try
        //    {
        //        while (true)
        //        {
        //            using (MySqlConnection conn = new MySqlConnection(MyConnectionstring))
        //            {
        //                MySqlDataAdapter ad = new MySqlDataAdapter("Select * From Customer", conn);
        //                DataTable dt = new DataTable();
        //                ad.Fill(dt);

        //                Console.WriteLine(dt.Rows.Count);
        //                Console.WriteLine("MySql Data:");
        //                Console.WriteLine("Id" + "\t|" + "Name" + "\t|" + "Customer_Type" + "\t|" + " Address" + "\t|" + " Phone_Number");
        //                Console.WriteLine();
        //                foreach (DataRow dr in dt.Rows)
        //                {
        //                    Customer1 p = new Customer1();
        //                    p.id = Convert.ToInt32(dr["Id"].ToString());
        //                    p.name = dr["Name"].ToString();
        //                    p.customer = dr["Customer_Type"].ToString();
        //                    p.address = dr["Address"].ToString();
        //                    p.phone = dr["Phone_Number"].ToString();
        //                    SqlInsertion(p);
        //                    Console.WriteLine("Id" + "\t|" + "Name" + "\t|" + "Customer_Type" + "\t|" + " Address" + "\t|" + " Phone_Number");
        //                }
        //                Console.WriteLine("\n\t SQL DATA INSERTION FROM MYSQL");
        //                Thread.Sleep(5 * 60);
        //            }
        //        }
        //    }



        //    catch (Exception ex)
        //    {
        //        Console.WriteLine("Error: " + ex.Message);
        //    }
        //    Console.Read();
        //}



        //public static void SqlInsertion(Customer1 p)
        //{
        //    string MySqlconnectionstring = "Data Source = 123.176.34.15,4433\\MSSQLSERVER; Initial Catalog = ProAccDevNew; User ID = sa; Password = promantus@123; MultipleActiveResultSets = True; Application Name = EntityFramework";
        //    try
        //    {
        //        using (SqlConnection con = new SqlConnection(MySqlconnectionstring))
        //        {
        //            con.Open();
        //            string count = "Select Count(*) From Customer1 where (Id='" + p.id + "')";
        //            SqlCommand cmd = new SqlCommand(count, con);
        //            int numrecord = (int)cmd.ExecuteScalar();



        //            if (numrecord == 0)
        //            {
        //                string myQuery = "Insert into  Customer1 (Id ,Name,Customer_Type ,Address,Phone_Number) Values('" + p.id + "','" + p.name + "','" + p.customer + "','" + p.address + "','" + p.phone + "')";
        //                SqlCommand cmd2 = new SqlCommand(myQuery, con);
        //                cmd2.ExecuteNonQuery();
        //                con.Close();
        //            }
        //            else
        //            {
        //                Console.WriteLine("Already In records");
        //            }
        //        }
        //    }
        //    catch (Exception e)



        //    {
        //        Console.WriteLine("Error: " + e.Message);
        //    }


        //}

        //public class Customer1
        //{
        //    public int id { get; set; }
        //    public string name { get; set; }
        //    public string customer { get; set; }
        //    public string address { get; set; }
        //    public string phone { get; set; }
        //}

        private async Task mysql()
        {

            //string MyConnectionstring = "Server= 127.0.0.1;Database=survey;User name=root;password ="; //Uid=sql12353260;Pwd= qHCWfvIhS7;
            //string MyConnectionstring = "Server=127.0.0.1;user id = root;password = ;database = survey";
            //string MyConnectionstring = "Driver={MySQL ODBC 5.1 Driver};server= 123.176.34.15;port=8021;Database=survey;Uid=survey;Pwd=Admin1!;option3";
            //string MyConnectionstring = "Server=123.176.34.15,8021;user id = survey;password = Admin1!;database = survey";
            //string MyConnectionstring = "Server= sql12.freemysqlhosting.net;Database=sql12353260;Uid=sql12353260;Pwd= qHCWfvIhS7;";

            //string MyConnectionstring = "Data Source = 123.176.34.15;port=8021;Integrated Security=False; Initial Catalog = survey; User ID = survey; Password = Admin1!;";
            string constr = ConfigurationManager.ConnectionStrings["mysql"].ConnectionString;
            try
            {
                while (true)
                {
                    using (MySqlConnection conn = new MySqlConnection(constr))
                    {

                        MySqlDataAdapter ad = new MySqlDataAdapter("SELECT * FROM question", conn);
                        DataTable dt = new DataTable();
                        ad.Fill(dt);

                        foreach (DataRow dr in dt.Rows)
                        {
                            question p = new question();
                            p.id = Convert.ToInt32(dr["Id"].ToString());
                            p.User_ID = Convert.ToInt32(dr["User_ID"].ToString());
                            p.la_q1 = dr["la_q1"].ToString();
                            p.la_q2 = dr["la_q2"].ToString();
                            p.la_q3 = dr["la_q3"].ToString();
                            p.la_q4 = dr["la_q4"].ToString();
                            p.la_q5 = dr["la_q5"].ToString();
                            p.la_q6 = dr["la_q6"].ToString();
                            p.la_q7 = dr["la_q7"].ToString();
                            p.la_q8 = dr["la_q8"].ToString();
                            p.la_q9 = dr["la_q9"].ToString();
                            p.la_q10 = dr["la_q10"].ToString();
                            p.la_q11 = dr["la_q11"].ToString();
                            p.la_q12 = dr["la_q12"].ToString();
                            p.fq1 = dr["fq1"].ToString();
                            p.fq2 = dr["fq2"].ToString();
                            p.fq3 = dr["fq3"].ToString();
                            p.submitted = Convert.ToInt32(dr["submitted"].ToString());

                            SqlInsertion(p);
                            //Console.WriteLine("Id" + "\t|" + "Name" + "\t|" + "Customer_Type" + "\t|" + " Address" + "\t|" + " Phone_Number");
                        }
                        Console.WriteLine("\n\t SQL DATA INSERTION FROM MYSQL");
                        await Task.Delay(5 * 60 * 1000);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
        }



        public static void SqlInsertion(question p)
        {
            string MySqlconnectionstring = "Data Source = 123.176.34.15,4433\\MSSQLSERVER; Initial Catalog = ProAccDevNew; User ID = sa; Password = promantus@123; MultipleActiveResultSets = True; Application Name = EntityFramework";
            try
            {
                using (SqlConnection con = new SqlConnection(MySqlconnectionstring))
                {
                    con.Open();
                    string count = "Select Count(*) From question where (Id='" + p.id + "')";
                    SqlCommand cmd = new SqlCommand(count, con);
                    int numrecord = (int)cmd.ExecuteScalar();



                    if (numrecord == 0)
                    {
                        string myQuery = "Insert into  question (Id,User_ID,la_q1,la_q2.la_q3,la_q4,la_q5,la_q6,la_q7,la_q8,la_q9,la_q10,la_q11,la_q12,fq1,fq2,fq3,submitted) Values('" + p.id + "','" + p.User_ID + "','" + p.la_q1 + "','" + p.la_q2 + "','" + p.la_q3 + "','" + p.la_q4 + "','" + p.la_q5 + "','" + p.la_q6 + "','" + p.la_q7 + "','" + p.la_q8 + "','" + p.la_q9 + "','" + p.la_q10 + "','" + p.la_q11 + "','" + p.la_q12 + "','" + p.fq1 + "','" + p.fq2 + "','" + p.fq3 + "','" + p.submitted + "')";
                        SqlCommand cmd2 = new SqlCommand(myQuery, con);
                        cmd2.ExecuteNonQuery();
                        con.Close();
                    }
                    else
                    {
                        Console.WriteLine("Already In records");
                    }
                }
            }
            catch (Exception e)



            {
                Console.WriteLine("Error: " + e.Message);
            }


        }

        public class question
        {
            public int id { get; set; }
            public int User_ID { get; set; }
            public string la_q1 { get; set; }
            public string la_q2 { get; set; }
            public string la_q3 { get; set; }
            public string la_q4 { get; set; }
            public string la_q5 { get; set; }
            public string la_q6 { get; set; }
            public string la_q7 { get; set; }
            public string la_q8 { get; set; }
            public string la_q9 { get; set; }
            public string la_q10 { get; set; }
            public string la_q11 { get; set; }
            public string la_q12 { get; set; }
            public string fq1 { get; set; }
            public string fq2 { get; set; }
            public string fq3 { get; set; }
            public int submitted { get; set; }
        }

    }
}