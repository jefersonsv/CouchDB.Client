using System;

namespace CouchDB.Client
{
    /// <summary>
    /// http://docs.couchdb.org/en/2.1.0/   
    /// </summary>
    public partial class CouchDatabase
    {
        private CouchClient client;
        private string database;

        internal CouchDatabase(CouchClient client, string database)
        {
            client.http.client.BaseUrl = new Uri(client.connectionString + database);
            this.client = client;
            this.database = database;
        }
    }
}
