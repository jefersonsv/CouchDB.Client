using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CouchDB.Client
{
    public partial class CouchDatabase
    {
        /// <summary>
        /// http://docs.couchdb.org/en/2.0.0/api/ddoc/common.html
        /// </summary>
        /// <param name="id"></param>
        /// <param name="rev"></param>
        /// <returns></returns>
        public async Task<CouchResponse> DeleteAsync(string id, string rev)
        {
            var request = new RestSharp.RestRequest(id, RestSharp.Method.DELETE);
            request.AddQueryParameter("rev", rev);
            return await client.http.ExecuteAsync(request);
        }
    }
}
