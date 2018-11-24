using Newtonsoft.Json.Linq;
using RestSharp;
using System.Threading.Tasks;

namespace CouchDB.Client
{
    public partial class CouchDatabase
    {
        /// <summary>
        /// http://docs.couchdb.org/en/2.1.2/api/document/common.html#put--db-docid
        /// </summary>
        /// <param name="json"></param>
        /// <param name="batchMode">http://docs.couchdb.org/en/2.1.2/api/database/common.html#batch-mode-writes</param>
        /// <returns></returns>
        public async Task<CouchResponse> InsertRevisionAsync(JToken json, string id, bool batchMode = false)
        {
            var request = new RestSharp.RestRequest(Helper.EncodeID(id), RestSharp.Method.PUT);
            if (batchMode)
                request.AddQueryParameter("batch", "ok");

            request.AddParameter("application/json", json, ParameterType.RequestBody);
            return await client.http.ExecuteAsync(request);
        }
    }
}
