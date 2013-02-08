using System;
using System.IO;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PdfConversion.Server.DataService;

namespace PdfConversion.Server.Polling.Tests
{
    [TestClass]
    public class UnitTestPoller
    {
        [TestMethod]
        public void TestDeposit5Files()
        {
            IPoller p = PollerFactory.GetPoller(@".\..\..\shared", @".\..\..\tmp", 10, "testTitle", "Alex Gyori");
            using (Microsoft.QualityTools.Testing.Fakes.ShimsContext.Create())
            {
                int i = 0;
                PdfConversion.Server.Converter.Fakes.ShimImageToPdfConverter.AllInstances.ConvertString
                    = (a, f) => { i++; return true; };
                int time = 0;
                System.IO.Fakes.ShimDirectory.GetFilesString =
                    f =>
                    {
                        if (time == 0)
                        {
                            time++;
                            return new String[5] { "a", "b", "c", "d", "e" };
                        }
                        else
                        {
                            time++;
                            return null;
                        }
                    };
                System.IO.Fakes.ShimFile.MoveStringString = (a, b) => { };
                PdfConversion.Server.DataService.Fakes.ShimFileStatusRepositoryFactory.GetRepository =
                    () =>
                    {
                        var repoStub = new PdfConversion.Server.DataService.Fakes.StubIRepository<FileStatusEntity>();
                        repoStub.InsertT0 = ent => { };
                        repoStub.SaveChanges = () => { };
                        return repoStub;
                    };

                Thread pollingThread = new Thread(p.Poll);
                pollingThread.Start();
                Thread.Sleep(2000);
                pollingThread.Interrupt();
                Assert.IsTrue(i == 5);

            }
        }

        [TestMethod]
        public void TestThrowIoExceptionGetFiles()
        {
            IPoller p = PollerFactory.GetPoller(@".\..\..\shared", @".\..\..\tmp", 10, "testTitle", "Alex Gyori");
            using (Microsoft.QualityTools.Testing.Fakes.ShimsContext.Create())
            {
                int i = 0;
                PdfConversion.Server.Converter.Fakes.ShimImageToPdfConverter.AllInstances.ConvertString
                    = (a, f) => { i++; return true; };                
                System.IO.Fakes.ShimDirectory.GetFilesString = str => { throw new IOException(); };                   
                System.IO.Fakes.ShimFile.MoveStringString = (a, b) => { };
                PdfConversion.Server.DataService.Fakes.ShimFileStatusRepositoryFactory.GetRepository =
                    () =>
                    {
                        var repoStub = new PdfConversion.Server.DataService.Fakes.StubIRepository<FileStatusEntity>();
                        repoStub.InsertT0 = ent => { };
                        repoStub.SaveChanges = () => { };
                        return repoStub;
                    };

                Thread pollingThread = new Thread(p.Poll);
                pollingThread.Start();
                Thread.Sleep(2000);
                pollingThread.Interrupt();
                Assert.IsTrue(i == 0);


            }
        }

        [TestMethod]
        public void TestThrowIoExceptionMoveFile()
        {
            IPoller p = PollerFactory.GetPoller(@".\..\..\shared", @".\..\..\tmp", 10, "testTitle", "Alex Gyori");
            using (Microsoft.QualityTools.Testing.Fakes.ShimsContext.Create())
            {
                int i = 0;
                PdfConversion.Server.Converter.Fakes.ShimImageToPdfConverter.AllInstances.ConvertString
                    = (a, f) => { i++; return true; };
                System.IO.Fakes.ShimDirectory.GetFilesString = str => new String[5] { "a", "b", "c", "d", "e" };
                System.IO.Fakes.ShimFile.MoveStringString = (a, b) => { throw new IOException(); };
                PdfConversion.Server.DataService.Fakes.ShimFileStatusRepositoryFactory.GetRepository =
                    () =>
                    {
                        var repoStub = new PdfConversion.Server.DataService.Fakes.StubIRepository<FileStatusEntity>();
                        repoStub.InsertT0 = ent => { };
                        repoStub.SaveChanges = () => { };
                        return repoStub;
                    };

                Thread pollingThread = new Thread(p.Poll);
                pollingThread.Start();
                Thread.Sleep(2000);
                pollingThread.Interrupt();
                Assert.IsTrue(i == 0);


            }
        }

        [TestMethod]
        public void TestThrowExceptionAtInsert()
        {
            IPoller p = PollerFactory.GetPoller(@".\..\..\shared", @".\..\..\tmp", 10, "testTitle", "Alex Gyori");
            using (Microsoft.QualityTools.Testing.Fakes.ShimsContext.Create())
            {
                int i = 0;
                PdfConversion.Server.Converter.Fakes.ShimImageToPdfConverter.AllInstances.ConvertString
                    = (a, f) => { i++; return true; };
                System.IO.Fakes.ShimDirectory.GetFilesString = str => new String[5] { "a", "b", "c", "d", "e" };
                System.IO.Fakes.ShimFile.MoveStringString = (a, b) => { };
                PdfConversion.Server.DataService.Fakes.ShimFileStatusRepositoryFactory.GetRepository =
                    () =>
                    {
                        var repoStub = new PdfConversion.Server.DataService.Fakes.StubIRepository<FileStatusEntity>();
                        repoStub.InsertT0 = ent => { throw new Exception(); };
                        repoStub.SaveChanges = () => { };
                        return repoStub;
                    };

                Thread pollingThread = new Thread(p.Poll);
                pollingThread.Start();
                Thread.Sleep(2000);
                pollingThread.Interrupt();
                Assert.IsTrue(i == 0);


            }
        }

        [TestMethod]
        public void TestThrowExceptionAtSave()
        {
            IPoller p = PollerFactory.GetPoller(@".\..\..\shared", @".\..\..\tmp", 10, "testTitle", "Alex Gyori");
            using (Microsoft.QualityTools.Testing.Fakes.ShimsContext.Create())
            {
                int i = 0;
                PdfConversion.Server.Converter.Fakes.ShimImageToPdfConverter.AllInstances.ConvertString
                    = (a, f) => { i++; return true; };
                System.IO.Fakes.ShimDirectory.GetFilesString = str => new String[5] { "a", "b", "c", "d", "e" };
                System.IO.Fakes.ShimFile.MoveStringString = (a, b) => { };
                PdfConversion.Server.DataService.Fakes.ShimFileStatusRepositoryFactory.GetRepository =
                    () =>
                    {
                        var repoStub = new PdfConversion.Server.DataService.Fakes.StubIRepository<FileStatusEntity>();
                        repoStub.InsertT0 = ent => {  };
                        repoStub.SaveChanges = () => { throw new Exception(); };
                        return repoStub;
                    };

                Thread pollingThread = new Thread(p.Poll);
                pollingThread.Start();
                Thread.Sleep(2000);
                pollingThread.Interrupt();
                Assert.IsTrue(i == 0);


            }
        }
    }
}
