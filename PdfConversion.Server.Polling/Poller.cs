using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.IO;
using PdfConversion.Server.DataService;
using PdfConversionService.API;
using PdfConversion.Server.Converter;
using System.Threading.Tasks;

namespace PdfConversion.Server.Polling
{
    public class PollerFactory
    {
        public static IPoller GetPoller(string sharedFolder, string tmpFolder, int numOfThreads, string title, string author)
        {
            return new Poller(sharedFolder, tmpFolder, numOfThreads, title, author);
        }
    }

    public interface IPoller
    {
        void Poll();
    }
    class Poller : IPoller
    {
        private string tmpFolder;
        private string poolingFolder;
        private string outputFolder;
        private string author;
        private string title;

        private int activeThreadsInPool = 0;
        public Poller(string sharedFolder, string tmpFolder, int numOfThreads, string title, string author)
        {
            this.title = title;
            this.author = author;

            //Configure threadpool
            ThreadPool.SetMaxThreads(numOfThreads, numOfThreads);
            //configure blackboard(shared folders)
            this.poolingFolder = System.IO.Path.Combine(sharedFolder, "in");
            this.outputFolder = System.IO.Path.Combine(sharedFolder, "out");
            this.createIfNotExists(this.poolingFolder);
            this.createIfNotExists(this.outputFolder);
            //configure temp folder
            this.tmpFolder = tmpFolder;
            this.createIfNotExists(this.tmpFolder);
        }

        private void createIfNotExists(string p)
        {
            if (!System.IO.Directory.Exists(p))
            {
                System.IO.Directory.CreateDirectory(p);
            }
        }

        public void Poll()
        {
            //Poll constantly
            while (true)
            {
                try
                {
                    //Get the files in current directory
                    //When there is nothing to do, the directory should be empty
                    string[] files = Directory.GetFiles(poolingFolder);
                    //Iterate over each file in the input directory,
                    //then move it to a temporary working directory
                    foreach (var file in files)
                    {
                        
                        //Make temporary filename
                        String tempFile = this.MakeTempFile(file);

                        //Moving a file is cheap(no bitwise copy happens, only fast fileSystem operation)
                        //Caveat: the temporary folder should be in the same partition with the input folder, so that moving is cheap
                        //otherwise this operation can be very slow for large files.                                       
                        //If another process holds the file then IOException will
                        //be thrown. It will retry to move it later. 
                        //(This exception is thrown sometimes when the file is not yet completely copied by the OS)
                        try
                        {
                            System.IO.File.Move(file, tempFile);
                        }
                        catch (IOException)
                        {
                            continue;
                        }

                        InsertFileStatusToDatabase(file, tempFile);

                        //Enqueue to thread pool; the thread pul is setup so that it only has
                        //as many threads as it is allowed in the configuration file
                        ImageToPdfConverter converter = new ImageToPdfConverter(this.outputFolder, title, author);
                        //Use Interlocked to guarantee threadsafe manipulation of the field
                        //the field holds the number of queued work items.                        
                        Interlocked.Increment(ref this.activeThreadsInPool);

                        ThreadPool.QueueUserWorkItem((f) => { try { converter.Convert((String)f); } finally { Interlocked.Decrement(ref this.activeThreadsInPool); } }, tempFile);


                    }
                    

                    Thread.Sleep(1000);
                }
                catch (ThreadInterruptedException)
                {
                    //Waits for work in threadpool to be done.
                    while (this.activeThreadsInPool > 0) ;

                    return;
                }
            }
        }

        private void InsertFileStatusToDatabase(string file, String tempFile)
        {
            //Prepare an entity
            FileStatusEntity entity = new FileStatusEntity(file.Substring(file.LastIndexOf("\\") + 1), tempFile.Substring(tempFile.LastIndexOf("\\") + 1));
            //Connect to db and insert the entity to keep track of the work being done.
            using (var repo = FileStatusRepositoryFactory.GetRepository())
            {
                repo.Insert(entity);
                repo.SaveChanges();
            }
        }
        /// <summary>
        /// Create a path for the temporary file coresponding
        /// to the given parameter.
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        private string MakeTempFile(string file)
        {
            String fileName = file.Substring(file.LastIndexOf("\\") + 1);
            fileName = Guid.NewGuid().ToString().Substring(0, 8) + fileName;
            fileName = System.IO.Path.Combine(this.tmpFolder, fileName);
            return fileName;
        }

    }
}
