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
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="showRevisions">http://docs.couchdb.org/en/2.1.2/api/document/common.html#obtaining-an-extended-revision-history</param>
        /// <returns></returns>
        public async Task<CouchResponse> GetAsync(string id, bool showRevisions = false)
        {
            var request = new RestSharp.RestRequest(Helper.EncodeID(id), RestSharp.Method.GET);
            if (showRevisions)
                request.AddQueryParameter("revs_info", "true");

            return await client.http.ExecuteAsync(request);
        }
    }
}
