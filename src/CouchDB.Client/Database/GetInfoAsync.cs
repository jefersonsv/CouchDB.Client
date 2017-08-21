using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CouchDB.Client
{
    public partial class CouchDatabase
    {
        public async Task<CouchResponse> GetInfoAsync()
        {
            var request = new RestSharp.RestRequest();
            return await client.http.ExecuteAsync(request);
        }
    }
}
