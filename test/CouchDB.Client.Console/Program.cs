using System;
using System.Text;
using Newtonsoft.Json.Linq;

namespace CouchDB.Client.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            var cs = CouchDB.Client.Helper.GetConnectionString();
            var client = new CouchClient(cs);
            var database = client.GetDatabase("esl");

            try
            {
                var fileTemp = System.IO.Path.GetTempFileName();
                database.DumpAsync(fileTemp);

                System.IO.FileInfo file = new System.IO.FileInfo(fileTemp);

                
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