using CouchDB.Client.FluentMango;
using Newtonsoft.Json.Linq;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CouchDB.Client
{
    public partial class CouchDatabase
    {
        /// <summary>
        /// http://docs.couchdb.org/en/2.0.0/api/database/bulk-api.html#api-db-all-docs
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
