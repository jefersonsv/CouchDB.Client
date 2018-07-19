using EnsureThat;
using RestSharp;
using RestSharp.Authenticators;
using System;
using System.Threading.Tasks;

namespace CouchDB.Client
{
    public partial class CouchClient
    {
        internal RestClientWrapper http;
        internal string connectionString;
        
        public CouchClient(string connectionString)
        {
            Ensure.That(connectionString).IsNotNullOrWhiteSpace();

            connectionString = connectionString.EndsWith("/", StringComparison.InvariantCultureIgnoreCase) ? connectionString : connectionString + "/";
            connectionString = connectionString.StartsWith("http", StringComparison.InvariantCultureIgnoreCase) ? connectionString : "http://" + connectionString;
            Uri uri = new Uri(connectionString);

            if (!string.IsNullOrEmpty(uri.UserInfo))
            {
                this.connectionString = connectionString.Replace(uri.UserInfo + "@", string.Empty);
            }
            else
            {
                this.connectionString = connectionString;
            }

            var client = new RestClient(this.connectionString);
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
