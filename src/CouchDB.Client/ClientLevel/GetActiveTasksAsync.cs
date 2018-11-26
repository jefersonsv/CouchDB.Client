using System.Threading.Tasks;

namespace CouchDB.Client
{
    public partial class CouchClient
    {
        /// <summary>
        /// http://docs.couchdb.org/en/2.2.0/api/server/common.html#active-tasks
        /// </summary>
        /// <returns></returns>
        public async Task<CouchResponse> GetActiveTasksAsync()
        {
            var request = new RestSharp.RestRequest("_active_tasks");
            return await http.ExecuteAsync(request);
        }
    }
}
