using CouchDB.Client.FluentMango;
using System;
using System.Threading.Tasks;

namespace CouchDB.Client
{
    public partial class CouchDatabase
    {
        /// <summary>
        /// Executes the built-in _all_docs view, returning all of the documents in the database. With the exception of the URL parameters (described below), this endpoint works identically to any other view
        /// </summary>
        /// <see cref="https://docs.couchdb.org/en/stable/api/database/bulk-api.html?highlight=batch#get--db-_all_docs"/>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<CouchResponse> SelectAsync()
        {
            var request = new RestSharp.RestRequest("_all_docs", RestSharp.Method.GET);
            return await client.http.ExecuteAsync(request);
        }

        /// <summary>
        /// Allows to specify multiple keys to be selected from the database
        /// </summary>
        /// <see cref="https://docs.couchdb.org/en/stable/api/database/bulk-api.html?highlight=batch#post--db-_all_docs" />
        /// <returns></returns>
        public async Task<CouchResponse> SelectAsync(string ids)
        {
            var request = new RestSharp.RestRequest("_all_docs", RestSharp.Method.POST);

            DocsBuilder expression = new DocsBuilder();
            expression.Add(ids);

            request.AddParameter("application/json", expression.ToString(), RestSharp.ParameterType.RequestBody);
            return await client.http.ExecuteAsync(request);
        }

        /// <summary>
        /// http://docs.couchdb.org/en/2.0.0/api/database/find.html
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        [Obsolete]
        public async Task<CouchResponse> SelectAsync(FindBuilder expression)
        {
            var request = new RestSharp.RestRequest("/_find", RestSharp.Method.POST);
            request.AddParameter("application/json", expression.ToString(), RestSharp.ParameterType.RequestBody);
            return await client.http.ExecuteAsync(request);
        }

        /// <summary>
        /// http://docs.couchdb.org/en/2.0.0/api/database/find.html
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        public async Task<CouchResponse> FindAsync(FindBuilder expression)
        {
            var request = new RestSharp.RestRequest("/_find", RestSharp.Method.POST);
            request.AddParameter("application/json", expression.ToString(), RestSharp.ParameterType.RequestBody);
            return await client.http.ExecuteAsync(request);
        }
    }
}
