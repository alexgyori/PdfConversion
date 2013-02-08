using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using PdfConversion.Server.Converter;
using PdfConversion.Server.DataService;
using PdfConversionService.API;

namespace PdfConversion.Server.Polling
{

    public class WorkResumer
    {
        private string author;
        private string title;
        private string tempFOlder;
        private string sharedFolder;

        public WorkResumer(String sharedFolder, String tempFolder, String title, String author)
        {
            this.sharedFolder = sharedFolder;
            this.tempFOlder = tempFolder;
            this.title = title;
            this.author = author;
        }

        public void Resume()
        {


            using(var repo = FileStatusRepositoryFactory.GetRepository())
            {
                var toConvert = repo.SearchFor(ent => ent.FileState != FileState.Converted && ent.FileState != FileState.Erroneous);
                if (toConvert.Count() != 0)
                {
                    Console.WriteLine(@"Recovering...");
                    Console.WriteLine(@"The application was not closed properly last time.");
                    Console.WriteLine(@"Some unconverted files might have remained in inconsistent states.");
                    Console.WriteLine(@"Please wait...");


                    foreach (var ent in toConvert)
                    {
                        Console.Write(".");
                        var source = this.tempFOlder + ent.PhysicalFileName;
                        var dest = this.sharedFolder + "\\in\\" + ent.FileName;
                        try
                        {
                            File.Move(source, dest);
                        }
                        catch (IOException ex)
                        {
                            Console.WriteLine(ex);
                        }
                        repo.Delete(ent);
                    }
                    repo.SaveChanges();
                    Console.WriteLine();
                }
            }
        }
        
    }
}
