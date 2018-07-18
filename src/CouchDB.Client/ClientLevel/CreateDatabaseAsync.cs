using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CouchDB.Client
{
    public partial class CouchClient
    {
        /// <summary>
        /// http://docs.couchdb.org/en/2.1.2/api/database/common.html#put--db
        /// The default is 3 replicas and 8 shards
        /// </summary>
        /// <returns></returns>
        public async Task<CouchResponse> CreateDatabaseAsync(string databaseName)
        {
            var request = new RestSharp.RestRequest(databaseName, RestSharp.Method.PUT);
            return await http.ExecuteAsync(request);
        }
    }
}
