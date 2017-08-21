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
        public async Task<CouchResponse> ForceUpdateAsync(JToken json)
        {
            var insert = await UpdateAsync(json);
            if (insert.StatusCode == System.Net.HttpStatusCode.Conflict)
            {
                var id = json.GetString("_id");
                var get = await GetAsync(id);
                json["_rev"] = get.Json.GetString("_rev");

                var request = new RestSharp.RestRequest(RestSharp.Method.POST);
                request.AddParameter("application/json", json, ParameterType.RequestBody);
                return await client.http.ExecuteAsync(request);
            }

            return insert;
        }
    }
}
