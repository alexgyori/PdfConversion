using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using PdfConversionService.API;
using PdfConversion.Server.DataService;

namespace PdfConversion.Server.StatusService
{    
    public sealed class ConversionService : IConversionService
    {
        public FileState GetFileState(string fileName)
        {
            using (var repo = FileStatusRepositoryFactory.GetRepository())
            {
                IQueryable<FileStatusEntity> results;
                try
                {
                    results = repo.SearchFor(entity => entity.FileName.Equals(fileName));
                }
                catch
                {
                    return FileState.Missing;
                }
                if (results != null)
                {
                    var ent = results.FirstOrDefault();
                    if (ent != null)
                    {
                        return ent.FileState;
                    }
                    else
                    {
                        return FileState.Missing;
                    }
                }
                return FileState.Missing;

            }
        }       

        

        public ServerState GetServerStatus()
        {
            using (var repo = FileStatusRepositoryFactory.GetRepository())
            {
                var workingOn = repo.SearchFor(entity => entity.FileState != FileState.Converted 
                    && entity.FileState!=FileState.Erroneous);
                if (workingOn != null)
                {
                    if (workingOn.Count() == 0)
                    {
                        return ServerState.Waiting;
                    }
                    else
                    {
                        return ServerState.Processing;
                    }
                }
                else
                    //if this is null, something is wrong :)
                    //EF should not return null, Data service should not return null.
                    return ServerState.Offline;
            }
        }





        public IEnumerable<FileStatus> GetAvailableFiles()
        {
       
            try
            {
                List<FileStatusEntity> ls;
                using (var repo = FileStatusRepositoryFactory.GetRepository())
                {
                    ls = repo.GetAll().ToList();

                }
                ConcurrentBag<FileStatus> results = new ConcurrentBag<FileStatus>();
                //This can be done in parallel
                Parallel.ForEach<FileStatusEntity>(ls, ent =>
                {
                    var element = new FileStatus();
                    element.ConvertedFileName = ent.ConvertedFileName;
                    element.FileName = ent.FileName;
                    element.FileState = ent.FileState;
                    element.PhysicalFileName = ent.PhysicalFileName;
                    results.Add(element);
                });
                return results;
            }
            //A more fine grained approach should be taken but maybe later.
            catch
            {
                return new List<FileStatus>();
            }
        
        }
    }
}
