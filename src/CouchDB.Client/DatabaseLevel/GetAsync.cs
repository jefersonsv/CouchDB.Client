using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CouchDB.Client
{
    public partial class CouchDatabase
    {
        public async Task<CouchResponse> GetAsync(string id, bool showRevisions = false)
        {
            var request = new RestSharp.RestRequest(Helper.EncodeID(id), RestSharp.Method.GET);
            if (showRevisions)
                request.AddQueryParameter("revs_info", "true");

            return await client.http.ExecuteAsync(request);
        }
    }
}
