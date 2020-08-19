//using DocumentFormat.OpenXml;
//using DocumentFormat.OpenXml.Packaging;
//using DocumentFormat.OpenXml.Wordprocessing;
using ProAcc.BL.Model;
using ProACC_DB;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using static ProAcc.BL.Model.Common;
using Spire.Presentation;
using System.Text.RegularExpressions;
using System.Configuration;



//using A = DocumentFormat.OpenXml.Drawing;
//using DW = DocumentFormat.OpenXml.Drawing.Wordprocessing;
//using PIC = DocumentFormat.OpenXml.Drawing.Pictures;

namespace ProAcc.BL
{
    public class Base : Common
    {
        private ProAccEntities db = new ProAccEntities();
        //private LogHelper _Log = new LogHelper();
        public  string _salt = "d5cc07aa70fd47";
        #region Login
        public LogedUser UserValidation(LogedUser user)
        {
            DataSet ds = new DataSet();
           
            DBHelper dB = new DBHelper("SP_Login", CommandType.StoredProcedure);
            dB.addIn("@Type", "Login");
            dB.addIn("@UserName", user.Username);
            dB.addIn("@Password", PasswordEncrypt(user.Password));
            ds = dB.ExecuteDataSet();
            DataTable dt = new DataTable();
            if (ds.Tables.Count != 0)
            {
                //DataTable dt1 = new DataTable();
                dt = ds.Tables[0];
                //dt1 = ds.Tables[1];
                if (dt.Rows.Count == 1)
                {
                    if (!string.IsNullOrEmpty(dt.Rows[0][0].ToString()))
                    {


                        user.ID = Guid.Parse(dt.Rows[0][0].ToString());
                        user.Type = Convert.ToInt32(dt.Rows[0][1].ToString());
                        user.Name = dt.Rows[0][2].ToString();

                        //User_ID = user.ID;
                        //User_Name = user.Name;
                        //for (int i = 0; i < dt1.Rows.Count; i++)
                        //{
                        //    if (user.Type == Convert.ToInt32(dt1.Rows[i]["id"]))
                        //    {
                        //        User_Type = dt1.Rows[0]["UserType"].ToString().ToUpper();
                        //        break;
                        //    }
                        //}
                    }
                }
            }
            return user;

        }

        public string PasswordEncrypt(string st)
        {
            return Cipher.Encrypt(st, _salt);
        }
        public string PasswordDecrypt(string st)
        {
            return Cipher.Decrypt(st, _salt);
        }
        #endregion


        #region Charts
        public GeneralList sP_GetActivities_Bar1(Guid InstanceId)
        {
            GeneralList sP_ = new GeneralList();
            DataTable dt = new DataTable();
            DBHelper dB = new DBHelper("SP_ReadinessReport", CommandType.StoredProcedure);

            dB.addIn("@Type", "Activities_Bar1");
            dB.addIn("@InstanceId", InstanceId);
            dt = dB.ExecuteDataTable();
            if (dt.Rows.Count > 0)
            {
                List<Lis> _Lob = new List<Lis>();
                foreach (DataRow dr in dt.Rows)
                {
                    _Lob.Add(
                        new Lis
                        {
                            Name = dr["ACT_NAME"].ToString(),
                            _Value = Convert.ToInt32(dr["_Count"].ToString()
                            )
                        });

                }

                sP_._List = _Lob;


            }
            return sP_;
        }
        public GeneralList sP_GetActivities_Bar2(Guid InstanceId)
        {
            GeneralList sP_ = new GeneralList();
            DataTable dt = new DataTable();
            DBHelper dB = new DBHelper("SP_ReadinessReport", CommandType.StoredProcedure);

            dB.addIn("@Type", "Activities_Bar2");
            dB.addIn("@InstanceId", InstanceId);
            dt = dB.ExecuteDataTable();
            if (dt.Rows.Count > 0)
            {
                List<Lis> _Lob = new List<Lis>();
                foreach (DataRow dr in dt.Rows)
                {
                    _Lob.Add(
                        new Lis
                        {
                            Name = dr["Phase"].ToString(),
                            _Value = Convert.ToInt32(dr["_Count"].ToString()
                            )
                        });

                }

                sP_._List = _Lob;


            }
            return sP_;
        }
        public GeneralList sP_GetActivities_Donut(Guid InstanceId)
        {
            GeneralList sP_ = new GeneralList();
            DataTable dt = new DataTable();
            DBHelper dB = new DBHelper("SP_ReadinessReport", CommandType.StoredProcedure);

            dB.addIn("@Type", "Activities_Donut");
            dB.addIn("@InstanceId", InstanceId);
            dt = dB.ExecuteDataTable();
            if (dt.Rows.Count > 0)
            {
                List<Lis> _Lob = new List<Lis>();
                foreach (DataRow dr in dt.Rows)
                {
                    _Lob.Add(
                        new Lis
                        {
                            Name = dr["Condition"].ToString(),
                            _Value = Convert.ToInt32(dr["_Count"].ToString()
                            )
                        });

                }

                sP_._List = _Lob;


            }
            return sP_;
        }
        public GeneralList sP_GetFiori_Bar(Guid InstanceId)
        {
            GeneralList sP_ = new GeneralList();
            DataTable dt = new DataTable();
            DBHelper dB = new DBHelper("SP_ReadinessReport", CommandType.StoredProcedure);

            dB.addIn("@Type", "Fiori_Bar");
            dB.addIn("@InstanceId", InstanceId);
            dt = dB.ExecuteDataTable();
            if (dt.Rows.Count > 0)
            {
                List<Lis> _Lob = new List<Lis>();
                foreach (DataRow dr in dt.Rows)
                {
                    _Lob.Add(
                        new Lis
                        {
                            Name = dr["Application_Area"].ToString(),
                            _Value = Convert.ToInt32(dr["_Count"].ToString()
                            )
                        });

                }

                sP_._List = _Lob;


            }
            return sP_;
        }
        public GeneralList sP_GetCustomCode_Bar(Guid InstanceId)
        {
            GeneralList sP_ = new GeneralList();
            DataTable dt = new DataTable();
            DBHelper dB = new DBHelper("SP_ReadinessReport", CommandType.StoredProcedure);

            dB.addIn("@Type", "CustomCode_Bar");
            dB.addIn("@InstanceId", InstanceId);
            dt = dB.ExecuteDataTable();
            if (dt.Rows.Count > 0)
            {
                List<Lis> _Lob = new List<Lis>();
                foreach (DataRow dr in dt.Rows)
                {
                    _Lob.Add(
                        new Lis
                        {
                            Name = dr["_Status"].ToString(),
                            _Value = Convert.ToInt32(dr["_Count"].ToString()
                            )
                        });

                }

                sP_._List = _Lob;


            }
            return sP_;
        }
        public GeneralList sP_SimplificationReport_Bar(String Input, Guid InstanceId)
        {
            GeneralList sP_ = new GeneralList();
            DataTable dt = new DataTable();
            DBHelper dB = new DBHelper("SP_SimplificationReport", CommandType.StoredProcedure);
            if (Input == "ALL")
            {
                dB.addIn("@Type", "ALL");
                dB.addIn("@InstanceId", InstanceId);
            }
            else
            {
                dB.addIn("@Type", "Single");
                dB.addIn("@Input", Input);
                dB.addIn("@InstanceId", InstanceId);
            }
            dt = dB.ExecuteDataTable();
            if (dt.Rows.Count > 0)
            {
                List<Lis> _Lob = new List<Lis>();
                foreach (DataRow dr in dt.Rows)
                {
                    _Lob.Add(
                        new Lis
                        {
                            Name = dr["LOB_NAME"].ToString(),
                            _Value = Convert.ToInt32(dr["_Count"].ToString()
                            )
                        });

                }

                sP_._List = _Lob;


            }
            return sP_;
        }

        public GeneralList sP_GetSAPFioriAppsReport_Bar(string Input, Guid InstanceId)
        {
            GeneralList sP_ = new GeneralList();//manoj
            DataTable dt = new DataTable();
            DBHelper dB = new DBHelper("SP_FioriAppsReport", CommandType.StoredProcedure);

            dB.addIn("@Type", "ALL");
            dB.addIn("@InstanceId", InstanceId);
            dB.addIn("@Input", Input);
            dt = dB.ExecuteDataTable();
            if (dt.Rows.Count > 0)
            {
                List<Lis> _Lob = new List<Lis>();
                foreach (DataRow dr in dt.Rows)
                {
                    _Lob.Add(
                        new Lis
                        {
                            Name = dr["Roles"].ToString(),
                            _Value = Convert.ToInt32(dr["_Count"].ToString()
                            )
                        });

                }

                sP_._List = _Lob;


            }
            return sP_;
        }

        public GeneralList sP_GetActivitiesReport_Bar(string Phase, string condition, Guid InstanceId)
        {
            GeneralList sP_ = new GeneralList();//manoj
            DataTable dt = new DataTable();
            DBHelper dB = new DBHelper("SP_ActivitiesReport", CommandType.StoredProcedure);

            dB.addIn("@Type", "ALL");
            dB.addIn("@InstanceId", InstanceId);
            dB.addIn("@Phase", Phase);
            dB.addIn("@condition", condition);
            dt = dB.ExecuteDataTable();
            if (dt.Rows.Count > 0)
            {
                List<Lis> _Lob = new List<Lis>();
                foreach (DataRow dr in dt.Rows)
                {
                    _Lob.Add(
                        new Lis
                        {
                            Name = dr["ACT_NAME"].ToString(),
                            _Value = Convert.ToInt32(dr["_Count"].ToString()
                            )
                        });

                }

                sP_._List = _Lob;


            }
            return sP_;
        }
        #endregion

        #region Reports
        public SP_ReadinessReport_Result sAPInput(Guid InstanceId)
        {
            SP_ReadinessReport_Result GetRelevant = new SP_ReadinessReport_Result();
            DataTable dt = new DataTable();
            DBHelper dB = new DBHelper("SP_ReadinessReport", CommandType.StoredProcedure);
            dB.addIn("@Type", "Simple_Donut");
            dB.addIn("@InstanceId", InstanceId);
            dt = dB.ExecuteDataTable();
            if (dt.Rows.Count == 1)
            {
                GetRelevant.RC = Convert.ToInt32(dt.Rows[0]["RC"].ToString());
                GetRelevant.RC_NON = Convert.ToInt32(dt.Rows[0]["RC_NON"].ToString());
                GetRelevant.R_NON = Convert.ToInt32(dt.Rows[0]["R_NON"].ToString());
                GetRelevant.R = Convert.ToInt32(dt.Rows[0]["R"].ToString());
            }
            return GetRelevant;
        }

        public List<SP_HomeDonut_Result> SAP_Home_Donut(Guid InstanceId)
        {

            List<SP_HomeDonut_Result> LS = new List<SP_HomeDonut_Result>();
            DataSet dS = new DataSet();
            DBHelper dB = new DBHelper("SP_Home_Donut", CommandType.StoredProcedure);
            dB.addIn("@Type", "Simple_Donut");
            dB.addIn("@InstanceId", InstanceId);
            dS = dB.ExecuteDataSet();
            if (dS.Tables.Count > 0)
            {
                for (int i = 0; i < dS.Tables.Count; i++)
                {
                    DataTable dt = new DataTable();
                    dt = dS.Tables[i];
                    SP_HomeDonut_Result GetResult = new SP_HomeDonut_Result();
                    GetResult.COMPLETE = Convert.ToInt32(dt.Rows[0]["COMPLETE"]);
                    GetResult.WIP = Convert.ToInt32(dt.Rows[0]["WIP"]);
                    GetResult.ONHOLD = Convert.ToInt32(dt.Rows[0]["ONHOLD"]);
                    GetResult.YetToStart = Convert.ToInt32(dt.Rows[0]["YetToStart"]);
                    GetResult.NA = Convert.ToInt32(dt.Rows[0]["NA"]);
                    GetResult.UploadStatus = Convert.ToInt32(dt.Rows[0]["UploadStatus"]);
                    LS.Add(GetResult);
                }


            }


            //if (dt.Rows.Count == 1)
            //{
            //    //var a = Convert.ToInt32(dt.Rows[0]["COMPLETE"]);
            //    //var b = Convert.ToInt32(dt.Rows[0]["WIP"]);
            //    //var c = Convert.ToInt32(dt.Rows[0]["ONHOLD"]);
            //    //var d = Convert.ToInt32(dt.Rows[0]["YetToStart"]);

            //    //var e = a + b + c + d;

            //    //int T1 = (int)Math.Round((double)(100 * a) / e);  
            //    //int T2 = (int)Math.Round((double)(100 * b) / e);  
            //    //int T3 = (int)Math.Round((double)(100 * c) / e); 
            //    //int T4 = (int)Math.Round((double)(100 * d) / e); 

            //    //GetRelevant.COMPLETE = T1; // Convert.ToInt32(T1);//Convert.ToInt32(dt.Rows[0]["COMPLETE"].ToString());
            //    //GetRelevant.WIP = T2; // Convert.ToInt32(T2.ToString()); //Convert.ToInt32(dt.Rows[0]["WIP"].ToString());
            //    //GetRelevant.ONHOLD = T3; // Convert.ToInt32(T3.ToString()); //Convert.ToInt32(dt.Rows[0]["ONHOLD"].ToString());
            //    //GetRelevant.YetToStart = T4+"%"; // Convert.ToInt32(T4.ToString());//Convert.ToInt32(dt.Rows[0]["YetToStart"].ToString());

            //    GetRelevant.COMPLETE = Convert.ToInt32(dt.Rows[0]["As_COMPLETE"]);
            //    GetRelevant.WIP = Convert.ToInt32(dt.Rows[0]["As_WIP"]);
            //    GetRelevant.ONHOLD = Convert.ToInt32(dt.Rows[0]["As_ONHOLD"]);
            //    GetRelevant.YetToStart = Convert.ToInt32(dt.Rows[0]["As_YetToStart"]);
            //    GetRelevant.NA = Convert.ToInt32(dt.Rows[0]["As_NA"]);
            //    GetRelevant.UploadStatus = Convert.ToInt32(dt.Rows[0]["As_UploadStatus"]);

            //}
            return LS;
        }
        public GeneralList sP_SimplificationReport(Guid InstanceId)
        {
            GeneralList sP_ = new GeneralList();
            DataTable dt = new DataTable();
            DBHelper dB = new DBHelper("SP_SimplificationReport", CommandType.StoredProcedure);
            dB.addIn("@Type", "GetDropdown");
            dB.addIn("@InstanceId", InstanceId);
            dt = dB.ExecuteDataTable();
            List<Lis> _Lob = new List<Lis>();

            int count = 0;
            foreach (DataRow dr in dt.Rows)
            {
                _Lob.Add(new Lis { Name = dr["LOB"].ToString(), _Value = count });
                count = count++;
            }

            sP_._List = _Lob;
            return sP_;
        }

        public List<SAPInput_SimplificationReport> SAPInput_Simplification(Guid InstanceId, string LOB)
        {
            List<SAPInput_SimplificationReport> SR = new List<SAPInput_SimplificationReport>();
            DataTable dt = new DataTable();
            DBHelper dB = new DBHelper("SP_SimplificationReport", CommandType.StoredProcedure);

            dB.addIn("@InstanceId", InstanceId);
            if (LOB == "ALL")
            {
                dB.addIn("@Type", "SR_Table_All");
            }
            else
            {
                dB.addIn("@Type", "SR_Table_Single");
                dB.addIn("@Input", LOB);

            }
            dt = dB.ExecuteDataTable();
            //  List<DataRow> list = new List<DataRow>(dt.Select());
            int i = 0;
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    i += 1;
                    SAPInput_SimplificationReport data = new SAPInput_SimplificationReport();
                    data.S_No = i;
                    data.Title = dr["Title"].ToString();
                    data.Category = dr["Category"].ToString();
                    data.Relevance = dr["Relevance"].ToString();
                    data.LoB_Technology = dr["LoB/Technology"].ToString();
                    data.Business_Area = dr["Business Area"].ToString();
                    data.Consistency_Status = dr["Consistency Status"].ToString();
                    data.Manual_Status = dr["Manual Status"].ToString();
                    data.SAP_Notes = dr["SAP Notes"].ToString();
                    data.Relevance_Summary = dr["Relevance Summary"].ToString();
                    SR.Add(data);
                }
            }
            return SR;
        }

        public List<SAPInput_CustomCode> SAPInput_CustomCodeReport(Guid InstanceId)
        {
            List<SAPInput_CustomCode> CR = new List<SAPInput_CustomCode>();
            DataTable dt = new DataTable();
            DBHelper dB = new DBHelper("SP_CustomCode", CommandType.StoredProcedure);
            dB.addIn("@Type", "CustomTable");
            dB.addIn("@InstanceId", InstanceId);
            dt = dB.ExecuteDataTable();
            //  List<DataRow> list = new List<DataRow>(dt.Select());
            int i = 0;
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    i += 1;
                    SAPInput_CustomCode data = new SAPInput_CustomCode();
                    data.S_No = i;
                    data.Custom_Code_Topic = dr["Custom Code Topic"].ToString();
                    data.Status = dr["Status"].ToString();
                    data.Application_Component = dr["Application Component"].ToString();
                    data.Custom_Code_Note = dr["Custom Code Note"].ToString();

                    CR.Add(data);
                }
            }
            return CR;
        }

        public List<Model.SAPInput_Activities> GetActivitiesReport_Table(string Phase, string condition, Guid InstanceId)
        {
            List<Model.SAPInput_Activities> AR = new List<Model.SAPInput_Activities>();
            DataTable dt = new DataTable();
            DBHelper dB = new DBHelper("SP_ActivitiesReport", CommandType.StoredProcedure);
            dB.addIn("@Type", "ACT_Table");
            dB.addIn("@InstanceId", InstanceId);
            dB.addIn("@Phase", Phase);
            dB.addIn("@condition", condition);
            dt = dB.ExecuteDataTable();
            //  List<DataRow> list = new List<DataRow>(dt.Select());
            int i = 0;
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    i += 1;
                    Model.SAPInput_Activities data = new Model.SAPInput_Activities();
                    data.S_No = i;
                    data.Related_Simplification_Items = dr["Related Simplification Items"].ToString();
                    data.Activities = dr["Activities"].ToString();
                    data.Phase = dr["Phase"].ToString();
                    data.Condition = dr["Condition"].ToString();
                    data.Additional_Information = dr["Additional Information"].ToString();

                    AR.Add(data);
                }
            }
            return AR;
        }
        public List<SAPFioriAppsModel> sp_GetSAPFioriAppsTable(String Input, Guid InstanceId)
        {
            List<SAPFioriAppsModel> FiR = new List<SAPFioriAppsModel>();
            DataTable dt = new DataTable();
            DBHelper dB = new DBHelper("SP_FioriAppsReport", CommandType.StoredProcedure);
            //dB.addIn("@Type", "FioriApps_Table");
            dB.addIn("@InstanceId", InstanceId);
            if (Input == "ALL")
            {
                dB.addIn("@Type", "FioriApps_Table");
            }
            else
            {
                dB.addIn("@Type", "FioriApps_Table_Single");
                dB.addIn("@Input", Input);

            }
            dt = dB.ExecuteDataTable();
            //  List<DataRow> list = new List<DataRow>(dt.Select());
            int i = 0;
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    i += 1;
                    SAPFioriAppsModel data = new SAPFioriAppsModel();
                    data.S_No = i;
                    data.Role = dr["Role"].ToString();
                    data.Name = dr["Name"].ToString();
                    data.Application_Area = dr["Application Area"].ToString();
                    data.Description = dr["Description"].ToString();

                    FiR.Add(data);
                }
            }
            return FiR;
        }

        public List<SAPInput_PreConvertion> sp_GetPreConvertionTable(Guid InstanceId)
        {
            List<SAPInput_PreConvertion> PR = new List<SAPInput_PreConvertion>();
            DataSet DS = new DataSet();
            DBHelper dB = new DBHelper("SP_PreConvertion", CommandType.StoredProcedure);
            dB.addIn("@Type", "PreConvertion_Table");
            dB.addIn("@InstanceId", InstanceId);
            DS = dB.ExecuteDataSet();
            DataTable dt1 = new DataTable();
            DataTable dt2 = new DataTable();

            dt1 = DS.Tables[0];
            dt2 = DS.Tables[1];
            List<PicturetoData> Pic = new List<PicturetoData>();
            if (dt2.Rows.Count > 0)
            {
                foreach (DataRow dr in dt2.Rows)
                {
                    PicturetoData P = new PicturetoData();
                    P.ID = Convert.ToInt32(dr["Id"].ToString());
                    P.PictureName = dr["PictureName"].ToString();
                    P.GivenName = dr["GivenName"].ToString();
                    Pic.Add(P);
                }
            }

            if (dt1.Rows.Count > 0)
            {
                foreach (DataRow dr in dt1.Rows)
                {
                    //string Re_Result="";
                    //string Re_Possible="";
                    SAPInput_PreConvertion data = new SAPInput_PreConvertion();

                    //foreach (var item in Pic)
                    //{
                    //    var Result = dr["Last Consistency Result"].ToString();
                    //    var Possible = dr["Exemption Possible"].ToString();
                    //    if (item.PictureName == Result)
                    //    {
                    //        Re_Result = item.GivenName.ToString();
                    //    }
                    //    if (item.PictureName == Possible)
                    //    {
                    //        Re_Possible = item.GivenName.ToString();
                    //    }

                    //}


                    //if (Result== Empty)
                    //{Result = RE_Empty;}
                    //else if(Result== Error)
                    //{ Result = RE_Error; }
                    //else if (Result == Warning)
                    //{ Result = RE_Warning; }
                    //else if (Result == NotApplicable)
                    //{ Result = RE_NotApplicable; }
                    //else if (Result == Success)
                    //{ Result = RE_Success; }

                    //if (Possible == Empty)
                    //{ Possible = RE_Empty; }
                    //else if (Possible == Error)
                    //{ Possible = RE_Error; }
                    //else if (Possible == Warning)
                    //{ Possible = RE_Warning; }
                    //else if (Possible == NotApplicable)
                    //{ Possible = RE_NotApplicable; }
                    //else if (Possible == Success)
                    //{ Possible = RE_Success; }

                    data.ID = Convert.ToInt32(dr["id"].ToString());
                    data.Relevance = Convert.ToInt32(dr["Relevance"].ToString());
                    data.Last_Consistency_Result = Convert.ToInt32(dr["Last Consistency Result"].ToString());
                    data.Exemption_Possible = Convert.ToInt32(dr["Exemption Possible"].ToString());
                    data.SAP_ID = dr["SAP_ID"].ToString();
                    data.Title = dr["Title"].ToString();
                    data.Lob_Technology = dr["Lob/Technology"].ToString();
                    data.Business_Area = dr["Business Area"].ToString();
                    data.Catetory = dr["Catetory"].ToString();
                    data.Component = dr["Component"].ToString();
                    data.Status = dr["Status"].ToString();
                    data.Note = dr["Note"].ToString();
                    data.Application_Area = dr["Application Area"].ToString();
                    data.Summary = dr["Summary"].ToString();
                    data.Test = 2;
                    PR.Add(data);
                }
            }
            return PR;
        }
        #endregion

        #region FileUpload
        public Boolean Upload_Activities(DataTable CustomTable, string fileName, Guid Instance_ID, Guid User_ID)
        {
            Boolean status = false;


            DBHelper dB = new DBHelper("SP_FileUpload", CommandType.StoredProcedure);

            dB.addIn("@Type", "up_Activities");
            dB.addIn("@tblActivities", CustomTable);
            dB.addIn("@File_Type", "Activities");
            dB.addIn("@FileUploadID", Guid.NewGuid());
            dB.addIn("@instanceId", Instance_ID);
            dB.addIn("@fileName", fileName);
            dB.addIn("@Createdby", User_ID);

            dB.ExecuteScalar();
            status = true;
            return status;

        }
        public Boolean Upload_CustomCode(DataTable CustomTable, string fileName, Guid Instance_ID, Guid User_ID)
        {
            Boolean status = false;


            DBHelper dB = new DBHelper("SP_FileUpload", CommandType.StoredProcedure);

            dB.addIn("@Type", "up_CustomCode");
            dB.addIn("@tblCustomCode", CustomTable);
            dB.addIn("@File_Type", "CustomCode");
            dB.addIn("@FileUploadID", Guid.NewGuid());
            dB.addIn("@instanceId", Instance_ID);
            dB.addIn("@fileName", fileName);
            dB.addIn("@Createdby", User_ID);

            dB.ExecuteScalar();
            status = true;
            return status;

        }
        public Boolean Upload_FioriApps(DataTable CustomTable, string fileName, Guid Instance_ID, Guid User_ID)
        {
            Boolean status = false;


            DBHelper dB = new DBHelper("SP_FileUpload", CommandType.StoredProcedure);

            dB.addIn("@Type", "up_FioriApps");
            dB.addIn("@tblFioriApps", CustomTable);
            dB.addIn("@File_Type", "RecommendedFioriApp");
            dB.addIn("@FileUploadID", Guid.NewGuid());
            dB.addIn("@instanceId", Instance_ID);
            dB.addIn("@fileName", fileName);
            dB.addIn("@Createdby", User_ID);

            dB.ExecuteScalar();
            status = true;
            return status;

        }

        public Boolean Upload_Simplification(DataTable CustomTable, string fileName, Guid Instance_ID, Guid User_ID)
        {
            Boolean status = false;


            DBHelper dB = new DBHelper("SP_FileUpload", CommandType.StoredProcedure);

            dB.addIn("@Type", "up_Simplification");
            dB.addIn("@tblSimplification", CustomTable);
            dB.addIn("@File_Type", "RelevantSimplificationItems");
            dB.addIn("@FileUploadID", Guid.NewGuid());
            dB.addIn("@instanceId", Instance_ID);
            dB.addIn("@fileName", fileName);
            dB.addIn("@Createdby", User_ID);

            dB.ExecuteScalar();
            status = true;
            return status;

        }

        public Boolean Upload_Bwextractors(string fileName, Guid Instance_ID, Guid User_ID)
        {
            Boolean status = false;


            DBHelper dB = new DBHelper("SP_FileUpload", CommandType.StoredProcedure);

            dB.addIn("@Type", "up_Bwextractors");
            //dB.addIn("@tblSimplification", CustomTable);
            dB.addIn("@File_Type", "Bwextractors");
            dB.addIn("@FileUploadID", Guid.NewGuid());
            dB.addIn("@instanceId", Instance_ID);
            dB.addIn("@fileName", fileName);
            dB.addIn("@Createdby", User_ID);

            dB.ExecuteScalar();
            status = true;
            return status;

        }
        public Boolean Upload_HanaDatabaseTables(string fileName, Guid Instance_ID, Guid User_ID)
        {
            Boolean status = false;


            DBHelper dB = new DBHelper("SP_FileUpload", CommandType.StoredProcedure);

            dB.addIn("@Type", "up_HanaDatabaseTables");
            //dB.addIn("@tblSimplification", CustomTable);
            dB.addIn("@File_Type", "HanaDatabaseTables");
            dB.addIn("@FileUploadID", Guid.NewGuid());
            dB.addIn("@instanceId", Instance_ID);
            dB.addIn("@fileName", fileName);
            dB.addIn("@Createdby", User_ID);

            dB.ExecuteScalar();
            status = true;
            return status;

        }
        public Boolean Upload_SAPReadinessCheck(string fileName, Guid Instance_ID, Guid User_ID)
        {
            Boolean status = false;


            DBHelper dB = new DBHelper("SP_FileUpload", CommandType.StoredProcedure);

            dB.addIn("@Type", "up_SAPReadinessCheck");
            //dB.addIn("@tblSimplification", CustomTable);
            dB.addIn("@File_Type", "SAPReadinessCheck");
            dB.addIn("@FileUploadID", Guid.NewGuid());
            dB.addIn("@instanceId", Instance_ID);
            dB.addIn("@fileName", fileName);
            dB.addIn("@Createdby", User_ID);

            dB.ExecuteScalar();
            status = true;
            return status;

        }

        public Boolean Upload_SAPPreConvertion(DataTable CustomTable, string fileName, Guid Instance_ID, Guid User_ID)
        {
            Boolean status = false;


            DBHelper dB = new DBHelper("SP_FileUpload", CommandType.StoredProcedure);

            dB.addIn("@Type", "up_SAPPreConvertion");
            dB.addIn("@tblPreConvertion", CustomTable);
            dB.addIn("@File_Type", "PreConvertion");
            dB.addIn("@FileUploadID", Guid.NewGuid());
            dB.addIn("@instanceId", Instance_ID);
            dB.addIn("@fileName", fileName);
            dB.addIn("@Createdby", User_ID);

            var a = dB.ExecuteScalar();
            status = true;
            return status;


        }

        public Boolean GetUploadRevert(Guid Instance_ID, Guid User_Id)
        {
            Boolean status = false;

            DBHelper dB = new DBHelper("SP_CreateAnalysis_UploadRevert", CommandType.StoredProcedure);
            dB.addIn("@LOGINID", User_Id);
            dB.addIn("@InstanceID", Instance_ID);

            var a = dB.ExecuteScalar();
            status = true;
            return status;
        }

        #endregion


        #region Drop down
        public GeneralList GetInstanceDropdown(string projectID)
        {
            GeneralList sP_ = new GeneralList();
            DataSet ds = new DataSet();
            Guid ID = Guid.Parse(projectID);
            DBHelper dB = new DBHelper("SP_Instance", CommandType.StoredProcedure);
            dB.addIn("@Type", "GetInstance");
            dB.addIn("@Id", ID);
            ds = dB.ExecuteDataSet();
            List<Lis> _Lob = new List<Lis>();
            if (ds.Tables.Count > 0)
            {
                DataTable dt = new DataTable();
                dt = ds.Tables[0];
                int count = 0;
                foreach (DataRow dr in dt.Rows)
                {
                    _Lob.Add(new Lis
                    {
                        Name = dr["InstaceName"].ToString(),
                        Value = dr["Id"].ToString()
                    });
                    count = count++;
                }
            }


            sP_._List = _Lob;
            return sP_;
        }
        public GeneralList GetPerConvertionUploadInstance(string projectID)
        {
            GeneralList sP_ = new GeneralList();
            DataSet ds = new DataSet();
            Guid ID = Guid.Parse(projectID);
            DBHelper dB = new DBHelper("SP_Instance", CommandType.StoredProcedure);
            dB.addIn("@Type", "GetPerConvertionUploadInstance");
            dB.addIn("@Id", ID);
            ds = dB.ExecuteDataSet();
            List<Lis> _Lob = new List<Lis>();
            if (ds.Tables.Count > 0)
            {
                DataTable dt = new DataTable();
                dt = ds.Tables[0];
                int count = 0;
                foreach (DataRow dr in dt.Rows)
                {
                    _Lob.Add(new Lis
                    {
                        Name = dr["InstaceName"].ToString(),
                        Value = dr["Id"].ToString()
                    });
                    count = count++;
                }
            }


            sP_._List = _Lob;
            return sP_;
        }

        public GeneralList spCustomerDropdown(string Id, int type)
        {
            GeneralList sP_ = new GeneralList();
            DataSet ds = new DataSet();
            DBHelper dB = new DBHelper("SP_CustomerDrp", CommandType.StoredProcedure);
            if (type == 1)
            {
                dB.addIn("@Type", "CustomerDrp_Admin");

            }
            else if (type == 2)
            {
                dB.addIn("@Type", "CustomerDrp_Consultant");
                dB.addIn("@Id", Id);
            }
            else if (type == 3)
            {
                dB.addIn("@Type", "CustomerDrp_Customer");
                dB.addIn("@Id", Id);
            }

            ds = dB.ExecuteDataSet();
            List<Lis> _Lob = new List<Lis>();
            if (ds.Tables.Count > 0)
            {
                DataTable dt = new DataTable();
                dt = ds.Tables[0];
                int count = 0;
                foreach (DataRow dr in dt.Rows)
                {
                    _Lob.Add(new Lis
                    {
                        Name = dr["Name"].ToString(),
                        Value = dr["id"].ToString()
                    });
                    count = count++;
                }
            }
            sP_._List = _Lob;
            return sP_;
        }
        public Lis SpConvertionStatus(string Instance)
        {
            Lis Status = new Lis();
            DataTable dt = new DataTable();
            DBHelper dB = new DBHelper("SP_ConvertionStatus", CommandType.StoredProcedure);
            dB.addIn("@Type", "Status");
            dB.addIn("@Instance", Instance);
            dt = dB.ExecuteDataTable();
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    Status.Value = dr[0].ToString();

                }
            }

            return Status;
        }
        public GeneralList sP_AnalysisDropdownProject()
        {
            GeneralList sP_ = new GeneralList();
            DataSet ds = new DataSet();
            DBHelper dB = new DBHelper("SP_CreateAnalysis", CommandType.StoredProcedure);
            dB.addIn("@Type", "Drp_Project");
            ds = dB.ExecuteDataSet();
            List<Lis> _Lob = new List<Lis>();
            if (ds.Tables.Count > 0)
            {
                DataTable dt = new DataTable();
                dt = ds.Tables[0];
                foreach (DataRow dr in dt.Rows)
                {
                    _Lob.Add(new Lis
                    {
                        Name = dr["ProjectName"].ToString(),
                        Value = dr["CustProjconfigID"].ToString()
                    });

                }
            }


            sP_._List = _Lob;
            return sP_;
        }

        public Tuple<List<Lis>, List<Lis>, List<Lis>> sP_AnalysisDropdowns()
        {
            List<Lis> list1 = new List<Lis>();
            List<Lis> list2 = new List<Lis>();
            List<Lis> list3 = new List<Lis>();
            DataSet ds = new DataSet();
            DBHelper dB = new DBHelper("SP_CreateAnalysis", CommandType.StoredProcedure);
            dB.addIn("@Type", "Drp_Project");
            ds = dB.ExecuteDataSet();

            if (ds.Tables.Count > 0)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    DataTable dt = new DataTable();
                    dt = ds.Tables[0];//Customer
                    foreach (DataRow dr in dt.Rows)
                    {
                        list1.Add(new Lis
                        {
                            Name = dr["Name"].ToString(),
                            Value = dr["ID"].ToString()
                        });

                    }
                }
                if (ds.Tables[1].Rows.Count > 0)
                {
                    DataTable dt = new DataTable();
                    dt = ds.Tables[1];//ProjectName
                    foreach (DataRow dr in dt.Rows)
                    {
                        list2.Add(new Lis
                        {

                            Name = dr["ProjectName"].ToString(),
                            Value = dr["CustProjconfigID"].ToString(),
                            linkID = dr["CustomerID"].ToString()
                        });

                    }
                }
                if (ds.Tables[2].Rows.Count > 0)
                {
                    DataTable dt = new DataTable();
                    dt = ds.Tables[2];//ProjectName
                    foreach (DataRow dr in dt.Rows)
                    {
                        list3.Add(new Lis
                        {

                            Name = dr["InstaceName"].ToString(),
                            Value = dr["ProjectInstanceConfigID"].ToString(),
                            linkID = dr["CustProjconfigID"].ToString()
                        });

                    }
                }

            }

            return Tuple.Create(list1, list2, list3);
        }

        public Tuple<List<Lis>, List<Lis>> sp_GetActivitiesReportDropdown(Guid InstanceId)
        {
            List<Lis> list1 = new List<Lis>();
            List<Lis> list2 = new List<Lis>();
            DataSet ds = new DataSet();
            DBHelper dB = new DBHelper("SP_ActivitiesReport", CommandType.StoredProcedure);
            dB.addIn("@Type", "GetDropdown");
            dB.addIn("@InstanceId", InstanceId);
            ds = dB.ExecuteDataSet();
            if (ds.Tables.Count > 0)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    DataTable dt = new DataTable();
                    dt = ds.Tables[0];
                    int count = 0;
                    foreach (DataRow dr in dt.Rows)
                    {
                        list1.Add(new Lis
                        {
                            Name = dr["Phase"].ToString(),
                            _Value = count
                        });
                        count = count++;
                    }
                }
                if (ds.Tables[1].Rows.Count > 0)
                {
                    DataTable dt = new DataTable();
                    dt = ds.Tables[1];
                    int count = 0;
                    foreach (DataRow dr in dt.Rows)
                    {
                        list2.Add(new Lis
                        {

                            Name = dr["Condition"].ToString(),
                            _Value = count
                        });
                        count = count++;
                    }
                }
            }

            return Tuple.Create(list1, list2);
        }
        public GeneralList sp_GetFioriAppsReportDropdown(Guid InstanceId)
        {
            GeneralList sP_ = new GeneralList();
            DataSet ds = new DataSet();
            DBHelper dB = new DBHelper("SP_FioriAppsReport", CommandType.StoredProcedure);
            dB.addIn("@Type", "GetDropdown");
            dB.addIn("@InstanceId", InstanceId);
            ds = dB.ExecuteDataSet();
            List<Lis> _Lob = new List<Lis>();
            if (ds.Tables.Count > 0)
            {
                DataTable dt = new DataTable();
                dt = ds.Tables[0];
                int count = 0;
                foreach (DataRow dr in dt.Rows)
                {
                    _Lob.Add(new Lis
                    {
                        Name = dr["Roles"].ToString(),
                        _Value = count
                    });
                    count = count++;
                }
            }


            sP_._List = _Lob;
            return sP_;
        }

        public List<PicturetoData> Sp_GetPicturetoDatas()
        {

            DataTable dt = new DataTable();
            DBHelper dB = new DBHelper("SP_PictureTodata", CommandType.StoredProcedure);
            dB.addIn("@Type", "DropDown");
            //dB.addIn("@InstanceId", InstanceId);
            dt = dB.ExecuteDataTable();
            List<PicturetoData> Pic = new List<PicturetoData>();
            if (dt.Rows.Count > 0)
            {

                foreach (DataRow dr in dt.Rows)
                {
                    PicturetoData P = new PicturetoData();
                    P.ID = Convert.ToInt32(dr["Id"].ToString());
                    P.PictureName = dr["PictureName"].ToString();
                    P.GivenName = dr["GivenName"].ToString();
                    Pic.Add(P);
                }
            }
            return Pic;
        }


        public Tuple<List<Lis>> SP_GetAllUser()
        {
            List<Lis> list1 = new List<Lis>();
            DataSet ds = new DataSet();
            DBHelper dB = new DBHelper("SP_ResorceAllocation", CommandType.StoredProcedure);
            dB.addIn("@Type", "GetAllUser");
            ds = dB.ExecuteDataSet();
            if (ds.Tables.Count > 0)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    DataTable dt = new DataTable();
                    dt = ds.Tables[0];
                    foreach (DataRow dr in dt.Rows)
                    {
                        list1.Add(new Lis
                        {
                            Name = dr["FullName"].ToString(),
                            Value = dr["UserId"].ToString(),
                            linkID = dr["RoleName"].ToString()
                        });

                    }
                }
            }
            return Tuple.Create(list1);
        }
        #endregion

        #region CUD
        public Boolean sp_PreConvertion_Update(SAPInput_PreConvertion Data)
        {
            Boolean Result = false;
            DBHelper dB = new DBHelper("SP_PreConvertion", CommandType.StoredProcedure);
            dB.addIn("@Type", "UpdatePreConvertion");
            dB.addIn("@ID", Data.ID);
            dB.addIn("@Last_Consistency_Result", Data.Last_Consistency_Result);
            dB.addIn("@Exemption_Possible", Data.Exemption_Possible);
            dB.ExecuteScalar();
            Result = true;

            return Result;
        }
        #endregion

        #region Masters
        public List<PhaseMaster> GetPhaseMasters()
        {
            DataTable dt = new DataTable();
            DBHelper dB = new DBHelper("SP_Master", CommandType.StoredProcedure);
            dB.addIn("@Type", "GetPhase");
            dt = dB.ExecuteDataTable();
            List<PhaseMaster> PM = new List<PhaseMaster>();
            if (dt.Rows.Count > 0)
            {

                foreach (DataRow dr in dt.Rows)
                {
                    PhaseMaster P = new PhaseMaster();
                    P.Id = Convert.ToInt32(dr["Id"].ToString());
                    P.PhaseName = dr["PhaseName"].ToString();
                    PM.Add(P);
                }
            }

            return PM;
        }
        public List<ApplicationAreaMaster> GetApplicationAreaMasters()
        {
            DataTable dt = new DataTable();
            DBHelper dB = new DBHelper("SP_Master", CommandType.StoredProcedure);
            dB.addIn("@Type", "GetApplicationArea");
            dt = dB.ExecuteDataTable();
            List<ApplicationAreaMaster> AA = new List<ApplicationAreaMaster>();
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    ApplicationAreaMaster P = new ApplicationAreaMaster();
                    P.Id = Convert.ToInt32(dr["Id"].ToString());
                    P.ApplicationArea = dr["ApplicationArea"].ToString();
                    AA.Add(P);
                }
            }

            return AA;
        }

        //public List<PendingMaster> GetPendingMasters()
        //{
        //    DataTable dt = new DataTable();
        //    DBHelper dB = new DBHelper("SP_Master", CommandType.StoredProcedure);
        //    dB.addIn("@Type", "GetPending");
        //    dt = dB.ExecuteDataTable();
        //    List<PendingMaster> PM = new List<PendingMaster>();
        //    if (dt.Rows.Count > 0)
        //    {

        //        foreach (DataRow dr in dt.Rows)
        //        {
        //            PendingMaster P = new PendingMaster();
        //            P.Id = Convert.ToInt32(dr["Id"].ToString());
        //            P.PendingName = dr["PendingName"].ToString();
        //            PM.Add(P);
        //        }
        //    }

        //    return PM;
        //}
        public List<RoleMaster> GetRoleMasters()
        {
            DataTable dt = new DataTable();
            DBHelper dB = new DBHelper("SP_Master", CommandType.StoredProcedure);
            dB.addIn("@Type", "GetRole");
            dt = dB.ExecuteDataTable();
            List<RoleMaster> TM = new List<RoleMaster>();
            if (dt.Rows.Count > 0)
            {

                foreach (DataRow dr in dt.Rows)
                {
                    RoleMaster P = new RoleMaster();
                    P.RoleId = Convert.ToInt32(dr["RoleId"].ToString());
                    P.RoleName = dr["RoleName"].ToString();
                    TM.Add(P);
                }
            }

            return TM;
        }
        public List<Buldingblock> GetBuldingblock()
        {
            DataTable dt = new DataTable();
            DBHelper dB = new DBHelper("SP_Master", CommandType.StoredProcedure);
            dB.addIn("@Type", "GetBuldingblock");
            dt = dB.ExecuteDataTable();
            List<Buldingblock> TM = new List<Buldingblock>();
            if (dt.Rows.Count > 0)
            {

                foreach (DataRow dr in dt.Rows)
                {
                    Buldingblock P = new Buldingblock();
                    P.block_ID = Convert.ToInt32(dr["block_ID"].ToString());
                    P.Block_Name = dr["Block_Name"].ToString();
                    TM.Add(P);
                }
            }

            return TM;
        }
        public List<UserMaster> GetUser()
        {
            DataTable dt = new DataTable();
            DBHelper dB = new DBHelper("SP_Master", CommandType.StoredProcedure);
            dB.addIn("@Type", "GetUser");
            dt = dB.ExecuteDataTable();
            List<UserMaster> L = new List<UserMaster>();
            if (dt.Rows.Count > 0)
            {

                foreach (DataRow dr in dt.Rows)
                {
                    UserMaster P = new UserMaster();
                    P.UserId = Guid.Parse(dr["UserId"].ToString());
                    P.Name = dr["Name"].ToString();
                    P.RoleID = Convert.ToInt32(dr["RoleId"].ToString());
                    if (dr["Customer_Id"].ToString() != "")
                    {
                        P.Customer_Id = Guid.Parse(dr["Customer_Id"].ToString());
                    }

                    L.Add(P);
                }
            }

            return L;
        }
        public List<StatusMaster> GetStatus()
        {
            DataTable dt = new DataTable();
            DBHelper dB = new DBHelper("SP_Master", CommandType.StoredProcedure);
            dB.addIn("@Type", "GetStatus");
            dt = dB.ExecuteDataTable();
            List<StatusMaster> L = new List<StatusMaster>();
            if (dt.Rows.Count > 0)
            {

                foreach (DataRow dr in dt.Rows)
                {
                    StatusMaster P = new StatusMaster();
                    P.Id = Convert.ToInt32(dr["Id"].ToString());
                    P.StatusName = dr["StatusName"].ToString();
                    L.Add(P);
                }
            }

            return L;
        }

        public List<Instance> GetInstanceMasters()
        {
            DataTable dt = new DataTable();
            DBHelper dB = new DBHelper("SP_Master", CommandType.StoredProcedure);
            dB.addIn("@Type", "GetInstance");
            dt = dB.ExecuteDataTable();
            List<Instance> PM = new List<Instance>();
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    Instance P = new Instance();
                    P.Instance_id = Guid.Parse(dr["Instance_id"].ToString());
                    P.InstaceName = dr["InstaceName"].ToString();
                    PM.Add(P);
                }
            }

            return PM;
        }
        #endregion



        #region Other



        public void CreateIfMissing(string path)
        {
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
        }



        public void SendExcepToDB(string ExceptionMsg, string ExceptionType, string ExceptionURL, string ExceptionSource)
        {
            DBHelper dB = new DBHelper("Sp_ExceptionLogging", CommandType.StoredProcedure);
            dB.addIn("@ExceptionMsg", ExceptionMsg);
            dB.addIn("@ExceptionType", ExceptionType);
            dB.addIn("@ExceptionURL", ExceptionURL);
            dB.addIn("@ExceptionSource", ExceptionSource);
            dB.ExecuteScalar();
        }


        public List<ProjectMonitorModel> Sp_GetProjectMonitorEdit(Guid InstanceId, string LoginID, int PhaseId)
        {
            DataTable dt = new DataTable();
            bool Status = false;
            if (PhaseId != 1)
            {
                DBHelper dB1 = new DBHelper("SP_ProjectMonitor", CommandType.StoredProcedure);
                dB1.addIn("@Type", "CheckPreviousPhase");
                dB1.addIn("@InstunceID", InstanceId);
                dB1.addIn("@PhaseId", PhaseId);
                dB1.addIn("@Cre_By", LoginID);
                dt = dB1.ExecuteDataTable();
                if (dt.Rows.Count == 0)
                {
                    Status = true;
                }
            }
            else
            {
                Status = true;
            }
            List<ProjectMonitorModel> PM = new List<ProjectMonitorModel>();
            if (Status)
            {
                DBHelper dB = new DBHelper("SP_ProjectMonitor", CommandType.StoredProcedure);
                dB.addIn("@Type", "PullData");
                dB.addIn("@InstunceID", InstanceId);
                dB.addIn("@PhaseId", PhaseId);
                dB.addIn("@Cre_By", LoginID);
                dt = dB.ExecuteDataTable();

                if (dt.Rows.Count > 0)
                {
                    //int count = 1;
                    var myLocalDateTime = DateTime.UtcNow;
                    foreach (DataRow dr in dt.Rows)
                    {

                        ProjectMonitorModel P = new ProjectMonitorModel();
                        P.Id = Guid.Parse(dr["id"].ToString());
                        P.ActivityID = Convert.ToInt32(dr["ActivityID"].ToString());
                        P.BuldingBlockID = Convert.ToInt32(dr["BuildingBlock_ID"].ToString());
                        P.Instance = Guid.Parse(dr["InstanceID"].ToString());
                        P.Task = dr["Task"].ToString();
                        P.PhaseId = Convert.ToInt32(dr["PhaseId"].ToString());
                        P.SequenceNum = Convert.ToInt32(dr["Sequence_Num"].ToString());
                        P.ApplicationAreaID = Convert.ToInt32(dr["ApplicationAreaID"].ToString());  //dr["ApplicationAreaID"].ToString();
                        P.Task_Other_Environment = Convert.ToBoolean(dr["Task_Other_Environment"].ToString());
                        P.Dependency = Convert.ToBoolean(dr["Dependency"].ToString());
                        P.Pending = dr["Pending"].ToString();
                        P.Delay_occurred = Convert.ToBoolean(dr["Delay_occurred"].ToString());
                        P.RoleID = Convert.ToInt32(dr["RoleId"].ToString());
                        P.UserID = Guid.Parse(dr["UserID"].ToString());
                        P.StatusId = Convert.ToInt32(dr["StatusId"].ToString());

                        P.EST_hours = float.Parse(dr["EST_hours"].ToString());
                        P.Actual_St_hours = float.Parse(dr["Actual_St_hours"].ToString());

                        P.Planed__St_Date = Convert.ToDateTime(dr["Planed__St_Date"].ToString());
                        P.Actual_St_Date = Convert.ToDateTime(dr["Actual_St_Date"].ToString());
                        P.Planed__En_Date = Convert.ToDateTime(dr["Planed__En_Date"].ToString());
                        P.Actual_En_Date = Convert.ToDateTime(dr["Actual_En_Date"].ToString());
                        P.Notes = dr["Notes"].ToString();

                        //P.Task_Other_Environment = false;
                        //P.Dependency = false;
                        //P.Pending = "";
                        //P.Delay_occurred = false;
                        //P.ConsultantID=
                        //P.StatusId = 0;
                        //P.EST_hours = 0;
                        //P.Actual_St_hours = 0;
                        //P.Planed__St_Date = TimeZone.CurrentTimeZone.ToUniversalTime(myLocalDateTime);
                        //P.Actual_St_Date = TimeZone.CurrentTimeZone.ToUniversalTime(myLocalDateTime);
                        //P.Planed__En_Date = TimeZone.CurrentTimeZone.ToUniversalTime(myLocalDateTime);
                        //P.Actual_En_Date = TimeZone.CurrentTimeZone.ToUniversalTime(myLocalDateTime);
                        //P.Notes = "";




                        //P.ID = Convert.ToInt32(dr["Id"].ToString());
                        //P.PictureName = dr["PictureName"].ToString();
                        //P.GivenName = dr["GivenName"].ToString();
                        PM.Add(P);
                    }
                }
            }
            return PM;
        }
        public List<ProjectMonitorModel> Sp_GetProjectMonitor(Guid InstanceId, string LoginID, int PhaseId)
        {
            DataTable dt = new DataTable();
            DBHelper dB = new DBHelper("SP_ProjectMonitor", CommandType.StoredProcedure);
            dB.addIn("@Type", "CheckResource");
            dB.addIn("@InstunceID", InstanceId);
            dB.addIn("@PhaseId", PhaseId);
            dB.addIn("@Cre_By", LoginID);
            dt = dB.ExecuteDataTable();
            List<ProjectMonitorModel> PM = new List<ProjectMonitorModel>();
            if (dt.Rows.Count > 0)
            {
                //int count = 1;
                var myLocalDateTime = DateTime.UtcNow;
                foreach (DataRow dr in dt.Rows)
                {

                    ProjectMonitorModel P = new ProjectMonitorModel();
                    P.Id = Guid.Parse(dr["id"].ToString());
                    P.BuldingBlockID = Convert.ToInt32(dr["BuildingBlock_ID"].ToString());
                    P.UserID = Guid.Parse(dr["UserID"].ToString());
                    P.PhaseId = Convert.ToInt32(dr["PhaseId"].ToString());
                    P.Task = dr["Task"].ToString();
                    P.SequenceNum = Convert.ToInt32(dr["Sequence_Num"].ToString());
                    P.RoleID = Convert.ToInt32(dr["RoleID"].ToString());
                    P.StatusId = Convert.ToInt32(dr["StatusId"].ToString());
                    P.Notes = "";
                    PM.Add(P);
                }
            }

            return PM;
        }
        public bool Sp_UpdateResourceTask(ProjectMonitorModel PM)
        {
            bool Status = false;
            DBHelper dB = new DBHelper("SP_ProjectMonitor", CommandType.StoredProcedure);
            dB.addIn("@Type", "UpdateResourceTask");
            dB.addIn("@Id", PM.Id);
            dB.addIn("@InstunceID", PM.Instance);
            dB.addIn("@RoleId", PM.RoleID);
            dB.addIn("@UserID", PM.UserID);
            dB.addIn("@Cre_By", PM.Modified_by);
            dB.addIn("@on", PM.Modified_On);

            dB.ExecuteScalar();
            Status = true;
            return Status;

        }
        public bool Sp_DeleteResourceTask(ProjectMonitorModel PM)
        {
            bool Status = false;
            DBHelper dB = new DBHelper("SP_ProjectMonitor", CommandType.StoredProcedure);
            dB.addIn("@Type", "DeleteResourceTask");
            dB.addIn("@Id", PM.Id);
            //dB.addIn("@InstunceID", PM.Instance);
            //dB.addIn("@UserID", PM.UserID);
            dB.addIn("@Cre_By", PM.Modified_by);
            dB.addIn("@on", PM.Modified_On);

            dB.ExecuteScalar();
            Status = true;
            return Status;

        }

        public bool Sp_UpdateMonitor(ProjectMonitorModel PM, int PhaseId)
        {
            bool Status = false;
            LogHelper _log = new LogHelper();
            try
            {
                _log.createLog((PM.Planed__St_Date).ToString());
                DBHelper dB = new DBHelper("SP_ProjectMonitor", CommandType.StoredProcedure);
                dB.addIn("@Type", "UpdateTask");
                dB.addIn("@ApplicationArea", PM.ApplicationAreaID);
                dB.addIn("@Task_Other_Environment", PM.Task_Other_Environment);
                dB.addIn("@Dependency", PM.Dependency);
                dB.addIn("@Pending", PM.Pending);
                dB.addIn("@Delay_occurred", PM.Delay_occurred);

                dB.addIn("@StatusId", PM.StatusId);
                dB.addIn("@EST_hours", PM.EST_hours);
                dB.addIn("@Actual_St_hours", PM.Actual_St_hours);

                dB.addIn("@Planed__St_Date", PM.Planed__St_Date, SqlDbType.DateTime);
                dB.addIn("@Actual_St_Date", PM.Actual_St_Date, SqlDbType.DateTime);
                dB.addIn("@Planed__En_Date", PM.Planed__En_Date, SqlDbType.DateTime);
                dB.addIn("@Actual_En_Date", PM.Actual_En_Date, SqlDbType.DateTime);

                dB.addIn("@Notes", PM.Notes);
                dB.addIn("@Cre_By", PM.Cre_By);
                dB.addIn("@Id", PM.Id);
                dB.ExecuteScalar();
                Boolean s = UpdateMonitor_Mail(PM, PhaseId);
                if (s)
                    Status = true;
                
            }
            catch (Exception ex)
            {

                _log.createLog(ex, "");
            }

            return Status;

        }
       
        public Boolean SP_SubmitResource(String IDS, Guid ProjectID, Guid LoginID)
        {
            bool Result = false;

            DBHelper dB = new DBHelper("SP_ResorceAllocation", CommandType.StoredProcedure);
            dB.addIn("@Type", "SubmitResource");
            dB.addIn("@User_IDs", IDS);
            dB.addIn("@Project_Id", ProjectID);
            dB.addIn("@CrBy", LoginID);
            dB.ExecuteScalar();
            Result = true;
            return Result;
        }

        public Boolean Sp_AddNewTask(ProjectMonitorModel PM)
        {
            Boolean Res = false;
            DBHelper dB = new DBHelper("SP_ProjectMonitor", CommandType.StoredProcedure);
            dB.addIn("@Type", "AddNewTask");
            dB.addIn("@InstunceID", PM.Instance);
            dB.addIn("@Task", PM.Task);
            dB.addIn("@PhaseId", PM.PhaseId);
            dB.addIn("@Sequence_Num", PM.SequenceNum);
            dB.addIn("@Cre_By", PM.Cre_By);
            dB.ExecuteScalar();
            Res = true;
            return Res;
        }

        public bool Sp_ResourceCheck_Assement(string InstanceID)
        {
            bool Res = true;

            return Res;
        }
        #endregion



        #region Extra
        public List<ResourceAllocationModel> SP_GetResourceAllocation(String ProjectID)
        {
            DataTable dt = new DataTable();
            DBHelper dB = new DBHelper("SP_ResorceAllocation", CommandType.StoredProcedure);
            dB.addIn("@Type", "GetAllData");
            dB.addIn("@Project_Id", ProjectID);
            dt = dB.ExecuteDataTable();
            List<ResourceAllocationModel> RA = new List<ResourceAllocationModel>();
            if (dt.Rows.Count > 0)
            {

                foreach (DataRow dr in dt.Rows)
                {
                    ResourceAllocationModel L = new ResourceAllocationModel();
                    L.RA_ID = Guid.Parse(dr["RA_ID"].ToString());
                    L.ProjectID = dr["Project_Id"].ToString();
                    L.CustomertID = dr["Customer_Id"].ToString();

                    L.ProjName = dr["Project_Name"].ToString();
                    L.CustomerName = dr["Company_Name"].ToString();

                    L.UserName = dr["NAME"].ToString();
                    L.UserId = dr["USERIDS"].ToString();

                    RA.Add(L);
                }
            }


            return RA;
        }



        //private string User_ID = HttpContext.Current.Session["UserName"].ToString();
        //private string InstanceId = HttpContext.Current.Session["UserName"].ToString();
        //Graph ReadinessReport



        //public Boolean AddInstance(string ProjectID, string InstaceName, Guid Instance_ID)
        //{
        //    Boolean status = false;

        //    Guid Project_ID = Guid.Parse(ProjectID);
        //    DBHelper dB = new DBHelper("SP_Instance", CommandType.StoredProcedure);
        //    dB.addIn("@Type", "AddInstance");
        //    dB.addIn("@Id", Instance_ID);
        //    dB.addIn("@InstaceName", InstaceName);
        //    dB.addIn("@CustProjconfigID", Project_ID);
        //    dB.addIn("@Cre_By", userID);
        //    dB.ExecuteScalar();
        //    status = false;
        //    return status;
        //}

        //public bool AddInstance(string IDInstance)
        //{
        //    bool status = false;
        //    //InstanceId = Guid.Parse(IDInstance);
        //    Guid ProjectID = Guid.Empty;
        //    Guid IDInstanceID = Guid.Parse(IDInstance);
        //    Instance_Name = db.ProjectInstanceConfigs.FirstOrDefault(x => x.Id == IDInstanceID).InstaceName;
        //    ProjectID = db.ProjectInstanceConfigs.FirstOrDefault(x => x.Id == IDInstanceID).CustProjconfigID;
        //    Project_Name = db.CustomerProjectConfigs.FirstOrDefault(x => x.Id == ProjectID).ProjectName;
        //    return status;
        //}
        #endregion

        public List<ProjectMonitorModel> Sp_GetMasterAdd(Guid InstanceId, int PhaseId)
        {
            DataTable dt = new DataTable();
            DBHelper dB = new DBHelper("SP_ProjectMonitor", CommandType.StoredProcedure);
            dB.addIn("@Type", "GetMasterAdd");
            dB.addIn("@InstunceID", InstanceId);
            dB.addIn("@PhaseId", PhaseId);

            // dB.addIn("@Cre_By", LoginID);
            dt = dB.ExecuteDataTable();
            List<ProjectMonitorModel> PM = new List<ProjectMonitorModel>();
            if (dt.Rows.Count > 0)
            {
                //int count = 1;
                var myLocalDateTime = DateTime.UtcNow;
                foreach (DataRow dr in dt.Rows)
                {

                    ProjectMonitorModel P = new ProjectMonitorModel();
                    P.ActivityID = Convert.ToInt32(dr["Activity_ID"].ToString());
                    //Guid.Parse(dr["id"].ToString());
                    //P.UserID = Guid.Parse(dr["UserID"].ToString());
                    //P.Instance = InstanceId;

                    P.Task = dr["Task"].ToString();
                    P.PhaseId = Convert.ToInt32(dr["PhaseId"].ToString());
                    P.SequenceNum = Convert.ToInt32(dr["Sequence_Num"].ToString());
                    //P.Task_Other_Environment = false;
                    //P.Dependency = false;
                    //P.Pending = "";
                    //P.Delay_occurred = false;
                    P.RoleID = Convert.ToInt32(dr["RoleID"].ToString());
                    //P.ConsultantID=
                    //P.StatusId = 0;
                    //P.EST_hours = 0;
                    //P.Actual_St_hours = 0;
                    //P.Planed__St_Date = TimeZone.CurrentTimeZone.ToUniversalTime(myLocalDateTime);
                    //P.Actual_St_Date = TimeZone.CurrentTimeZone.ToUniversalTime(myLocalDateTime);
                    //P.Planed__En_Date = TimeZone.CurrentTimeZone.ToUniversalTime(myLocalDateTime);
                    //P.Actual_En_Date = TimeZone.CurrentTimeZone.ToUniversalTime(myLocalDateTime);
                    //P.Notes = "";




                    //P.ID = Convert.ToInt32(dr["Id"].ToString());
                    //P.PictureName = dr["PictureName"].ToString();
                    //P.GivenName = dr["GivenName"].ToString();
                    PM.Add(P);
                }
            }

            return PM;
        }

        public Boolean Sp_AddResource(Guid InstanceId, int Activity_ID, string LoginID)
        {
            Boolean Result = false;
            DataTable dt = new DataTable();
            DBHelper dB = new DBHelper("SP_ProjectMonitor", CommandType.StoredProcedure);
            dB.addIn("@Type", "AddResource");
            dB.addIn("@InstunceID", InstanceId);
            dB.addIn("@ActivityId", Activity_ID);
            dB.addIn("@Cre_By", LoginID);
            dB.ExecuteScalar();
            Result = true;
            return Result;
        }
        //private string Empty = "";
        //private string Error = "Picture@5C\\QInconsistency at level error@";
        //private string Warning = "Picture@5D\\QInconsistency at level warning@";
        //private string NotApplicable = "Picture@00\\QNot Applicable@";
        //private string Success = "Picture@5B\\QNo potential inconsistency@";

        //private string RE_Empty = "Not Applicable";
        //private string RE_Error = "Error";
        //private string RE_Warning = "Warning";
        //private string RE_NotApplicable = "Not Applicable";
        //private string RE_Success = "Success";

        public List<ProjectMonitorModel> Sp_GetReportData(Guid InstanceId, string LoginID)
        {
            DataTable dt = new DataTable();

            List<ProjectMonitorModel> PM = new List<ProjectMonitorModel>();

            DBHelper dB = new DBHelper("SP_ProjectMonitor", CommandType.StoredProcedure);
            dB.addIn("@Type", "GetReportData");
            dB.addIn("@InstunceID", InstanceId);
            dt = dB.ExecuteDataTable();

            if (dt.Rows.Count > 0)
            {
                //int count = 1;
                var myLocalDateTime = DateTime.UtcNow;
                foreach (DataRow dr in dt.Rows)
                {

                    ProjectMonitorModel P = new ProjectMonitorModel();
                    P.Id = Guid.Parse(dr["id"].ToString());
                    P.ActivityID = Convert.ToInt32(dr["ActivityID"].ToString());
                    P.Instance = Guid.Parse(dr["InstanceID"].ToString());
                    P.Task = dr["Task"].ToString();
                    P.PhaseId = Convert.ToInt32(dr["PhaseId"].ToString());
                    P.SequenceNum = Convert.ToInt32(dr["Sequence_Num"].ToString());
                    P.ApplicationAreaID = Convert.ToInt32(dr["ApplicationAreaID"].ToString());  //dr["ApplicationArea"].ToString();
                    P.Task_Other_Environment = Convert.ToBoolean(dr["Task_Other_Environment"].ToString());
                    P.Dependency = Convert.ToBoolean(dr["Dependency"].ToString());
                    P.Pending = dr["Pending"].ToString();
                    P.Delay_occurred = Convert.ToBoolean(dr["Delay_occurred"].ToString());
                    P.RoleID = Convert.ToInt32(dr["RoleId"].ToString());
                    P.UserID = Guid.Parse(dr["UserID"].ToString());
                    P.StatusId = Convert.ToInt32(dr["StatusId"].ToString());

                    P.EST_hours = float.Parse(dr["EST_hours"].ToString());
                    P.Actual_St_hours = float.Parse(dr["Actual_St_hours"].ToString());

                    P.Planed__St_Date = Convert.ToDateTime(dr["Planed__St_Date"].ToString());
                    P.Actual_St_Date = Convert.ToDateTime(dr["Actual_St_Date"].ToString());
                    P.Planed__En_Date = Convert.ToDateTime(dr["Planed__En_Date"].ToString());
                    P.Actual_En_Date = Convert.ToDateTime(dr["Actual_En_Date"].ToString());
                    P.Notes = dr["Notes"].ToString();

                    PM.Add(P);
                }
            }

            return PM;
        }

        public List<ProjectMonitorModel> Sp_GetReportDataReport(Guid InstanceId, string LoginID)
        {
            DataTable dt = new DataTable();

            List<ProjectMonitorModel> PM = new List<ProjectMonitorModel>();

            DBHelper dB = new DBHelper("SP_ProjectMonitor", CommandType.StoredProcedure);
            dB.addIn("@Type", "GetReportData");
            dB.addIn("@InstunceID", InstanceId);
            dt = dB.ExecuteDataTable();

            if (dt.Rows.Count > 0)
            {
                //int count = 1;
                var myLocalDateTime = DateTime.UtcNow;
                foreach (DataRow dr in dt.Rows)
                {

                    ProjectMonitorModel P = new ProjectMonitorModel();
                    P.Id = Guid.Parse(dr["id"].ToString());
                    P.ActivityID = Convert.ToInt32(dr["ActivityID"].ToString());
                    P.Instance = Guid.Parse(dr["InstanceID"].ToString());
                    P.BuldingBlockID = Convert.ToInt32(dr["BuildingBlock_id"].ToString());
                    P.Task = dr["Task"].ToString();
                    P.PhaseId = Convert.ToInt32(dr["PhaseId"].ToString());
                    P.SequenceNum = Convert.ToInt32(dr["Sequence_Num"].ToString());
                    P.ApplicationAreaID = Convert.ToInt32(dr["ApplicationAreaID"].ToString());  //dr["ApplicationArea"].ToString();
                    P.Task_Other_Environment = Convert.ToBoolean(dr["Task_Other_Environment"].ToString());
                    P.Dependency = Convert.ToBoolean(dr["Dependency"].ToString());
                    P.Pending = dr["Pending"].ToString();
                    P.Delay_occurred = Convert.ToBoolean(dr["Delay_occurred"].ToString());
                    if (P.Delay_occurred)
                    {
                        P.Delay_occurred_Report = "Yes";
                    }
                    else
                    {
                        P.Delay_occurred_Report = "NO";
                    }
                    
                    P.RoleID = Convert.ToInt32(dr["RoleId"].ToString());
                    P.UserID = Guid.Parse(dr["UserID"].ToString());
                    P.StatusId = Convert.ToInt32(dr["StatusId"].ToString());

                    P.EST_hours = float.Parse(dr["EST_hours"].ToString());
                    P.Actual_St_hours = float.Parse(dr["Actual_St_hours"].ToString());

                    P.Planed__St_Date = Convert.ToDateTime(dr["Planed__St_Date"].ToString());
                    P.Actual_St_Date = Convert.ToDateTime(dr["Actual_St_Date"].ToString());
                    P.Planed__En_Date = Convert.ToDateTime(dr["Planed__En_Date"].ToString());
                    P.Actual_En_Date = Convert.ToDateTime(dr["Actual_En_Date"].ToString());
                    P.Notes = dr["Notes"].ToString();

                    PM.Add(P);
                }
            }

            return PM;
        }


        public List<AuditReport.ProjectMonitorModel> Sp_GetAuditData()
        {
            DataTable dt = new DataTable();
            AuditReport A = new AuditReport();
            List<AuditReport.ProjectMonitorModel> AR = new List<AuditReport.ProjectMonitorModel>();
            DBHelper dB = new DBHelper("SP_Audit", CommandType.StoredProcedure);
            dB.addIn("@Type", "OverAllReport");
            dt = dB.ExecuteDataTable();
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    AuditReport.ProjectMonitorModel A_PM = new AuditReport.ProjectMonitorModel();
                    A_PM.Id = Convert.ToInt32(dr["Change_id"].ToString());
                    A_PM.InstaceName = dr["InstaceName"].ToString();
                    A_PM.Task = dr["Task"].ToString();
                    A_PM.PhaseName = dr["PhaseName"].ToString();
                    A_PM.RoleName = dr["RoleName"].ToString();

                    A_PM.Operation = dr["Operation"].ToString();
                    A_PM.OperationAt = Convert.ToDateTime(dr["OperationAt"].ToString());

                    A_PM.By = dr["By"].ToString();

                    AR.Add(A_PM);

                }
            }

            return AR;
        }

        public List<AuditReport.ProjectMonitorModel> Sp_GetAuditDatas(AuditReport.ProjectMonitorModel model)
        {
            DataTable dt = new DataTable();
            AuditReport A = new AuditReport();
            List<AuditReport.ProjectMonitorModel> AR = new List<AuditReport.ProjectMonitorModel>();
            DBHelper dB = new DBHelper("SP_Audit", CommandType.StoredProcedure);
            if (model.ActionID == null && model.TABLE_NAME == null)
            {
                dB.addIn("@Type", "AuditReport");
            }
            else
            {
                dB.addIn("@Type", "AuditReportsearch");
                dB.addIn("@Action", model.ActionID);
                dB.addIn("@Tablename", model.TABLE_NAME);
            }
            dB.addIn("@Startdate", model.startdate);
            dB.addIn("@enddate", model.enddate);
            dt = dB.ExecuteDataTable();
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    AuditReport.ProjectMonitorModel A_PM = new AuditReport.ProjectMonitorModel();
                    A_PM.Id = Convert.ToInt32(dr["AUDIT_ID"].ToString());
                    A_PM.NAME = dr["NAME"].ToString();
                    A_PM.TABLE_NAME = dr["TABLE_NAME"].ToString();
                    A_PM.SUMMARY = dr["SUMMARY"].ToString();
                    A_PM.ACTION = dr["ACTION"].ToString();
                    A_PM.CREATED_DATE = Convert.ToDateTime(dr["CREATED_DATE"].ToString());

                    AR.Add(A_PM);

                }
            }
            return AR;
        }

       
        


        public List<Buldingblock> GetBlock()
        {
            List<Buldingblock> ListM = new List<Buldingblock>();
            DataTable dt = new DataTable();
            DBHelper dB = new DBHelper("SP_Master", CommandType.StoredProcedure);
            dB.addIn("@Type", "GetBuldingblock");
            //dB.addIn("@InstunceID", InstanceId);
            dt = dB.ExecuteDataTable();
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    Buldingblock M = new Buldingblock();
                    M.Block_Name = dr["Block_Name"].ToString();
                    M.block_ID = Convert.ToInt32(dr["block_ID"].ToString());

                    ListM.Add(M);
                }
            }
            return ListM;
        }

        //public question Downloadppt()
        //{
        //    //List<GetQuestionary.question> ListQuestion = new List<GetQuestionary.question>();
        //    DataTable dt = new DataTable();
        //    DBHelper dB = new DBHelper("SP_Question", CommandType.StoredProcedure);
        //    dB.addIn("@Type", "QuestionList");
        //    //dB.addIn("@Id", id);
        //    dt = dB.ExecuteDataTable();
        //    question p = new question();
        //    if (dt.Rows.Count > 0)
        //    {
        //        foreach (DataRow dr in dt.Rows)
        //        {
        //            p.id = Convert.ToInt32(dr["Id"].ToString());
        //            p.User_ID = Convert.ToInt32(dr["User_ID"].ToString());
        //            p.la_q1 = dr["la_q1"].ToString();
        //            p.la_q2 = dr["la_q2"].ToString();
        //            p.la_q3 = dr["la_q3"].ToString();
        //            p.la_q4 = dr["la_q4"].ToString();
        //            p.la_q5 = dr["la_q5"].ToString();
        //            p.la_q6 = dr["la_q6"].ToString();
        //            p.la_q7 = dr["la_q7"].ToString();
        //            p.la_q8 = dr["la_q8"].ToString();
        //            p.la_q9 = dr["la_q9"].ToString();
        //            p.la_q10 = dr["la_q10"].ToString();
        //            p.la_q11 = dr["la_q11"].ToString();
        //            p.la_q12 = dr["la_q12"].ToString();
        //            p.fq1 = dr["fq1"].ToString();
        //            p.fq2 = dr["fq2"].ToString();
        //            p.fq3 = dr["fq3"].ToString();
        //            p.submitted = Convert.ToInt32(dr["submitted"].ToString());
        //            //ListQuestion.Add(p);
        //        }
        //    }

        //    return p;
        //}

        public List<UserMaster> Sp_AssignedTo(Guid Pid)
        {
            List<UserMaster> ListM = new List<UserMaster>();
            DataTable dt = new DataTable();
            DBHelper dB = new DBHelper("SP_IssueTrack", CommandType.StoredProcedure);
            dB.addIn("@Type", "AssignedTo");
            dB.addIn("@Project_Id", Pid);
            dt = dB.ExecuteDataTable();
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    UserMaster U = new UserMaster();
                    U.UserId = Guid.Parse(dr["UserId"].ToString());
                    U.Name = dr["Name"].ToString();
                  
                    ListM.Add(U);
                }
            }
            return ListM;
        }

        public List<UserMaster> Sp_EditAssignedTo(Guid Iid)
        {
            List<UserMaster> ListM = new List<UserMaster>();
            DataTable dt = new DataTable();
            DBHelper dB = new DBHelper("SP_IssueTrack", CommandType.StoredProcedure);
            dB.addIn("@Type", "EditAssignedTo");
            dB.addIn("@Instance_Id", Iid);
            dt = dB.ExecuteDataTable();
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    UserMaster U = new UserMaster();
                    U.UserId = Guid.Parse(dr["UserId"].ToString());
                    U.Name = dr["Name"].ToString();

                    ListM.Add(U);
                }
            }
            return ListM;
        }
        public bool Sp_InsertIssue(IssueTrackModel blism)
        {
            bool Status = false;
            LogHelper _log = new LogHelper();
            try
            {
                DBHelper dB = new DBHelper("SP_IssueTrack", CommandType.StoredProcedure);
                dB.addIn("@Type", "AddIssueTrack");
                dB.addIn("@ProjectInstance_Id", blism.ProjectInstance_Id);                
                dB.addIn("@IssueName", blism.IssueName);
                dB.addIn("@PhaseID", blism.PhaseID);
                dB.addIn("@TaskId", blism.TaskId);
                dB.addIn("@StartDate", blism.StartDate);
                dB.addIn("@EndDate", blism.EndDate);
                //dB.addIn("@LastUpdatedDate", blism.LastUpdatedDate);
                dB.addIn("@AssignedTo", blism.AssignedTo);
                dB.addIn("@Status", blism.Status);
                dB.addIn("@IsApproved", false);
                dB.addIn("@Cre_By", blism.Cre_By);
                dB.addIn("@Comments", blism.Comments);
                dB.addIn("@Description", blism.Description);

                dB.ExecuteScalar();
                //Boolean s = AddIssueTrack_Mail(blism);
                //if(s)
                Status = true;
            }
            catch (Exception ex)
            {

                _log.createLog(ex, "");
            }
            return Status;
        }

        public IssueTrackModel Sp_EditIssueTrackData(Guid id)
        {
            
            LogHelper _log = new LogHelper();
           
               // IssueTrackModel ITM = new IssueTrackModel();
                DataTable dt = new DataTable();

                DBHelper dB = new DBHelper("SP_IssueTrack", CommandType.StoredProcedure);
                dB.addIn("@Type", "EditIssueTrack");
               
                dB.addIn("@Id", id);
                dt = dB.ExecuteDataTable();
             IssueTrackModel P = new IssueTrackModel();
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                       
                        P.Issuetrack_Id = Guid.Parse(dr["Issuetrack_Id"].ToString());
                        P.RunningID = Convert.ToInt32(dr["RunningID"].ToString());
                        P.IssueName = dr["IssueName"].ToString();
                        P.PhaseID = Convert.ToInt32(dr["PhaseID"].ToString());
                        P.Task = dr["Task"].ToString();
                        P.ProjectInstance_Id = Guid.Parse(dr["ProjectInstance_Id"].ToString());
                        P.StartDate = Convert.ToDateTime(dr["StartDate"].ToString());
                        P.EndDate = Convert.ToDateTime(dr["EndDate"].ToString());
                        P.LastUpdatedDate = Convert.ToDateTime(dr["LastUpdatedDate"].ToString());
                        P.AssignedTo = Guid.Parse(dr["AssignedTo"].ToString());
                        P.Status = dr["Status"].ToString();
                        P.IsApproved = Convert.ToBoolean(dr["IsApproved"].ToString());
                        //P.Comments = dr["Comments"].ToString();
                        P.Phase = dr["Phase"].ToString();
                        P.Assigned = dr["Assigned"].ToString();
                       
                        //ITM.Add(P);
                       
                    }
                }
            return P;
            //return ITM;


        }

        public bool Sp_UpdateIssue(IssueTrackModel blism)
        {
            bool Status = false;
            LogHelper _log = new LogHelper();
            try
            {
                DBHelper dB = new DBHelper("SP_IssueTrack", CommandType.StoredProcedure);
                dB.addIn("@Type", "UpdateIssueTrack");             
                dB.addIn("@Status", blism.Status);
                dB.addIn("@Modified_by", blism.Modified_by);
                dB.addIn("@Comments", blism.Comments);
                dB.addIn("@Id", blism.Issuetrack_Id);
                dB.ExecuteScalar();

                Status = true;
            }
            catch (Exception ex)
            {

                _log.createLog(ex, "");
            }
            return Status;
        }

        public List<ActivityMasterModel> Sp_GetActivityMasterData()
        {
            List<ActivityMasterModel> AMM = new List<ActivityMasterModel>();
            DataTable dt = new DataTable();

            DBHelper dB = new DBHelper("SP_Master", CommandType.StoredProcedure);
            dB.addIn("@Type", "GetActivity");
            dt = dB.ExecuteDataTable();

            if (dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    ActivityMasterModel a = new ActivityMasterModel();
                    a.Activity_ID = Convert.ToInt32(dr["Activity_ID"].ToString());
                    a.Task = dr["Task"].ToString();
                    a.BuildingBlock_id = Convert.ToInt32(dr["BuildingBlock_id"].ToString());
                    a.PhaseID = Convert.ToInt32(dr["PhaseID"].ToString());
                    a.RoleID = Convert.ToInt32(dr["RoleID"].ToString());
                    a.ApplicationAreaID = Convert.ToInt32(dr["ApplicationAreaID"].ToString());
                    a.EST_hours1 = dr["EST_hours"].ToString().Replace(".",":");
                    a.Buldingblock = dr["Buldingblock"].ToString();
                    a.Phase = dr["Phase"].ToString();
                    a.Role = dr["Role"].ToString();
                    a.ApplicationArea = dr["ApplicationArea"].ToString();

                    AMM.Add(a);
                }
            }

            return AMM;
        }

        public ActivityMasterModel Sp_GetActivityCreationById(int? id)
        {

            LogHelper _log = new LogHelper();
            DataTable dt = new DataTable();

            DBHelper dB = new DBHelper("SP_Master", CommandType.StoredProcedure);
            dB.addIn("@Type", "EditActivityCreationById");
            dB.addIn("@ID", id);
            dt = dB.ExecuteDataTable();
            ActivityMasterModel a = new ActivityMasterModel();
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                   // ActivityMasterModel a = new ActivityMasterModel();
                    a.Activity_ID = Convert.ToInt32(dr["Activity_ID"].ToString());
                    a.Task = dr["Task"].ToString();
                    a.BuildingBlock_id = Convert.ToInt32(dr["BuildingBlock_id"].ToString());
                    a.PhaseID = Convert.ToInt32(dr["PhaseID"].ToString());
                    a.RoleID = Convert.ToInt32(dr["RoleID"].ToString());
                    a.ApplicationAreaID = Convert.ToInt32(dr["ApplicationAreaID"].ToString());
                    a.EST_hours1 = dr["EST_hours"].ToString().Replace(".", ":");
                    a.Buldingblock = dr["Buldingblock"].ToString();
                    a.Phase = dr["Phase"].ToString();
                    a.Role = dr["Role"].ToString();
                    a.ApplicationArea = dr["ApplicationArea"].ToString();
                }
            }
            return a;
           
        }

       

        public List<IssueTrackModel> Sp_GetIssueTrackData()
        {
            List<IssueTrackModel> ITM = new List<IssueTrackModel>();
            DataTable dt = new DataTable();
            //if (Status)
            {
                DBHelper dB = new DBHelper("SP_IssueTrack", CommandType.StoredProcedure);
                dB.addIn("@Type", "PullData");
                //dB.addIn("@Issuetrack_Id", InstanceId);
                dt = dB.ExecuteDataTable();

                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        IssueTrackModel P = new IssueTrackModel();
                        P.Issuetrack_Id = Guid.Parse(dr["Issuetrack_Id"].ToString());
                        P.RunningID = Convert.ToInt32(dr["RunningID"].ToString());
                        P.IssueName = dr["IssueName"].ToString();
                        P.PhaseID = Convert.ToInt32(dr["PhaseID"].ToString());
                        //P.TaskId = Convert.ToInt32(dr["TaskId"].ToString());
                        P.Task = dr["Task"].ToString();
                        P.ProjectInstance_Id = Guid.Parse(dr["ProjectInstance_Id"].ToString());                        
                        P.StartDate = Convert.ToDateTime(dr["StartDate"].ToString());
                        P.EndDate = Convert.ToDateTime(dr["EndDate"].ToString());
                        P.LastUpdatedDate = Convert.ToDateTime(dr["LastUpdatedDate"].ToString());
                        P.AssignedTo = Guid.Parse(dr["AssignedTo"].ToString());
                        P.Status = dr["Status"].ToString();
                        P.IsApproved = Convert.ToBoolean(dr["IsApproved"].ToString());
                        P.Comments = dr["Comments"].ToString();
                        P.Phase = dr["Phase"].ToString();
                        P.Assigned = dr["Assigned"].ToString();
                        P.Instance = dr["Instance"].ToString();
                        P.Project = dr["Project"].ToString();
                        P.Customer = dr["Customer"].ToString();

                       // P.IssueID = dr["IssueID"].ToString();
                        if (P.PhaseID == 1)
                        {
                            P.IssueID = "Ass_" + P.RunningID;
                        }
                        if (P.PhaseID == 2)
                        {
                            P.IssueID = "Pre_" + P.RunningID;
                        }
                        if (P.PhaseID == 3)
                        {
                            P.IssueID = "Con_" + P.RunningID;
                        }
                        if (P.PhaseID == 4)
                        {
                            P.IssueID = "Post_" + P.RunningID;
                        }
                        if (P.PhaseID == 5)
                        {
                            P.IssueID = "Val_" + P.RunningID;
                        }
                        if (P.PhaseID == 0)
                        {
                            P.IssueID = "Com_" + P.RunningID;
                        }

                        ITM.Add(P);
                    }
                }
            }
            return ITM;
        }

        public bool Sp_UpdateIssueTrack(IssueTrackModel Data)
        {
            bool Status = false;
            LogHelper _log = new LogHelper();
            try
            {
                DBHelper dB = new DBHelper("SP_IssueTrack", CommandType.StoredProcedure);
                dB.addIn("@Type", "UpdateIssueTrack");
                dB.addIn("@Id", Data.Issuetrack_Id);
                dB.addIn("@Status", Data.Status);
                dB.addIn("@EndDate", Data.EndDate);
                dB.addIn("@Description", Data.Description);
                dB.addIn("@AssignedTo", Data.AssignedTo);
                dB.addIn("@Comments", Data.Comments);
                dB.addIn("@Cre_By", Data.Modified_by);

                dB.Execute();// ExecuteScalar();

                Status = true;
            }
            catch (Exception ex)
            {

                _log.createLog(ex, "");
            }

            return Status;

        }
        //public Boolean Proceess_WordAddImage()
        //{
        //    string document = @"D:\Office\Projects\ProACC\ProAccNew\ProAcc\Asset\UploadedFiles\1cb32e25-e9e7-4581-b4b3-3bf2741c58ec.docx";
        //    string fileName = @"D:\Office\Projects\ProACC\ProAccNew\ProAcc\Asset\images\resizeimage.png";
        //    InsertAPicture(document, fileName);


        //    return true;
        //}
        //public static void InsertAPicture(string document, string fileName)
        //{
        //    Document doc = new Document(document);
        //    Section section = doc.Sections[0];
        //   Documents.Paragraph para = section.AddParagraph();
        //    para.AppendPicture(Image.FromFile(fileName));
        //    doc.SaveToFile(document, FileFormat.Docx);
        //}

        //public Boolean Proceess_WordAddImage()
        //{
        //    string filePath = @"D:\Office\Projects\ProACC\ProAccNew\ProAcc\Asset\UploadedFiles\1cb32e25-e9e7-4581-b4b3-3bf2741c58ec.docx";

        //    // WordprocessingDocument WP =
        //    //WordprocessingDocument.Open(filePath, true);

        //    // using (WordprocessingDocument wordprocessingDocument =
        //    //WordprocessingDocument.Open(filePath, true))
        //    // {
        //    //     // Insert other code here.

        //    // }
        //    // AddGraph(WP, filePath);
        //    // MainDocumentPart mainPart = WordprocessingDocument.MainDocumentPart;
        //    // ImagePart imagePart = mainPart.AddImagePart(ImagePartType.Jpeg);

        //    // using (FileStream stream = new FileStream(filePath, FileMode.Open))
        //    // {
        //    //     imagePart.FeedData(stream);
        //    // }

        //    //using (WordprocessingDocument doc = WordprocessingDocument.Create(filePath, WordprocessingDocumentType.Document))

        //    //{

        //    //    //// Creates the MainDocumentPart and add it to the document (doc)

        //    //    MainDocumentPart mainPart = doc.AddMainDocumentPart();

        //    //    mainPart.Document = new Document(

        //    //        new Body(

        //    //            new Paragraph(

        //    //                new Run(

        //    //                    new Text("Hello World!!!!!")))));

        //    //}


        //    WordprocessingDocument wordprocessingDocument =
        //WordprocessingDocument.Open(filePath, true);

        //    MainDocumentPart mainPart = wordprocessingDocument.MainDocumentPart;
        //    ImagePart imagePart = mainPart.AddImagePart(ImagePartType.Jpeg);
        //    string Imagepath = @"D:\Office\Projects\ProACC\ProAccNew\ProAcc\Asset\images\resizeimage.png";
        //    //using (FileStream stream = new FileStream(Imagepath, FileMode.Open))
        //    //{
        //    //    imagePart.FeedData(stream);
        //    //}

        //    Bitmap image = new Bitmap(Imagepath);
        //    SdtElement controlBlock = doc.MainDocumentPart.HeaderParts.First().Header.Descendants<SdtElement>().Where
        //                                    (r => r.SdtProperties.GetFirstChild<Tag>().Val == tagName).SingleOrDefault();
        //    //find the Blip element of the content control
        //    Blip blip = controlBlock.Descendants<Blip>().FirstOrDefault();

        //    //add image and change embeded id
        //    ImagePart imagePart = doc.MainDocumentPart.AddImagePart(ImagePartType.Jpeg);
        //    using (MemoryStream stream = new MemoryStream())
        //    {
        //        image.Save(stream, ImageFormat.Jpeg);
        //        stream.Position = 0;
        //        imagePart.FeedData(stream);
        //    }
        //    blip.Embed = doc.MainDocumentPart.GetIdOfPart(imagePart);


        //    //AddImageToBody(wordprocessingDocument, mainPart.GetIdOfPart(imagePart));

        //    wordprocessingDocument.Close();





        //    return true;
        //}
        //private static void AddImageToBody(WordprocessingDocument wordDoc, string relationshipId)
        //{
        //    // Define the reference of the image.
        //    var element =
        //         new Drawing(
        //             new DW.Inline(
        //                 new DW.Extent() { Cx = 990000L, Cy = 792000L },
        //                 new DW.EffectExtent()
        //                 {
        //                     LeftEdge = 0L,
        //                     TopEdge = 0L,
        //                     RightEdge = 0L,
        //                     BottomEdge = 0L
        //                 },
        //                 new DW.DocProperties()
        //                 {
        //                     Id = (UInt32Value)1U,
        //                     Name = "Picture 1"
        //                 },
        //                 new DW.NonVisualGraphicFrameDrawingProperties(
        //                     new A.GraphicFrameLocks() { NoChangeAspect = true }),
        //                 new A.Graphic(
        //                     new A.GraphicData(
        //                         new PIC.Picture(
        //                             new PIC.NonVisualPictureProperties(
        //                                 new PIC.NonVisualDrawingProperties()
        //                                 {
        //                                     Id = (UInt32Value)0U,
        //                                     Name = "New Bitmap Image.jpg"
        //                                 },
        //                                 new PIC.NonVisualPictureDrawingProperties()),
        //                             new PIC.BlipFill(
        //                                 new A.Blip(
        //                                     new A.BlipExtensionList(
        //                                         new A.BlipExtension()
        //                                         {
        //                                             Uri =
        //                                                "{28A0092B-C50C-407E-A947-70E740481C1C}"
        //                                         })
        //                                 )
        //                                 {
        //                                     Embed = relationshipId,
        //                                     CompressionState =
        //                                     A.BlipCompressionValues.Print
        //                                 },
        //                                 new A.Stretch(
        //                                     new A.FillRectangle())),
        //                             new PIC.ShapeProperties(
        //                                 new A.Transform2D(
        //                                     new A.Offset() { X = 0L, Y = 0L },
        //                                     new A.Extents() { Cx = 990000L, Cy = 792000L }),
        //                                 new A.PresetGeometry(
        //                                     new A.AdjustValueList()
        //                                 )
        //                                 { Preset = A.ShapeTypeValues.Rectangle }))
        //                     )
        //                     { Uri = "http://schemas.openxmlformats.org/drawingml/2006/picture" })
        //             )
        //             {
        //                 DistanceFromTop = (UInt32Value)0U,
        //                 DistanceFromBottom = (UInt32Value)0U,
        //                 DistanceFromLeft = (UInt32Value)0U,
        //                 DistanceFromRight = (UInt32Value)0U,
        //                 EditId = "50D07946"
        //             });

        //    // Append the reference to body, the element should be in a Run.
        //    wordDoc.MainDocumentPart.Document.Body.AppendChild(new Paragraph(new Run(element)));
        //}
        //private string AddGraph(WordprocessingDocument wpd, string filepath)
        //{
        //    ImagePart ip = wpd.MainDocumentPart.AddImagePart(ImagePartType.Jpeg);
        //    string Imagepath= @"D:\Office\Projects\ProACC\ProAccNew\ProAcc\Asset\images\resizeimage.png";
        //    using (FileStream fs = new FileStream(Imagepath, FileMode.Open))
        //    {
        //        if (fs.Length == 0) return string.Empty;
        //        ip.FeedData(fs);
        //    }

        //    return wpd.MainDocumentPart.GetIdOfPart(ip);
        //}

        public List<PMTaskMonitor_> GetPMTask(String IDProject)
        {
            List<PMTaskMonitor_> ListM = new List<PMTaskMonitor_>();
            DataTable dt = new DataTable();
            DBHelper dB = new DBHelper("SP_PMTask", CommandType.StoredProcedure);
            dB.addIn("@Type", "GetPMTask");
            dB.addIn("@ProjectID", IDProject);
            //dB.addIn("@InstunceID", InstanceId);
            dt = dB.ExecuteDataTable();
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    PMTaskMonitor_ M = new PMTaskMonitor_();
                    M.Id = Guid.Parse(dr["id"].ToString());
                    M.PMTaskID = Guid.Parse(dr["PMTaskID"].ToString());
                    M.ProjectId = Guid.Parse(dr["ProjectId"].ToString());
                    //M.Delay_occurred = Convert.ToBoolean(dr["Delay_occurred"].ToString());
                    M.StatusId = Convert.ToInt32(dr["StatusId"].ToString());
                    M.EST_hours = Convert.ToDouble(dr["EST_hours"].ToString());
                    M.Actual_St_hours = Convert.ToDouble(dr["Actual_St_hours"].ToString());

                    M.Task_Other_Environment = Convert.ToBoolean(dr["Task_Other_Environment"].ToString());

                    M.Planed__St_Date = Convert.ToDateTime(dr["Planed__St_Date"].ToString());
                    M.Planed__En_Date = Convert.ToDateTime(dr["Planed__En_Date"].ToString());
                    M.Actual_St_Date = Convert.ToDateTime(dr["Actual_St_Date"].ToString());
                    M.Actual_En_Date = Convert.ToDateTime(dr["Actual_En_Date"].ToString());

                    M.Notes = Convert.ToString(dr["Notes"].ToString());


                    ListM.Add(M);
                }
            }
            return ListM;
        }

        #region Mail

        public Boolean UpdateMailList(int ID)
        {
            Boolean Res = false;
            LogHelper _log = new LogHelper();
            try
            {
                DBHelper dB = new DBHelper("SP_Mail", CommandType.StoredProcedure);
            dB.addIn("@Type", "UpdateMailList");
            dB.addIn("@Running_ID", ID);
            dB.ExecuteScalar();
            Res = true;
            }
            catch (Exception ex)
            {

                _log.createLog(ex, "-->UpdateMailList"+ex.Message.ToString());
            }
            return Res;
        }
        public Boolean AddQuestionnaire_Mail(int User_ID, string MailID)
        {
            Boolean Status = false;

            LogHelper _log = new LogHelper();
            try
            {
                DBHelper dB = new DBHelper("SP_Mail", CommandType.StoredProcedure);
                dB.addIn("@Type", "Questionnaire");
                dB.addIn("@Q_Mail", MailID);
                dB.addIn("@Q_UserID", User_ID);
                dB.addIn("@Cre_By", "00000000-0000-0000-0000-000000000000");
                dB.ExecuteScalar();

                Status = true;
            }
            catch (Exception ex)
            {

                _log.createLog(ex, "-->AddQuestionnaire_Mail" + ex.Message.ToString());
            }

            return Status;
        }

        private Boolean UpdateMonitor_Mail(ProjectMonitorModel PM, int PhaseId)
        {
            Boolean Status = false;

            LogHelper _log = new LogHelper();
            try
            {
                DBHelper Db1 = new DBHelper("SP_Mail", CommandType.StoredProcedure);
                Db1.addIn("@Type", "ProjectMonitor");
                Db1.addIn("@PhaseID", PhaseId);
                Db1.addIn("@InstanceID", PM.Instance);
                Db1.addIn("@PM_ID", PM.Id);
                Db1.addIn("@Cre_By", PM.Cre_By);
                DataTable dt = new DataTable();
                dt = Db1.ExecuteDataTable();

                Status = true;
            }
            catch (Exception ex)
            {

                _log.createLog(ex, "-->AddQuestionnaire_Mail" + ex.Message.ToString());
            }

            return Status;
        }
        public List<MailModel> GetMailList()
        {
            List<MailModel> ListM = new List<MailModel>();
            DataTable dt = new DataTable();
            DBHelper dB = new DBHelper("SP_Mail", CommandType.StoredProcedure);
            dB.addIn("@Type", "MailList");
            //dB.addIn("@InstunceID", InstanceId);
            dt = dB.ExecuteDataTable();
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    MailModel M = new MailModel();
                    M.Name = dr["Name"].ToString();
                    M.Running_ID = Convert.ToInt32(dr["Running_ID"].ToString());
                    M.To = Convert.ToString(dr["TO"].ToString());
                    M.Subject = dr["Subject"].ToString();
                    M.Body = dr["Body"].ToString();
                    M.TemplateName = dr["FileName"].ToString();
                    M.Q_UserID = Convert.ToInt32(dr["Q_UserID"].ToString());
                    ListM.Add(M);
                }
            }
            return ListM;
        }

        public Boolean sendmailtocustomer(String Name,String Email)
        {
            Boolean Status = false;

            LogHelper _log = new LogHelper();
            try
            {
                DBHelper dB = new DBHelper("SP_Mail", CommandType.StoredProcedure);
                dB.addIn("@Type", "SendCustomerMail");
                dB.addIn("@Q_Name", Name);
                dB.addIn("@Q_Mail", Email);                
                dB.addIn("@Cre_By", "00000000-0000-0000-0000-000000000000");
                dB.ExecuteScalar();

                Status = true;
            }
            catch (Exception ex)
            {
                _log.createLog(ex, "-->AddIssueTrack_Mail" + ex.Message.ToString());
            }

            return Status;
        }


        //public Boolean AddIssueTrack_Mail(IssueTrackModel ITM)
        //{
        //    Boolean Status = false;

        //    LogHelper _log = new LogHelper();
        //    try
        //    {
        //        DBHelper dB = new DBHelper("SP_Mail", CommandType.StoredProcedure);
        //        dB.addIn("@Type", "IssueTrack");
        //        //dB.addIn("@Q_Mail", MailID);
        //        dB.addIn("@UserID", ITM.AssignedTo);
        //        //dB.addIn("@Issuetrack_Id", ITM.Issuetrack_Id);
        //        dB.addIn("@Cre_By", "00000000-0000-0000-0000-000000000000");
        //        dB.ExecuteScalar();

        //        Status = true;
        //    }
        //    catch (Exception ex)
        //    {
        //        _log.createLog(ex, "-->AddIssueTrack_Mail" + ex.Message.ToString());
        //    }

        //    return Status;
        //}
        #endregion


        public string Phase_Assessment = "Assessment";
        public string Phase_PreConversion = "Pre Conversion";
        public string Phase_Conversion = "Conversion";
        public string Phase_PostConversion = "Post Conversion";
        public string Phase_Validation = "Validation Testing";
        public string SAPReportFileName = "SAPReadinessCheck";

        public void FileDelete(string Path)
        {
            if (File.Exists(Path))
            {
                // If file found, delete it    
                File.Delete(Path);
            }
        }

        //public Boolean FileExist(string path)
        //{
        //    StatusMaster 

        //}



    }
}