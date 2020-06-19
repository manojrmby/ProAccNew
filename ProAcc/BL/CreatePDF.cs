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
        public void convertDOCtoPDF(string PathDoc,string Pathpdf)
        {

            object misValue = System.Reflection.Missing.Value;
            //String PATH_APP_PDF = @"D:\Office\Projects\ProACC\ProAccNew\ProAcc\Asset\UploadedFiles\Temp\678bda2f-c281-48c1-b565-64f3773d3b1f.pdf";
            String PATH_APP_PDF = Pathpdf;
            FileDelete(Pathpdf);
           var WORD = new Word.Application();

            //Word.Document doc = WORD.Documents.Open(@"D:\Office\Projects\ProACC\ProAccNew\ProAcc\Asset\UploadedFiles\678bda2f-c281-48c1-b565-64f3773d3b1f.docx");
            Word.Document doc = WORD.Documents.Open(PathDoc);
            doc.Activate();

            doc.SaveAs2(@PATH_APP_PDF, Word.WdSaveFormat.wdFormatPDF, misValue, misValue, misValue,
            misValue, misValue, misValue, misValue, misValue, misValue, misValue);

            doc.Close();
            WORD.Quit();


            releaseObject(doc);
            releaseObject(WORD);

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