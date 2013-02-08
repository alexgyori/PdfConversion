using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PdfConversion.Server.DataService;

namespace PdfConversion.Server.Polling.Tests
{
    [TestClass]
    public class UnitTestWorkResumer
    {
        [TestMethod]
        public void TestSomeWorkToResume()
        {
           
            using (Microsoft.QualityTools.Testing.Fakes.ShimsContext.Create())
            {
                int counterDelete = 0;
                int counterSaveC = 0;
                PdfConversion.Server.DataService.Fakes.ShimFileStatusRepositoryFactory.GetRepository = () =>
                {
                    var shim = new PdfConversion.Server.DataService.Fakes.StubIRepository<FileStatusEntity>();

                    shim.SaveChanges = () => { counterSaveC++; };
                    shim.DeleteT0 = ent => { counterDelete++; };
                    shim.SearchForExpressionOfFuncOfT0Boolean = a =>
                    {
                        var ls = new List<FileStatusEntity>();
                        for(int i =0;i<10;i++)
                            ls.Add(new FileStatusEntity());
                        return ls.AsQueryable<FileStatusEntity>();
                    };
                    return shim;
                };
                int counterMoves = 0;
                System.IO.Fakes.ShimFile.MoveStringString = (a, b) => { counterMoves++; };
                WorkResumer resumer = new WorkResumer("folfer", "temp", "title", "author");
                resumer.Resume();
                Assert.IsTrue(counterMoves == 10);
                Assert.IsTrue(counterDelete == 10);
                Assert.IsTrue(counterSaveC == 1);
            }
        }


        [TestMethod]
        public void TestNoWorkToResume()
        {

            using (Microsoft.QualityTools.Testing.Fakes.ShimsContext.Create())
            {
                int counterDelete = 0;
                int counterSaveC = 0;
                PdfConversion.Server.DataService.Fakes.ShimFileStatusRepositoryFactory.GetRepository = () =>
                {
                    var shim = new PdfConversion.Server.DataService.Fakes.StubIRepository<FileStatusEntity>();

                    shim.SaveChanges = () => { counterSaveC++; };
                    shim.DeleteT0 = ent => { counterDelete++; };
                    shim.SearchForExpressionOfFuncOfT0Boolean = a =>
                    {
                        var ls = new List<FileStatusEntity>();                       
                        return ls.AsQueryable<FileStatusEntity>();
                    };
                    return shim;
                };
                int counterMoves = 0;
                System.IO.Fakes.ShimFile.MoveStringString = (a, b) => { counterMoves++; };
                WorkResumer resumer = new WorkResumer("folfer", "temp", "title", "author");
                resumer.Resume();
                Assert.IsTrue(counterMoves == 0);
                Assert.IsTrue(counterDelete == 0);
                Assert.IsTrue(counterSaveC == 0);
            }
        }

        [TestMethod]
        public void TestSomeWorkToResumeWithIOException()
        {

            using (Microsoft.QualityTools.Testing.Fakes.ShimsContext.Create())
            {
                int counterDelete = 0;
                int counterSaveC = 0;
                PdfConversion.Server.DataService.Fakes.ShimFileStatusRepositoryFactory.GetRepository = () =>
                {
                    var shim = new PdfConversion.Server.DataService.Fakes.StubIRepository<FileStatusEntity>();

                    shim.SaveChanges = () => { counterSaveC++; };
                    shim.DeleteT0 = ent => { counterDelete++; };
                    shim.SearchForExpressionOfFuncOfT0Boolean = a =>
                    {
                        var ls = new List<FileStatusEntity>();
                        for (int i = 0; i < 10; i++)
                            ls.Add(new FileStatusEntity());
                        return ls.AsQueryable<FileStatusEntity>();
                    };
                    return shim;
                };
                int counterMoves = 0;
                System.IO.Fakes.ShimFile.MoveStringString = (a, b) => { counterMoves++; throw new IOException(); };
                WorkResumer resumer = new WorkResumer("folfer", "temp", "title", "author");
                resumer.Resume();
                Assert.IsTrue(counterMoves == 10);
                Assert.IsTrue(counterDelete == 10);
                Assert.IsTrue(counterSaveC == 1);
            }
        }



    
    }
}
