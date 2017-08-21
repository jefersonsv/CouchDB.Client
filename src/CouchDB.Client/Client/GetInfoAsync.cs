using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CouchDB.Client
{
    public partial class CouchClient
    {
        public async Task<CouchResponse> GetInfoAsync()
        {
            var request = new RestSharp.RestRequest();
            return await http.ExecuteAsync(request);
        }
    }
}
