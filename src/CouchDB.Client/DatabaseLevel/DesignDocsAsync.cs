using CouchDB.Client.FluentMango;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace CouchDB.Client
{
    public partial class CouchDatabase
    {
        /// <summary>
        /// Returns a JSON structure of all of the design documents in a given database. The information is returned as a JSON structure containing meta information about the return structure, including a list of all design documents and basic contents, consisting the ID, revision and key. The key is the from the design document’s _id.
        /// </summary>
        /// <see cref="https://docs.couchdb.org/en/stable/api/database/bulk-api.html?highlight=batch#get--db-_design_docs"/>
        /// <param name="conflicts">Includes conflicts information in response. Ignored if include_docs isn’t true. Default is false.</param>
        /// <param name="descending">Return the design documents in descending by key order. Default is false.</param>
        /// <param name="endkey">Stop returning records when the specified key is reached</param>
        /// <param name="endkey_docid">Stop returning records when the specified design document ID is reached</param>
        /// <param name="include_docs">Include the full content of the design documents in the return. Default is false</param>
        /// <param name="inclusive_end">Specifies whether the specified end key should be included in the result. Default is true</param>
        /// <param name="key">Return only design documents that match the specified key. Optional.</param>
        /// <param name="keys">Return only design documents that match the specified keys. Optional.</param>
        /// <param name="limit">Limit the number of the returned design documents to the specified number. Optional.</param>
        /// <param name="skip">Skip this number of records before starting to return the results. Default is 0.</param>
        /// <param name="startkey">Return records starting with the specified key. Optional.</param>
        /// <param name="startkey_docid">Return records starting with the specified design document ID. Optional.</param>
        /// <param name="update_seq">Response includes an update_seq value indicating which sequence id of the underlying database the view reflects. Default is false.</param>
        /// <returns></returns>
        public async Task<CouchResponse> DesignDocsAsync(
            bool? conflicts = null,
            bool? descending = null,
            string endkey = null,
            string endkey_docid = null,
            bool? include_docs = null,
            bool? inclusive_end = null,
            string key = null,
            string keys = null,
            int? limit = null,
            int? skip = null,
            string startkey = null,
            string startkey_docid = null,
            bool? update_seq = null)
        {
            var request = new RestSharp.RestRequest("_design_docs", RestSharp.Method.GET);

            if (conflicts.HasValue)
                request.AddQueryParameter("conflicts", conflicts.Value.ToString().ToLower());

            if (descending.HasValue)
                request.AddQueryParameter("descending", descending.Value.ToString().ToLower());

            if (!string.IsNullOrEmpty(endkey))
                request.AddQueryParameter("endkey", endkey);

            if (!string.IsNullOrEmpty(endkey_docid))
                request.AddQueryParameter("endkey_docid", endkey_docid);

            if (include_docs.HasValue)
                request.AddQueryParameter("include_docs", include_docs.Value.ToString().ToLower());

            if (inclusive_end.HasValue)
                request.AddQueryParameter("inclusive_end", inclusive_end.Value.ToString().ToLower());

            if (!string.IsNullOrEmpty(key))
                request.AddQueryParameter("key", key);

            if (!string.IsNullOrEmpty(keys))
                request.AddQueryParameter("keys", keys);

            if (limit.HasValue)
                request.AddQueryParameter("limit", limit.Value.ToString());

            if (skip.HasValue)
                request.AddQueryParameter("skip", skip.Value.ToString());

            if (!string.IsNullOrEmpty(startkey))
                request.AddQueryParameter("startkey", startkey);

            if (!string.IsNullOrEmpty(startkey_docid))
                request.AddQueryParameter("startkey_docid", startkey_docid);

            if (update_seq.HasValue)
                request.AddQueryParameter("update_seq", update_seq.Value.ToString().ToLower());

            return await client.http.ExecuteAsync(request);
        }

        /// <summary>
        /// Allows to specify multiple keys to be selected from the database. This enables you to request multiple design documents in a single request
        /// </summary>
        /// <see cref="https://docs.couchdb.org/en/stable/api/database/bulk-api.html?highlight=batch#post--db-_design_docs" />
        /// <returns></returns>
        public async Task<CouchResponse> DesignDocsAsync(string[] ids)
        {
            var request = new RestSharp.RestRequest("_design_docs", RestSharp.Method.POST);

            KeysBuilder expression = new KeysBuilder();
            ids.ToList().ForEach(a => expression.Add(a));

            request.AddParameter("application/json", expression.ToString(), RestSharp.ParameterType.RequestBody);
            return await client.http.ExecuteAsync(request);
        }
    }
}
