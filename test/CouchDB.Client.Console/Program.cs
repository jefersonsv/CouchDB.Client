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

            // create document
            var doc = new
            {
                machine = Environment.MachineName,
                year = DateTime.Now.Year
            };

            var db = client.GetDatabase("test");
            var insert = await db.InsertAsync(doc);

            // get document by id
            var getdoc = await db.GetAsync(insert.Id);

            // select all
            var five = await db.SelectAsync(5);

            try
            {
                var all = await db.SelectAsync(new FindBuilder().Limit(1));

                //var fileTemp = System.IO.Path.GetTempFileName();
                //database.DumpAsync(fileTemp);
                //await database.SelectAsync(null)

                //System.IO.FileInfo file = new System.IO.FileInfo(fileTemp);


            }
            catch (Exception ex)
            {

            }



            //var cs = "http://username:password@server:5984/";
            //var client = new CouchClient(cs);
            //var db = client.GetDatabase("forever-note");
            //var config = db.GetAsync("config").Result;

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