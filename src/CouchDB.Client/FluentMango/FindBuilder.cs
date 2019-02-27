using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;

namespace CouchDB.Client.FluentMango
{
    /// <summary>
    /// The selector field is required
    /// </summary>
    /// <see cref="http://docs.couchdb.org/en/2.2.0/api/database/find.html"/>
    public class FindBuilder
    {
        Collection<Selector> selectors;
        JArray fields;
        JArray sort;
        JArray useIndex;
        int? skip;
        int? limit;
        int? readQuorum;
        string bookmark;
        bool? update;
        bool? stable;
        bool? stale;
        bool? executionStats;

        public FindBuilder()
        {
            selectors = new Collection<Selector>();
        }

        public static implicit operator string(FindBuilder builder)
        {
            return builder.ToString();
        }

        /// <summary>
        /// JSON array specifying which fields of each object should be returned. If it is omitted, the entire object is returned
        /// </summary>
        /// <param name="fields"></param>
        /// <returns></returns>
        public FindBuilder Fields(params string[] fieldArr)
        {
            fields = new JArray();
            fieldArr.ToList().ForEach(a => this.fields.Add(a));

            return this;
        }

        /// <summary>
        /// Maximum number of results returned. Default is 25. Optional
        /// </summary>
        /// <param name="limit"></param>
        /// <returns></returns>
        public FindBuilder Limit(int limit)
        {
            this.limit = limit;
            return this;
        }

        /// <summary>
        /// Instruct a query to use a specific index
        /// </summary>
        /// <param name="indexName"></param>
        /// <returns></returns>
        public FindBuilder UseIndex(string indexName)
        {
            this.useIndex = new JArray();
            this.useIndex.Add(indexName);
            return this;
        }

        /// <summary>
        /// Instruct a query to use a specific index
        /// </summary>
        /// <param name="indexes"></param>
        /// <returns></returns>
        public FindBuilder UseIndex(string[] indexes)
        {
            this.useIndex = new JArray();
            indexes.ToList().ForEach(a => this.useIndex.Add(a));
            return this;
        }

        /// <summary>
        /// Skip the first ‘n’ results, where ‘n’ is the value specified. Optional
        /// </summary>
        /// <param name="skip"></param>
        /// <returns></returns>
        public FindBuilder Skip(int skip)
        {
            this.skip = skip;
            return this;
        }

        /// <summary>
        /// JSON array following sort syntax. Optional
        /// </summary>
        /// <returns></returns>
        public FindBuilder Sort(string field)
        {
            this.sort = new JArray();
            this.sort.Add(field);

            return this;
        }

        /// <summary>
        /// JSON array following sort syntax. Optional
        /// </summary>
        /// <returns></returns>
        public FindBuilder Sort(string[] fields)
        {
            this.sort = new JArray();
            fields.ToList().ForEach(a => this.sort.Add(a));

            return this;
        }

        /// <summary>
        /// JSON array following sort syntax. Optional
        /// </summary>
        /// <returns></returns>
        public FindBuilder Sort(SortSyntax[] fieldsOrders)
        {
            this.sort = new JArray();
            fieldsOrders.ToList().ForEach(a =>
            {
                var obj = new JObject();
                obj.Add(a.Field, a.Order.ToString().ToLower());

                this.sort.Add(obj);
            });

            return this;
        }

        /// <summary>
        /// Read quorum needed for the result. This defaults to 1, in which case the document found in the index is returned. If set to a higher value, each document is read from at least that many replicas before it is returned in the results. This is likely to take more time than using only the document stored locally with the index. Optional, default: 1
        /// </summary>
        /// <param name="readQuorum"></param>
        /// <returns></returns>
        public FindBuilder ReadQuorum(int readQuorum)
        {
            this.readQuorum = readQuorum;
            return this;
        }

        /// <summary>
        /// A string that enables you to specify which page of results you require. Used for paging through result sets. Every query returns an opaque string under the bookmark key that can then be passed back in a query to get the next page of results. If any part of the selector query changes between requests, the results are undefined. Optional, default: null
        /// </summary>
        /// <param name="readQuorum"></param>
        /// <returns></returns>
        public FindBuilder Bookmark(string bookmark)
        {
            this.bookmark = bookmark;
            return this;
        }

        /// <summary>
        /// Whether to update the index prior to returning the result. Default is true. Optional
        /// </summary>
        /// <param name="update"></param>
        /// <returns></returns>
        public FindBuilder Update(bool update)
        {
            this.update = update;
            return this;
        }

        /// <summary>
        /// Whether or not the view results should be returned from a “stable” set of shards. Optional
        /// </summary>
        /// <param name="stable"></param>
        /// <returns></returns>
        public FindBuilder Stable(bool stable)
        {
            this.stable = stable;
            return this;
        }

        /// <summary>
        /// Combination of update=false and stable=true options. Optional
        /// </summary>
        /// <param name="stale"></param>
        /// <returns></returns>
        public FindBuilder Stale(bool stale)
        {
            this.stale = stale;
            return this;
        }

        /// <summary>
        /// Include execution statistics in the query response. Optional, default: ``false``
        /// </summary>
        /// <param name="stale"></param>
        /// <returns></returns>
        public FindBuilder ExecutionStats(bool executionStats)
        {
            this.executionStats = executionStats;
            return this;
        }

        /// <summary>
        /// Selector documents where satisfact the filter in selector object
        /// </summary>
        /// <param name="selector"></param>
        /// <returns></returns>
        public FindBuilder AddSelector(Selector selector)
        {
            this.selectors.Add(selector);
            return this;
        }

        /// <summary>
        /// Selector documents where satisfact the filter in parameters
        /// </summary>
        /// <param name="selector"></param>
        /// <returns></returns>
        public FindBuilder AddSelector(string field, SelectorOperator selectorOperator, object value)
        {
            this.selectors.Add(new Selector(field, selectorOperator, value));
            return this;
        }

        public FindBuilder AddSelector(string field, CombinationOperator combinationOperator, object value)
        {
            this.selectors.Add(new Selector(field, combinationOperator, value));
            return this;
        }

        /// <summary>
        /// Selector documents where the field match with value
        /// </summary>
        /// <param name="selector"></param>
        /// <returns></returns>
        public FindBuilder AddSelector(string field, object value)
        {
            this.selectors.Add(new Selector(field, SelectorOperator.Equals, value));
            return this;
        }

        public override string ToString()
        {
            if (this.selectors.Count == 0)
                throw new Exception("The selector is required");

            var obj = new JObject();

            foreach (var selector in selectors)
            {
                var token = JToken.Parse(selector.ToString());
                var prop = (JProperty) token.First;
                obj.Add(prop.Name, prop.Value);
            }

            // selector
            var ret = new JObject();
            ret.Add("selector", obj);

            // fields
            if (this.fields != null && this.fields.Count > 0)
                ret.Add("fields", this.fields);

            // sort
            if (this.sort != null && this.sort.Count > 0)
                ret.Add("sort", this.sort);

            if (this.useIndex != null && this.useIndex.Count > 0)
                ret.Add("use_index", this.sort);

            if (this.skip.HasValue)
                ret.Add("skip", this.skip);

            if (this.limit.HasValue)
                ret.Add("limit", this.limit);

            if (this.readQuorum.HasValue)
                ret.Add("r", this.readQuorum);

            if (!string.IsNullOrEmpty(bookmark))
                ret.Add("bookmark ", this.bookmark);

            if (this.update.HasValue)
                ret.Add("update", this.update.Value.ToString().ToLower());

            if (this.stable.HasValue)
                ret.Add("stable", this.stable.Value.ToString().ToLower());

            if (this.stale.HasValue)
                ret.Add("stale", stale.Value ? "ok" : "false");

            if (this.executionStats.HasValue)
                ret.Add("execution_stats", this.executionStats.Value.ToString().ToLower());

            return ret.ToString();
        }
    }
}