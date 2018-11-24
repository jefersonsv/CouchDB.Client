using System.Threading.Tasks;

namespace CouchDB.Client
{
    public partial class CouchClient
    {
        public async Task<CouchResponse> GetMembershipAsync()
        {
            var request = new RestSharp.RestRequest("_membership");
            return await http.ExecuteAsync(request);
        }
    }
}
