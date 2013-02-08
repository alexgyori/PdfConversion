using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PdfConversion.Server.DataService;

namespace PdfConversion.Server.Converter.Tests
{
    [TestClass]
    public class UnitTestPdfConverter
    {

        [TestMethod]
        public void Test_ConvertFilesInResources()
        {
            //Using shims(a form of stubs) to protect the database from inconsistencies
            //Not using them would make some changes to the database that would create inconsistencies.
            using (Microsoft.QualityTools.Testing.Fakes.ShimsContext.Create())
            {
                //Override GetRepository factory method with my own stub of the IRepository interface
                ShimTheDatabase();
                foreach (var file in Directory.GetFiles("./../../resultsTest1"))
                {
                    File.Delete(file);
                }

                //TEST
                IConverter converter = new ImageToPdfConverter("./../../resultsTest1", "TestConv", "Alex Gyori");
                foreach (var file in Directory.GetFiles("./../../resourcesTest1"))
                {
                    Assert.IsTrue(converter.Convert(file));
                }
                Assert.AreEqual(Directory.GetFiles("./../../resultsTest1").Length, Directory.GetFiles("./../../resourcesTest1").Length);
            }
        }

        [TestMethod]
        public void Test_ConvertNullFile()
        {
            using (Microsoft.QualityTools.Testing.Fakes.ShimsContext.Create())
            {
                IConverter conv = new ImageToPdfConverter("./../resultsTest1", "Test2", "Alex Gyori");
                ShimTheDatabase();
                Assert.IsFalse(conv.Convert(null));
            }
        }

        [TestMethod]
        public void Test_ConvertToNonExistingFolderTarget()
        {
            using (Microsoft.QualityTools.Testing.Fakes.ShimsContext.Create())
            {
                IConverter conv = new ImageToPdfConverter("./../../resultsTest2", "Test2", "Alex Gyori");
                ShimTheDatabase();
                if (Directory.Exists("./../../resultsTest2"))
                {
                    Directory.Delete("./../../resultsTest2", true);
                }
                Assert.IsTrue(conv.Convert("./../../resourcesTest1/FotoCanon.jpg"));
                Assert.IsTrue(Directory.Exists("./../../resultsTest2"));
                Assert.IsTrue(Directory.GetFiles("./../../resultsTest2").Length == 1);
            }
        }

        private static void ShimTheDatabase()
        {
            PdfConversion.Server.DataService.Fakes.ShimFileStatusRepositoryFactory.GetRepository = () =>
            {
                var shim = new PdfConversion.Server.DataService.Fakes.StubIRepository<FileStatusEntity>();
                //override saveChanges with NO-OP to protect the database;    
                shim.SaveChanges = () => { };

                shim.SearchForExpressionOfFuncOfT0Boolean = a => null;
                shim.InsertT0 = entity => { };
                return shim;
            };
        }

        [TestMethod]
        public void Test_ConvertNonImageFile()
        {
            using (Microsoft.QualityTools.Testing.Fakes.ShimsContext.Create())
            {
                ShimTheDatabase();
                IConverter conv = new ImageToPdfConverter("./../../resultsTest2", "Test2", "Alex Gyori");
                Assert.IsFalse(conv.Convert("./../../resources/bla.jpg"));
            }
        }

    }
}
