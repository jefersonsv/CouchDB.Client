using System.Threading.Tasks;

namespace CouchDB.Client
{
    public partial class CouchClient
    {
        /// <summary>
        /// http://docs.couchdb.org/en/2.1.2/api/database/common.html#get--db
        /// </summary>
        /// <returns></returns>
        public async Task<CouchResponse> GetDatabaseInfoAsync(string databaseName)
        {
            var request = new RestSharp.RestRequest(databaseName, RestSharp.Method.HEAD);
            return await http.ExecuteAsync(request);
        }
    }
}
