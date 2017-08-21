using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CouchDB.Client
{
    public partial class CouchClient
    {
        public async Task<CouchResponse> GetMerbershipAsync()
        {
            var request = new RestSharp.RestRequest("_membership");
            return await http.ExecuteAsync(request);
        }
    }
}
