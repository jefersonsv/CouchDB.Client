using EnsureThat;
using Newtonsoft.Json.Linq;
using System.Threading.Tasks;

namespace CouchDB.Client
{
    public partial class CouchDatabase
    {
        public async Task<CouchResponse> ForceMergeAsync(JToken json)
        {
            Ensure.That<JToken>(json, "json", optsFn => optsFn.WithMessage("The json object isn't defined"));

            var id = Ensure.That<string>(json.GetString("_id"), "_id", optsFn => optsFn.WithMessage("The _id field isn't defined"));

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
