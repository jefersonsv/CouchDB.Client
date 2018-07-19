using CouchDB.Client.FluentMango;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CouchDB.Client
{
    public partial class CouchDatabase
    {
        public async Task<CouchResponse> SelectAsync(string[] ids)
        {
            var request = new RestSharp.RestRequest("_all_docs", RestSharp.Method.POST);

            FindBuilder expression = new FindBuilder();
            expression.Keys(ids);

            request.AddParameter("application/json", expression.ToString(), RestSharp.ParameterType.RequestBody);
            return await client.http.ExecuteAsync(request);
        }

        public async Task<CouchResponse> SelectAsync()
        {
            var request = new RestSharp.RestRequest("_all_docs", RestSharp.Method.POST);

            FindBuilder expression = new FindBuilder();

            request.AddParameter("application/json", expression.ToString(), RestSharp.ParameterType.RequestBody);
            return await client.http.ExecuteAsync(request);
        }

        public async Task<CouchResponse> SelectAsync(int take)
        {
            var request = new RestSharp.RestRequest("_all_docs", RestSharp.Method.POST);

            FindBuilder expression = new FindBuilder()
                .Limit(take);

            request.AddParameter("application/json", expression.ToString(), RestSharp.ParameterType.RequestBody);
            return await client.http.ExecuteAsync(request);
        }

        /// <summary>
        /// http://docs.couchdb.org/en/2.0.0/api/database/find.html
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        public async Task<CouchResponse> SelectAsync(FindBuilder expression)
        {
            var request = new RestSharp.RestRequest("/_find", RestSharp.Method.POST);
            request.AddParameter("application/json", expression.ToString(), RestSharp.ParameterType.RequestBody);
            return await client.http.ExecuteAsync(request);
        }
    }
}
