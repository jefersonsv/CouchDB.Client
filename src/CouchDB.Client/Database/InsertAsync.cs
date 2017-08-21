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
        /// http://docs.couchdb.org/en/2.0.0/api/database/common.html#post--db
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        public async Task<CouchResponse> InsertAsync(JToken json)
        {
            var request = new RestSharp.RestRequest(RestSharp.Method.POST);
            request.AddParameter("application/json", json, ParameterType.RequestBody);
            return await client.http.ExecuteAsync(request);
        }

        public async Task<CouchResponse> InsertAsync<T>(T obj)
        {
            var json = Newtonsoft.Json.JsonConvert.SerializeObject(obj);
            return await this.InsertAsync(JToken.Parse(json));
        }
    }
}
