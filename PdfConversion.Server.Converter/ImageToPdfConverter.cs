using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MigraDoc.DocumentObjectModel;
using MigraDoc.Rendering;
using System.Diagnostics;
using MigraDoc.DocumentObjectModel.Shapes;
using PdfConversion.Server.DataService;
using PdfConversionService.API;
using System.IO;

namespace PdfConversion.Server.Converter
{
    public interface IConverter
    {       
        Boolean Convert(string filePath);
    }

    public class ImageToPdfConverter : IConverter
    {

        public ImageToPdfConverter(string outputFolder, string title, string author)
        {
            this.outputFolder = outputFolder;
            this.titlePrefix = title;
            this.author = author;
        }
        
        private Document document;
        private string outputFolder;
        private string titlePrefix;
        private string author;


        public bool Convert(string filePath)
        {
            try
            {
                ChangeFileStateToConverting(filePath);
                Image theImage = BuildCleanDocument(filePath);
                //Handle different image sizes and take care of image appearence(scaling)
                this.setUpImage(theImage);
                String outputFile = RenderDocument(filePath);
                ChangeFileStateToConverted(filePath, outputFile);
                return true;
            }
            catch
            {
                if(filePath!=null)
                    ChangeFileStateToErroneous(filePath);
                return false;
            }
        }

        

        private Image BuildCleanDocument(string filePath)
        {
            this.document = new Document();
            this.document.Info.Title = this.titlePrefix + Path.GetFileName(filePath);
            this.document.Info.Author = this.author;
            Section imgSection = this.document.AddSection();
            Image theImage = imgSection.AddImage(filePath);
            return theImage;
        }

        private string RenderDocument(string filePath)
        {

            PdfDocumentRenderer pdfRenderer = new PdfDocumentRenderer();
            pdfRenderer.Document = this.document;
            pdfRenderer.RenderDocument();
            String outputFile = outputFolder + "\\" + this.titlePrefix + Path.GetFileName(filePath) + ".pdf";
            if (!Directory.Exists(outputFolder))
            {
                Directory.CreateDirectory(outputFolder);
            }
            pdfRenderer.Save(outputFile);
            return outputFile;
        }

        private void ChangeFileStateToConverted(string filePath, String outputPdfFile)
        {
            this.ChangeFileStatusTemplateMethod(filePath,FileState.Converted, (FileStatusEntity entity) => { entity.ConvertedFileName = Path.GetFileName( outputPdfFile); });
        }

        private void ChangeFileStateToConverting(string filePath)
        {
            this.ChangeFileStatusTemplateMethod(filePath, FileState.Converting,(FileStatusEntity ent) => { /*No op needed in this case*/});
        }

        private void ChangeFileStateToErroneous(string filePath)
        {
            this.ChangeFileStatusTemplateMethod(filePath, FileState.Erroneous, (ent) => { });
        }

        //To avoid code duplication, introduced a template method that takes a lambda expression as a hook.
        private void ChangeFileStatusTemplateMethod(string filePath, FileState state, Action<FileStatusEntity> f)
        {
            using (var repo = FileStatusRepositoryFactory.GetRepository())
            {
                var fn = filePath.Substring(filePath.LastIndexOf("\\") + 1);
                var entities = repo.SearchFor(entity => entity.PhysicalFileName.Equals(fn));
                FileStatusEntity ent = null;
                if(entities !=null)
                     ent = entities.FirstOrDefault();                
                if (ent == null)
                {
                    ent = new FileStatusEntity("Some error ocurred", filePath);
                    repo.Insert(ent);
                }
                ent.FileState = state;
                f(ent);
                repo.SaveChanges();
            }
        }

        private Unit pageWidth = new Unit(14,UnitType.Centimeter);
        private Unit pageHeight = new Unit(25, UnitType.Centimeter);


        /// <summary>
        /// This method should scale the image only if needed in order to make it fit on the page.
        /// This does not work as intended because the library API does not provide real Height and Width.
        /// Find workaround if time allows it.
        /// </summary>
        /// <param name="img"></param>
        private void setUpImage(Image img)
        {
            
            /* This code does not work due to the lib.
            if (img.Width > pageWidth)
            {
                if (img.Height > pageHeight)
                {
                    if (img.Width / pageWidth > img.Height / pageHeight)
                        img.Width = pageWidth;
                    else
                        img.Height = pageHeight;
                }
                else
                    img.Width = pageWidth;
            }
            else if (img.Height > pageHeight)
                img.Height = pageHeight;
            */
            //These two lines should be removed
            //they scale improperly and also when not needed
            //TODO: Find better workadound.
            img.Height = pageHeight;
            img.Width = pageWidth;

            img.Top = ShapePosition.Top;
            img.Left = ShapePosition.Left;
            img.RelativeHorizontal = RelativeHorizontal.Margin;
            img.RelativeVertical = RelativeVertical.Margin;
            img.LockAspectRatio = true;
            img.WrapFormat.Style = WrapStyle.Through;
        }

    }
}
