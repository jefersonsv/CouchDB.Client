using EnsureThat;
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
        public async Task<CouchResponse> ForceMergeAsync(JToken json)
        {
            Ensure.That(json, "json")
                .WithExtraMessageOf(o => "The json object isn't defined");

            EnsureArg.IsNotNull(json);
            var id = Ensure
                .That<string>(json.GetString("_id"), "_id")
                .WithExtraMessageOf(o => "The _id field isn't defined")
                .IsNotNullOrEmpty();

            var get = await GetAsync(id.Value);
            var rev = get.Json.GetString("_rev");

            JObject existentOject = JObject.Parse(get.Content);
            JObject newOject = JObject.Parse(json.ToString());

            existentOject.Merge(newOject, new JsonMergeSettings
            {
                // union array values together to avoid duplicates
                MergeArrayHandling = MergeArrayHandling.Union
            });

            return await UpdateAsync(existentOject);
        }

        public async Task<CouchResponse> ForceMergeAsync<T>(T obj)
        {
            var json = Newtonsoft.Json.JsonConvert.SerializeObject(obj);
            return await this.ForceMergeAsync(JToken.Parse(json));
        }
    }
}
