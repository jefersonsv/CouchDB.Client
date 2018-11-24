using System.Threading.Tasks;

namespace CouchDB.Client
{
    public partial class CouchClient
    {
        /// <summary>
        /// http://docs.couchdb.org/en/2.1.2/api/database/common.html#delete--db
        /// </summary>
        /// <returns></returns>
        public async Task<CouchResponse> DeleteDatabaseAsync(string databaseName)
        {
            var request = new RestSharp.RestRequest(databaseName, RestSharp.Method.DELETE);
            return await http.ExecuteAsync(request);
        }
    }
}
