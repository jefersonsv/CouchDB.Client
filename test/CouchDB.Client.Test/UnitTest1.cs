using System;
using Xunit;

namespace CouchDB.Client.Test
{
    public class UnitTest1
    {
        [Fact]
        public void JsonValidationTest()
        {
            Assert.True(CouchDB.Client.Helper.IsValidJson("{a: 'oi'}"));
            Assert.False(CouchDB.Client.Helper.IsValidJson("sdfsdf"));
        }
    }
}
