using System;

namespace CouchDB.Client
{
    /// <summary>
    /// http://docs.couchdb.org/en/2.2.0/   
    /// </summary>
    public partial class CouchDatabase
    {
        private CouchClient client;
        private string database;

        internal CouchDatabase(CouchClient client, string database)
        {
            this.client = new CouchClient(client.originalConnectionString);
            this.client.http.client.BaseUrl = new Uri(client.connectionString + database); // add database name
            this.database = database;
        }
    }
}
