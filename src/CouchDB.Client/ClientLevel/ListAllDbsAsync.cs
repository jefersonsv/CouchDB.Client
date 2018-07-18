using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CouchDB.Client
{
    public partial class CouchClient
    {
        /// <summary>
        /// http://docs.couchdb.org/en/2.0.0/api/server/common.html#all-dbs
        /// </summary>
        /// <returns></returns>
        public async Task<CouchResponse> ListAllDbsAsync()
        {
            var request = new RestSharp.RestRequest("_all_dbs");
            return await http.ExecuteAsync(request);
        }
    }
}
