using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using PdfConversionService.API;
using PdfConversion.Server.DataService;

namespace PdfConversion.Server.StatusService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IService1" in both code and config file together.
    [ServiceContract]
    public interface IConversionService
    {        
        [OperationContract]
        FileState GetFileState(string fileName);

        [OperationContract]
        IEnumerable<FileStatus> GetAvailableFiles();

        [OperationContract]
        ServerState GetServerStatus();
    }
    [DataContract]
    public class FileStatus
    {
        [DataMember]
        public String FileName { get; set; }

        [DataMember]
        public String PhysicalFileName { get; set; }
        
        [DataMember]
        public FileState FileState { get; set; }

        [DataMember]
        public String ConvertedFileName { get; set; }

        public override String ToString()
        {
            return FileName;
        }
    }

}
