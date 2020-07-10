using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading;
using System.Web;

namespace ProAcc.BL
{
    public class GetQuestionary
    {
        public  void StartGQPULL()
        {
            mysql();
        }
        private static void mysql()
        {

            string MyConnectionstring = "Server= sql12.freemysqlhosting.net;Database=sql12353260;Uid=sql12353260;Pwd= qHCWfvIhS7;";
            try
            {
                while (true)
                {
                    using (MySqlConnection conn = new MySqlConnection(MyConnectionstring))
                    {
                        MySqlDataAdapter ad = new MySqlDataAdapter("Select * From Customer", conn);
                        DataTable dt = new DataTable();
                        ad.Fill(dt);




                        Console.WriteLine(dt.Rows.Count);
                        Console.WriteLine("MySql Data:");
                        Console.WriteLine("Id" + "\t|" + "Name" + "\t|" + "Customer_Type" + "\t|" + " Address" + "\t|" + " Phone_Number");
                        Console.WriteLine();
                        foreach (DataRow dr in dt.Rows)
                        {
                            Customer p = new Customer();
                            p.id = Convert.ToInt32(dr["Id"].ToString());
                            p.name = dr["Name"].ToString();
                            p.customer = dr["Customer_Type"].ToString();
                            p.address = dr["Address"].ToString();
                            p.phone = dr["Phone_Number"].ToString();
                            SqlInsertion(p);
                            Console.WriteLine("Id" + "\t|" + "Name" + "\t|" + "Customer_Type" + "\t|" + " Address" + "\t|" + " Phone_Number");
                        }
                        Console.WriteLine("\n\t SQL DATA INSERTION FROM MYSQL");
                        Thread.Sleep(5 * 60);
                    }
                }
            }



            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
            Console.Read();
        }



        public static void SqlInsertion(Customer p)
        {
            string MySqlconnectionstring = "Data Source=LAPTOP-EENAMS2L;Initial Catalog=Customer;integrated security=True;";
            try
            {
                using (SqlConnection con = new SqlConnection(MySqlconnectionstring))
                {
                    con.Open();
                    string count = "Select Count(*) From Customer where (Id='" + p.id + "')";
                    SqlCommand cmd = new SqlCommand(count, con);
                    int numrecord = (int)cmd.ExecuteScalar();



                    if (numrecord == 0)
                    {
                        string myQuery = "Insert into  Customer (Id ,Name,Customer_Type ,Address,Phone_Number) Values('" + p.id + "','" + p.name + "','" + p.customer + "','" + p.address + "','" + p.phone + "')";
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

        public class Customer

        {
            public int id { get; set; }
            public string name { get; set; }
            public string customer { get; set; }
            public string address { get; set; }
            public string phone { get; set; }
        }

    }
}