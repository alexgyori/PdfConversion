using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PdfConversion.Server.DataService;
using PdfConversionService.API;

namespace PdfConversion.Server.StatusService.Tests
{
    [TestClass]
    public class UnitTestStatusService
    {
        [TestMethod]
        public void TestConversionServiceNullResponseFromDataService()
        {
            ConversionService serv = new ConversionService();
            using (Microsoft.QualityTools.Testing.Fakes.ShimsContext.Create())
            {
                PdfConversion.Server.DataService.Fakes.ShimFileStatusRepositoryFactory.GetRepository = () =>
                {
                    var shim = new PdfConversion.Server.DataService.Fakes.StubIRepository<FileStatusEntity>();
                    //override saveChanges with NO-OP to protect the database;    
                    shim.SaveChanges = () => { };
                    shim.SearchForExpressionOfFuncOfT0Boolean = a => null;
                    
                    return shim;
                };                
                
                var status = serv.GetFileState("test");
                Assert.IsTrue(status == PdfConversionService.API.FileState.Missing);
            }
        }

        [TestMethod]
        public void TestConversionServiceExceptionResponseFromDataService()
        {
            ConversionService serv = new ConversionService();
            using (Microsoft.QualityTools.Testing.Fakes.ShimsContext.Create())
            {
                PdfConversion.Server.DataService.Fakes.ShimFileStatusRepositoryFactory.GetRepository = () =>
                {
                    var shim = new PdfConversion.Server.DataService.Fakes.StubIRepository<FileStatusEntity>();
                    //override saveChanges with NO-OP to protect the database;    
                    shim.SaveChanges = () => { };
                    shim.SearchForExpressionOfFuncOfT0Boolean = a => { throw new Exception(); };

                    return shim;
                };                

                var status = serv.GetFileState("test");
                Assert.IsTrue(status == PdfConversionService.API.FileState.Missing);
            }
        }

        [TestMethod]
        public void TestConversionServiceQueuedResponseFromDataService()
        {
            ConversionService serv = new ConversionService();
            using (Microsoft.QualityTools.Testing.Fakes.ShimsContext.Create())
            {
                PdfConversion.Server.DataService.Fakes.ShimFileStatusRepositoryFactory.GetRepository = () =>
                {
                    var shim = new PdfConversion.Server.DataService.Fakes.StubIRepository<FileStatusEntity>();
                    
                    shim.SaveChanges = () => { };
                    shim.SearchForExpressionOfFuncOfT0Boolean = a => { 
                        var ls = new List<FileStatusEntity>();
                        ls.Add(new FileStatusEntity());
                        return ls.AsQueryable<FileStatusEntity>();
                    };

                    return shim;
                };

                var status = serv.GetFileState("test");
                Assert.IsTrue(status == PdfConversionService.API.FileState.Queued);
            }
        }

        [TestMethod]
        public void TestConversionServiceErroneousResponseFromDataService()
        {
            ConversionService serv = new ConversionService();
            using (Microsoft.QualityTools.Testing.Fakes.ShimsContext.Create())
            {
                PdfConversion.Server.DataService.Fakes.ShimFileStatusRepositoryFactory.GetRepository = () =>
                {
                    var shim = new PdfConversion.Server.DataService.Fakes.StubIRepository<FileStatusEntity>();
                    
                    shim.SaveChanges = () => { };
                    shim.SearchForExpressionOfFuncOfT0Boolean = a =>
                    {
                        var ls = new List<FileStatusEntity>();
                        var entity = new FileStatusEntity();
                        entity.FileState = FileState.Erroneous;
                        ls.Add(entity);
                        return ls.AsQueryable<FileStatusEntity>();
                    };

                    return shim;
                };

                var status = serv.GetFileState("test");
                Assert.IsTrue(status == PdfConversionService.API.FileState.Erroneous);
            }
        }

        [TestMethod]
        public void TestConversionServiceConvertingResponseFromDataService()
        {
            ConversionService serv = new ConversionService();
            using (Microsoft.QualityTools.Testing.Fakes.ShimsContext.Create())
            {
                PdfConversion.Server.DataService.Fakes.ShimFileStatusRepositoryFactory.GetRepository = () =>
                {
                    var shim = new PdfConversion.Server.DataService.Fakes.StubIRepository<FileStatusEntity>();
                    
                    shim.SaveChanges = () => { };
                    shim.SearchForExpressionOfFuncOfT0Boolean = a =>
                    {
                        var ls = new List<FileStatusEntity>();
                        var entity = new FileStatusEntity();
                        entity.FileState = FileState.Converting;
                        ls.Add(entity);
                        return ls.AsQueryable<FileStatusEntity>();
                    };

                    return shim;
                };

                var status = serv.GetFileState("test");
                Assert.IsTrue(status == PdfConversionService.API.FileState.Converting);
            }
        }

        [TestMethod]
        public void TestConversionServiceSomeListResponseFromDataService()
        {
            ConversionService serv = new ConversionService();
            using (Microsoft.QualityTools.Testing.Fakes.ShimsContext.Create())
            {
                PdfConversion.Server.DataService.Fakes.ShimFileStatusRepositoryFactory.GetRepository = () =>
                {
                    var shim = new PdfConversion.Server.DataService.Fakes.StubIRepository<FileStatusEntity>();
                    
                    shim.SaveChanges = () => { };
                    shim.GetAll = () =>
                    {
                        var ls = new List<FileStatusEntity>();
                        var entity = new FileStatusEntity();
                        entity.FileState = FileState.Converting;
                        ls.Add(entity);
                        ls.Add(new FileStatusEntity());
                        return ls.AsQueryable<FileStatusEntity>();
                    };

                    return shim;
                };

                var results = serv.GetAvailableFiles();
                Assert.IsTrue(results.Count() == 2);
            }
        }

        [TestMethod]
        public void TestConversionServiceProcessingResponseFromDataService()
        {
            ConversionService serv = new ConversionService();
            using (Microsoft.QualityTools.Testing.Fakes.ShimsContext.Create())
            {
                PdfConversion.Server.DataService.Fakes.ShimFileStatusRepositoryFactory.GetRepository = () =>
                {
                    var shim = new PdfConversion.Server.DataService.Fakes.StubIRepository<FileStatusEntity>();
    
                    shim.SaveChanges = () => { };
                    shim.SearchForExpressionOfFuncOfT0Boolean = a =>
                    {
                        var ls = new List<FileStatusEntity>();
                        var entity = new FileStatusEntity();
                        entity.FileState = FileState.Converting;
                        ls.Add(entity);
                        return ls.AsQueryable<FileStatusEntity>();
                    };

                    return shim;
                };

                var status = serv.GetServerStatus();
                Assert.IsTrue(status == PdfConversionService.API.ServerState.Processing);
            }
        }

        [TestMethod]
        public void TestConversionServiceOfflineNullResponseFromDataService()
        {
            ConversionService serv = new ConversionService();
            using (Microsoft.QualityTools.Testing.Fakes.ShimsContext.Create())
            {
                PdfConversion.Server.DataService.Fakes.ShimFileStatusRepositoryFactory.GetRepository = () =>
                {
                    var shim = new PdfConversion.Server.DataService.Fakes.StubIRepository<FileStatusEntity>();

                    shim.SaveChanges = () => { };
                    shim.SearchForExpressionOfFuncOfT0Boolean = a => null;

                    return shim;
                };

                var status = serv.GetServerStatus();
                Assert.IsTrue(status == PdfConversionService.API.ServerState.Offline);
            }
        }


        [TestMethod]
        public void TestConversionServiceWaitingResponseFromDataService()
        {
            ConversionService serv = new ConversionService();
            using (Microsoft.QualityTools.Testing.Fakes.ShimsContext.Create())
            {
                PdfConversion.Server.DataService.Fakes.ShimFileStatusRepositoryFactory.GetRepository = () =>
                {
                    var shim = new PdfConversion.Server.DataService.Fakes.StubIRepository<FileStatusEntity>();

                    shim.SaveChanges = () => { };
                    shim.SearchForExpressionOfFuncOfT0Boolean = a =>
                    {
                        var ls = new List<FileStatusEntity>();                        
                        return ls.AsQueryable<FileStatusEntity>();
                    };

                    return shim;
                };

                var status = serv.GetServerStatus();
                Assert.IsTrue(status == PdfConversionService.API.ServerState.Waiting);
            }
        }
    }
}
