using EnsureThat;
using RestSharp;
using RestSharp.Authenticators;
using System;
using System.Threading.Tasks;

namespace CouchDB.Client
{
    public partial class CouchClient
    {
        internal string originalConnectionString;
        internal RestClientWrapper http;
        internal string connectionString;
        
        public CouchClient(string connectionString = null)
        {
            this.originalConnectionString = connectionString;
            
            if (string.IsNullOrEmpty(connectionString))
            {
                connectionString = "127.0.0.1:5984";
            }

            connectionString = connectionString.EndsWith("/", StringComparison.InvariantCultureIgnoreCase) ? connectionString : connectionString + "/";
            connectionString = connectionString.StartsWith("http", StringComparison.InvariantCultureIgnoreCase) ? connectionString : "http://" + connectionString;
            Uri uri = new Uri(connectionString);

            if (!string.IsNullOrEmpty(uri.UserInfo))
            {
                connectionString = connectionString.Replace(uri.UserInfo + "@", string.Empty);
            }

            this.connectionString = connectionString;
            var client = new RestClient(connectionString);
            if (!string.IsNullOrEmpty(uri.UserInfo))
            {
                client.Authenticator = new HttpBasicAuthenticator(
                    uri.UserInfo.Split(':')[0],
                    uri.UserInfo.Split(':')[1]
                );
            }

            http = new RestClientWrapper(client);
            http.client.AddDefaultHeader("Content-Type", "application/json");
        }

        public async Task<CouchDatabase> GetDatabaseAsync(string database)
        {
            var db = await this.GetDatabaseInfoAsync(database);
            if (db.StatusCode == System.Net.HttpStatusCode.OK)
                return new CouchDatabase(this, database);
            else
                return null;
        }
    }
}
