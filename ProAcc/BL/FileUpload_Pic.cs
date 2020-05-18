using Spire.Xls;
using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Linq;
using System.Web;

namespace ProAcc.BL
{
    public class FileUpload_Pic
    {
        #region PreConvertion
        public Boolean Pre_Process_PreConvertion(string FilePath, string fileName, Guid Instance_ID,string Temp_Path,Guid User_Id)
        {
            Boolean Status = false;
            FileUpload fileUpload = new FileUpload();
            try
            {
                Workbook workbook = new Workbook();
                workbook.LoadFromFile(FilePath);
                Worksheet sheet = workbook.Worksheets[0];
                var count = workbook.Worksheets.Count + 1;
                Worksheet Newsheet = workbook.Worksheets.Add("NewSheet" + count);
                
                List<Pic>pic_Lists = new List<Pic>();
                for (int i = 0; i < sheet.Pictures.Count; i++)
                {
                    var a = sheet.Pictures[i];
                    Pic p = new Pic();
                    p.Name = a.Name;
                    p.Row = a.BottomRow;
                    p.Col = a.LeftColumn;
                    sheet.Range[p.Row, p.Col].Text = p.Name;
                    pic_Lists.Add(p);
                }
                for (int i = sheet.Pictures.Count - 1; i >= 0; i--)
                {
                    sheet.Pictures[i].Remove();
                }
                
                string FileName = FilePath.Substring(FilePath.LastIndexOf('\\') +1);
                string Temp_Save = Temp_Path + "\\" + FileName;
                workbook.SaveToFile(Temp_Save, ExcelVersion.Version2016);
                Status = fileUpload.Process_PreConvertion(Temp_Save, FileName, Instance_ID, User_Id);
            }
            catch (Exception ex)
            {

                string error = ex.Message;
            }

          

             return Status;
        }


       

        #endregion
        private class Pic
        {
            public string Name { get; set; }
            public int Row { get; set; }
            public int Col { get; set; }
        }
    }

    
}