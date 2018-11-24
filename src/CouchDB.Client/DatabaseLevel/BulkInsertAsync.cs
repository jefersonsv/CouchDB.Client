using CouchDB.Client.FluentMango;
using RestSharp;
using System.Threading.Tasks;

namespace CouchDB.Client
{
    public partial class CouchDatabase
    {
        /// <summary>
        /// http://docs.couchdb.org/en/2.2.0/api/database/bulk-api.html#api-db-all-docs
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        public async Task<CouchResponse> BulkInsertAsync(dynamic[] documentsObjects)
        {
            var request = new RestSharp.RestRequest("_bulk_docs", RestSharp.Method.POST);

            FindBuilder expression = new FindBuilder();
            expression.Docs(documentsObjects);

            request.AddParameter("application/json", expression.ToString(), ParameterType.RequestBody);
            return await client.http.ExecuteAsync(request);
        }
    }
}
