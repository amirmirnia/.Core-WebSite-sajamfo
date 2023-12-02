using Spire.Doc;
using Spire.Doc.Documents;
using Word = Microsoft.Office.Interop.Word;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Spire.Pdf;
using Data.TMU.Model.msc;
using System.Drawing;
using Aspose.Words;
using ceTe.DynamicPDF.Conversion;
using Aspose.Words.Saving;
using GroupDocs.Viewer;
using GroupDocs.Viewer.Options;

namespace Core.TMU.FileSite
{
    public class SpireWord
    {
        public static bool insertWhitBookMark(string path, string oldnamefileword, string newnamefileWord, List<string> namebookmark, List<string> Text)
        {
            try
            {
                string Firstpath;
                if (File.Exists(Path.Combine(Directory.GetCurrentDirectory(), path, newnamefileWord)))
                {
                    Firstpath = Path.Combine(Directory.GetCurrentDirectory(), path,
                   newnamefileWord);
                }
                else
                {
                    Firstpath = Path.Combine(Directory.GetCurrentDirectory(), path,
                   oldnamefileword);
                }
                string Endpath = Path.Combine(Directory.GetCurrentDirectory(), path,
                    newnamefileWord);
                //Create a Document instance
                Spire.Doc.Document doc = new Spire.Doc.Document();
                //Load the Word document
                doc.LoadFromFile(Firstpath);


                //Create a BookmarksNavigator instance
                BookmarksNavigator navigator = new BookmarksNavigator(doc);
                int i = 0;
                foreach (var item in namebookmark)
                {

                    //Move to the bookmark named Test
                    navigator.MoveToBookmark(item);
                    Spire.Doc.Documents.Paragraph p = new Spire.Doc.Documents.Paragraph(doc);

                    var t = p.AppendText(Text[i]);
                    t.CharacterFormat.TextColor = Color.Black;
                    t.CharacterFormat.FontSize = 11;
                    t.CharacterFormat.BoldBidi = true;
                    //t.CharacterFormat.FontName = "B Nazanin";

                    //t.CharacterFormat.FontName = "Calibri";
                    //Create a TextBodyPart instance
                    navigator.InsertParagraph(p);
                    i++;
                }
                doc.SaveToFile(Endpath);
                return true;
            }
            catch (Exception)
            {

                return false;
            }
        }
        //public static bool insertWhitBookMark(string path, string oldnamefileword, string newnamefileWord, List<string> Bookmark, List<TextBookMark> Text)
        //{
        //    try
        //    {
        //        string Firstpath = Path.Combine(Directory.GetCurrentDirectory(), path,
        //            oldnamefileword);
        //        string Endpath = Path.Combine(Directory.GetCurrentDirectory(), path,
        //            newnamefileWord);
        //        //Create a Document instance
        //        Document doc = new Document();
        //        //Load the Word document
        //        doc.LoadFromFile(Firstpath);
        //        //Create a BookmarksNavigator instance
        //        BookmarksNavigator navigator = new BookmarksNavigator(doc);


        //        int i = 0;
        //        foreach (var item in Text)
        //        {
        //            //Move to the bookmark named Test
        //            navigator.MoveToBookmark(Bookmark[i]);
        //            Paragraph p = new Paragraph(doc);
        //            p.AppendText(item);
        //            //Create a TextBodyPart instance
        //            navigator.InsertParagraph(p);
        //            i++;
        //        }

        //        doc.SaveToFile(Endpath);
        //        return true;
        //    }
        //    catch (Exception)
        //    {

        //        return false;
        //    }
        //}
        public static void Print(string path, string namefileWord)
        {
            //string Pathfile = Path.Combine(Directory.GetCurrentDirectory(), path,
            //        namefileWord);
            //Create a Document instance
            //Document doc = new Document();
            //Load the Word document
            //doc.LoadFromFile(Pathfile);
            //System.Drawing.Printing.PrintDocument printDoc = doc.PrintDocument;
            //Print the document
            //printDoc.Print();
        }
        public static bool ConvertToPdf(string path, string Savepath, string namefile)
        {
            try
            {
                string Pathfile = Path.Combine(Directory.GetCurrentDirectory(), path,
                  namefile + ".docx");
                //Document doc = new Document();
                //doc.LoadFromFile(Pathfile);
                //ToPdfParameterList parameters = new ToPdfParameterList();
                //doc.SaveToFile(Path.Combine(Directory.GetCurrentDirectory(), Savepath) + namefile + ".pdf", parameters);
                Aspose.Words.Document doc = new Aspose.Words.Document(Pathfile);

                //WordConversionOptions options = new WordConversionOptions(false);
                //options.Author = "John Doe";
                //options.TopMargin = 144;
                //options.BottomMargin = 72;
                //options.LeftMargin = 72;
                //options.RightMargin = 72;
                //WordConverter wordDocConverter = new WordConverter(Pathfile, options);
                //wordDocConverter.Convert(Path.Combine(Directory.GetCurrentDirectory(), Savepath) + namefile + ".pdf");



                //dynamic pdf
                //Converter.ConvertAsync(Pathfile, Path.Combine(Directory.GetCurrentDirectory(), Savepath) + namefile + ".pdf");

                //ImageSaveOptions options = new ImageSaveOptions(SaveFormat.Jpeg);
                //doc.Save(Path.Combine(Directory.GetCurrentDirectory(), Savepath) + namefile + ".jpg", options);


                var converter = new GroupDocs.Conversion.Converter(Pathfile);
                // Prepare conversion options for target format PDF
                var convertOptions = converter.GetPossibleConversions()["pdf"].ConvertOptions;
                // Convert to PDF format
                converter.Convert(Path.Combine(Directory.GetCurrentDirectory(), Savepath) + namefile + ".pdf", convertOptions);

                //GroupDocs.Viewer
                //using (Viewer viewer = new Viewer(Pathfile))
                //{
                //    PdfViewOptions options = new PdfViewOptions(Path.Combine(Directory.GetCurrentDirectory(), Savepath) + namefile + ".pdf");
                //    viewer.View(options);
                //}

                //aspose
                //Aspose.Words.Document doc = new Aspose.Words.Document(Pathfile);
                //doc.Save(Path.Combine(Directory.GetCurrentDirectory(), Savepath) + namefile + ".pdf", SaveFormat.Pdf);
                return true;
            }
            catch (Exception)
            {
                return false;
                throw;
            }
            //PdfDocument docc = new PdfDocument(@"wwwroot\Status-face\Pdf\" + namefileWord + ".docx.pdf");

            ////Print the document with the default printer 
            //docc.Print();
            //System.Diagnostics.Process.Start(Path.Combine(Directory.GetCurrentDirectory(), Savepath) + namefileWord + ".pdf");
        }
        public static bool IsExists(string FileName, string path)
        {

            if (path != null && FileName != null)
            {
                string pathstring;
                if (FileName != "non")
                {
                    pathstring = Path.Combine(Directory.GetCurrentDirectory(), path, FileName);
                    if (File.Exists(pathstring))
                    {
                        return true;
                    }

                }
                return false;
            }
            else
            {
                return false;
            }
        }
    }
}
