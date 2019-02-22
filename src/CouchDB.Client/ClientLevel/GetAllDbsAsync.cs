using System;
using System.Threading.Tasks;

namespace CouchDB.Client
{
    public partial class CouchClient
    {
        /// <summary>
        /// http://docs.couchdb.org/en/2.2.0/api/server/common.html#all-dbs
        /// </summary>
        /// <returns></returns>
        [Obsolete("This method will be obsolete. Use ListAllDatabasesAsync() instead.")]
        public async Task<CouchResponse> GetAllDbsAsync()
        {
            var request = new RestSharp.RestRequest("_all_dbs");
            return await http.ExecuteAsync(request);
        }
    }
}
