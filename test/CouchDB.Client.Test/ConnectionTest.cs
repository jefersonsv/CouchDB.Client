using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CouchDB.Client.Test
{
    [TestClass]
    public class ConnectionTest
    {
        CouchClient client;
        CouchDatabase database;

        [TestInitialize]
        public void TestMethod1()
        {
            var cs = CouchDB.Client.Helper.GetConnectionString();
            client = new CouchClient(cs);
        }

        [TestMethod]
        public void Dump()
        {
            database = client.GetDatabase("esl");

            var fileTemp = System.IO.Path.GetTempFileName();
            database.DumpAsync(fileTemp);

            System.IO.FileInfo file = new System.IO.FileInfo(fileTemp);
            Assert.IsTrue(file.Length > 0);
        }
    }
}
