using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PdfConversionService.API;
using System.Data.Entity;
using System.Runtime.Serialization;

namespace PdfConversion.Server.DataService
{
   
    public class FileStatusEntity
    {
        public FileStatusEntity()
        {
        }

        public FileStatusEntity(string FileName, string PhysicalFileName)
        {
            this.Id = Guid.NewGuid();
            this.FileState = FileState.Queued;
            this.FileName = FileName;
            this.PhysicalFileName = PhysicalFileName;
            this.ConvertedFileName = null;
        }
        
        public Guid Id { get; set; }
        
        public String FileName { get; set; }
        
        public String PhysicalFileName { get; set; }
        
        public FileState FileState { get; set; }        
        
        public String ConvertedFileName { get; set; }
    }

    public class FileStatusContext : DbContext
    {
        DbSet<FileStatusEntity> FileStatusEntities { get; set; }
    }
}
