using Spire.Doc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;


//using Word = Microsoft.Office.Interop.Word;
namespace ProAcc.BL
{
    public class CreatePDF
    {
        public Boolean SpireconvertDOCtoPDF(string PathDoc, string Pathpdf)
        {
            Document document = new Document();
            Boolean Status = false; 
            try
            {
                if (File.Exists(PathDoc))
                {
                    document.LoadFromFile(PathDoc);
                    FileDelete(Pathpdf);
                    document.SaveToFile(Pathpdf, FileFormat.PDF);
                    Status = true;
                }
                else
                {
                    Status = false;
                }


                //SautinSoft.UseOffice u = new SautinSoft.UseOffice();

                //if (u.InitWord() == 0)

                //{
                //    string path = Path.GetDirectoryName(Pathpdf);
                //    string filename = Path.GetFileNameWithoutExtension(Pathpdf);
                //    //convert Word (RTF, DOC, DOCX to PDF)

                //    u.ConvertFile(PathDoc, @""+Pathpdf, SautinSoft.UseOffice.eDirection.DOC_to_PDF);

                //}

                //u.CloseOffice();

                //Word2Pdf objWorPdf = new Word2Pdf();
                //string backfolder1 = "D:\\WOrdToPDF\\";
                //string strFileName = "TestFile.docx";
                //object FromLocation = backfolder1 + "\\" + strFileName;
                //string FileExtension = Path.GetExtension(strFileName);
                //string ChangeExtension = strFileName.Replace(FileExtension, ".pdf");
                //if (FileExtension == ".doc" || FileExtension == ".docx")
                //{
                //    object ToLocation = backfolder1 + "\\" + ChangeExtension;
                //    objWorPdf.InputLocation = PathDoc;
                //    objWorPdf.OutputLocation = Pathpdf;
                //    //Status = true;
                //    objWorPdf.Word2PdfCOnversion();
                //}




            }
            catch (Exception Ex)
            {
                Status = false;
                throw;
            }
            return Status;
           
        }
        //public void convertDOCtoPDF(string PathDoc,string Pathpdf)
        //{

        //    object misValue = System.Reflection.Missing.Value;
        //    //String PATH_APP_PDF = @"D:\Office\Projects\ProACC\ProAccNew\ProAcc\Asset\UploadedFiles\Temp\678bda2f-c281-48c1-b565-64f3773d3b1f.pdf";
        //    String PATH_APP_PDF = Pathpdf;
        //    FileDelete(Pathpdf);
        //   var WORD = new Word.Application();

        //    //Word.Document doc = WORD.Documents.Open(@"D:\Office\Projects\ProACC\ProAccNew\ProAcc\Asset\UploadedFiles\678bda2f-c281-48c1-b565-64f3773d3b1f.docx");
        //    Word.Document doc = WORD.Documents.Open(PathDoc);
        //    doc.Activate();

        //    doc.SaveAs2(@PATH_APP_PDF, Word.WdSaveFormat.wdFormatPDF, misValue, misValue, misValue,
        //    misValue, misValue, misValue, misValue, misValue, misValue, misValue);

        //    doc.Close();
        //    WORD.Quit();


        //    releaseObject(doc);
        //    releaseObject(WORD);

        //}
        //private void releaseObject(object obj)
        //{
        //    try
        //    {
        //        System.Runtime.InteropServices.Marshal.ReleaseComObject(obj);
        //        obj = null;
        //    }
        //    catch (Exception ex)
        //    {
        //        //TODO
        //    }
        //    finally
        //    {
        //        GC.Collect();
        //    }
        //}

        private void FileDelete(string Path)
        {
            if (File.Exists(Path))
            {
                // If file found, delete it    
                File.Delete(Path);
            }
        }
    }
}