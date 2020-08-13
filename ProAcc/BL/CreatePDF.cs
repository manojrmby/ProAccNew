using Spire.Doc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using Word = Microsoft.Office.Interop.Word;
namespace ProAcc.BL
{
   
    public class CreatePDF
    {
        LogHelper _Log = new LogHelper();
        Base @base = new Base();
        //public Boolean SpireconvertDOCtoPDF(string PathDoc, string Pathpdf)
        //{
        //    Document document = new Document();
        //    Boolean Status = false; 
        //    try
        //    {
        //        if (File.Exists(PathDoc))
        //        {
        //            document.LoadFromFile(PathDoc);
        //            FileDelete(Pathpdf);
        //            document.SaveToFile(Pathpdf, FileFormat.PDF);
        //            Status = true;
        //        }
        //        else
        //        {
        //            Status = false;
        //        }


        //        //SautinSoft.UseOffice u = new SautinSoft.UseOffice();

        //        //if (u.InitWord() == 0)

        //        //{
        //        //    string path = Path.GetDirectoryName(Pathpdf);
        //        //    string filename = Path.GetFileNameWithoutExtension(Pathpdf);
        //        //    //convert Word (RTF, DOC, DOCX to PDF)

        //        //    u.ConvertFile(PathDoc, @""+Pathpdf, SautinSoft.UseOffice.eDirection.DOC_to_PDF);

        //        //}

        //        //u.CloseOffice();

        //        //Word2Pdf objWorPdf = new Word2Pdf();
        //        //string backfolder1 = "D:\\WOrdToPDF\\";
        //        //string strFileName = "TestFile.docx";
        //        //object FromLocation = backfolder1 + "\\" + strFileName;
        //        //string FileExtension = Path.GetExtension(strFileName);
        //        //string ChangeExtension = strFileName.Replace(FileExtension, ".pdf");
        //        //if (FileExtension == ".doc" || FileExtension == ".docx")
        //        //{
        //        //    object ToLocation = backfolder1 + "\\" + ChangeExtension;
        //        //    objWorPdf.InputLocation = PathDoc;
        //        //    objWorPdf.OutputLocation = Pathpdf;
        //        //    //Status = true;
        //        //    objWorPdf.Word2PdfCOnversion();
        //        //}




        //    }
        //    catch (Exception Ex)
        //    {
        //        Status = false;
        //        throw;
        //    }
        //    return Status;

        //}
        public Boolean convertDOCtoPDF(string PathDoc, string Pathpdf)
        {
            Boolean Status = false;
            var WORD = new Word.Application();
            try
            {

                _Log.createLog("---IN--");


                object misValue = System.Reflection.Missing.Value;
                //String PATH_APP_PDF = @"D:\Office\Projects\ProACC\ProAccNew\ProAcc\Asset\UploadedFiles\Temp\678bda2f-c281-48c1-b565-64f3773d3b1f.pdf";
                string currentDirectory = Path.GetDirectoryName(Pathpdf);
                if (!Directory.Exists(currentDirectory))
                {
                    Directory.CreateDirectory(currentDirectory);
                }
                _Log.createLog("---Before Delete--");
                @base.FileDelete(Pathpdf);

                _Log.createLog("---Before doc--");
                //Word.Document doc = WORD.Documents.Open(@"D:\Office\Projects\ProACC\ProAccNew\ProAcc\Asset\UploadedFiles\678bda2f-c281-48c1-b565-64f3773d3b1f.docx");
                Word.Document doc = WORD.Documents.Open(PathDoc);
                _Log.createLog("---Before Activate--");
                doc.Activate();
                _Log.createLog("---Before SaveAs2--");
                doc.SaveAs2(@Pathpdf, Word.WdSaveFormat.wdFormatPDF, misValue, misValue, misValue,
                misValue, misValue, misValue, misValue, misValue, misValue, misValue);
                _Log.createLog("---Before Close--");
                doc.Close();
                WORD.Quit();


                releaseObject(doc);
                releaseObject(WORD);
                Status = true;
            }
            catch (Exception ex)
            {
                _Log.createLog(ex, "-->convertDOCtoPDF" + ex.Message.ToString());
                
                WORD.Quit();
                Status = false;
                throw;
            }
            return Status;

        }
            private void releaseObject(object obj)
        {
            try
            {
                System.Runtime.InteropServices.Marshal.ReleaseComObject(obj);
                obj = null;
            }
            catch (Exception ex)
            {
                //TODO
            }
            finally
            {
                GC.Collect();
            }
        }

        
    }
}