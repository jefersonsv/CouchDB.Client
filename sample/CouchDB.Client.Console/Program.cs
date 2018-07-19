using System;
using System.Text;
using System.Threading.Tasks;
using CouchDB.Client.FluentMango;
using Newtonsoft.Json.Linq;

namespace CouchDB.Client.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            RunAsync().Wait();
        }

        public static async Task RunAsync()
        {
            var cs = CouchDB.Client.Helper.GetConnectionString();
            var client = new CouchClient(cs);

            // get database
            var db = await client.GetDatabaseAsync("test");

            var all = await db.SelectAsync();


            //var jo = new
            //{
            //    teste = "asdasd",
            //    numero = 123
            //};

            //var r = db.GetRevisionInfoAsync("d7836e68-eb2e-4182-a5a5-641ccc5f8a20").Result;
            //StringBuilder sb = new StringBuilder();
            //foreach (var item in r.Revisions)
            //{
            //    var ra = db.GetRevisionAsync(r.Id, item.Revision).Result;
            //    sb.AppendLine(ra.Json.GetString("html"));
            //}


            //var j = db.InsertAsync(jo);


            //var j2 = db.ForceDeleteAsync(j.Result.Id).Result;

        }
    }
}