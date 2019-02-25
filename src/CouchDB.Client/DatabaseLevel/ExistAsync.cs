using System.Threading.Tasks;

namespace CouchDB.Client
{
    public partial class CouchDatabase
    {
        /// <summary>
        /// Returns the HTTP Headers containing a minimal amount of information about the specified document. The method supports the same query arguments as the GET /{db}/{docid} method, but only the header information (including document size, and the revision as an ETag), is returned
        /// <see cref="http://docs.couchdb.org/en/2.2.0/api/document/common.html#db-doc"/>
        /// </summary>
        /// <param name="id">Document ID</param>
        /// <returns></returns>
        public async Task<CouchResponse> ExistAsync(string id)
        {
            var request = new RestSharp.RestRequest(Helper.EncodeID(id), RestSharp.Method.HEAD);
            return await client.http.ExecuteAsync(request);
        }
    }
}
