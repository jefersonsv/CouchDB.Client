using System.Threading.Tasks;

namespace CouchDB.Client
{
    public partial class CouchDatabase
    {
        /// <summary>
        /// http://docs.couchdb.org/en/2.2.0/api/document/common.html#obtaining-an-extended-revision-history
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<CouchResponse> GetRevisionInfoAsync(string id)
        {
            var request = new RestSharp.RestRequest(Helper.EncodeID(id), RestSharp.Method.GET);
            request.AddQueryParameter("revs_info", "true");
            return await client.http.ExecuteAsync(request);
        }
    }
}
