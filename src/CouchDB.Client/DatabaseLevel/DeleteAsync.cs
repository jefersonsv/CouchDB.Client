using System.Threading.Tasks;

namespace CouchDB.Client
{
    public partial class CouchDatabase
    {
        /// <summary>
        /// http://docs.couchdb.org/en/2.1.2/api/document/common.html#delete--db-docid
        /// </summary>
        /// <param name="id"></param>
        /// <param name="rev"></param>
        /// <param name="batchMode">http://docs.couchdb.org/en/2.1.2/api/database/common.html#batch-mode-writes</param>
        /// <returns></returns>
        public async Task<CouchResponse> DeleteAsync(string id, string rev, bool batchMode = false)
        {
            var request = new RestSharp.RestRequest(Helper.EncodeID(id), RestSharp.Method.DELETE);
            request.AddQueryParameter("rev", rev);
            if (batchMode)
                request.AddQueryParameter("batch", "ok");

            return await client.http.ExecuteAsync(request);
        }
    }
}
