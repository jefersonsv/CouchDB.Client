using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace CouchDB.Client
{
    public partial class CouchDatabase
    {
        /// <summary>
        /// http://www.greenacorn-websolutions.com/couchdb/export-import-a-database-with-couchdb.php
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async void DumpAsync(string destinyFileName)
        {
            using (var stream = File.Create(destinyFileName))
            {
                var request = new RestSharp.RestRequest("_all_docs", RestSharp.Method.GET);
                request.AddQueryParameter("include_docs", "true");
                request.AddQueryParameter("attachments", "true");
                request.ResponseWriter = w => w.CopyTo(stream);

                await client.http.RestExecuteAsync(request);
            }
        }
    }
}