using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CouchDB.Client
{
    public partial class CouchDatabase
    {
        public async Task<CouchResponse> GetAsync(string id)
        {
            var request = new RestSharp.RestRequest(Helper.EncodeID(id), RestSharp.Method.GET);
            return await client.http.ExecuteAsync(request);
        }

        public async Task<CouchResponse> GetAsync(JToken json)
        {
            var request = new RestSharp.RestRequest(Helper.EncodeID(json.GetString("_id")), RestSharp.Method.GET);
            return await client.http.ExecuteAsync(request);
        }
    }
}
