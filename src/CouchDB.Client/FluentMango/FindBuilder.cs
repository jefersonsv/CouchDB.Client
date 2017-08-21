using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CouchDB.Client.FluentMango
{
    /// <summary>
    /// http://docs.couchdb.org/en/2.0.0/api/database/find.html
    /// </summary>
    public class FindBuilder
    {
        private Collection<Selector> selectorCollection;
        private JsonTextWriter json;
        private StringBuilder sb;
        private StringWriter sw;

        private bool endBracketsUse { get; set; }

        private string[] fields { get; set; }

        private int? skip { get; set; }

        private int? limit { get; set; }

        public FindBuilder()
        {
            selectorCollection = new Collection<Selector>();
            sb = new StringBuilder();
            sw = new StringWriter(sb);
            json = new JsonTextWriter(sw);
            json.Formatting = Formatting.Indented;
            json.WriteStartObject();
        }

        public static implicit operator string(FindBuilder builder)
        {
            return builder.ToString();
        }

        public FindBuilder Fields(params string[] fields)
        {
            this.fields = fields;
            return this;
        }

        public FindBuilder Keys(string[] keys)
        {
            json.WritePropertyName("keys");
            json.WriteStartArray();
            keys.ToList().ForEach(o => json.WriteValue(o));
            json.WriteEndArray();

            return this;
        }

        public FindBuilder Docs<T>(T[] jsonArr)
        {
            json.WritePropertyName("docs");
            json.WriteStartArray();
            foreach (var item in jsonArr)
            {
                var jsonRaw = Newtonsoft.Json.JsonConvert.SerializeObject(item);
                json.WriteRawValue(jsonRaw);
            }
            json.WriteEndArray();

            return this;
        }

        public FindBuilder Limit(int limit)
        {
            this.limit = limit;
            return this;
        }

        public FindBuilder Selector(Selector selector)
        {
            selectorCollection.Add(selector);
            return this;
        }

        public FindBuilder Selector(string field, SelectorOperator selectorOperator, object value)
        {
            selectorCollection.Add(new Selector
            {
                Field = field,
                SelectorOperator = selectorOperator,
                Value = value
            });

            return this;
        }

        public FindBuilder Selector(string field, string equalsValue)
        {
            selectorCollection.Add(new Selector
            {
                Field = field,
                SelectorOperator = SelectorOperator.Equals,
                Value = equalsValue
            });

            return this;
        }

        public FindBuilder Skip(int skip)
        {
            this.skip = skip;
            return this;
        }

        private void WriteSelectores()
        {
            if (selectorCollection.Count == 0)
                return;

            this.StartSelector();

            foreach (var item in selectorCollection)
            {
                var first = item == selectorCollection[0];
                var last = item == selectorCollection[selectorCollection.Count - 1];

                if (selectorCollection.Count > 1 && first)
                {
                    json.WritePropertyName("$and");
                    json.WriteStartArray();
                }

                if (selectorCollection.Count > 1)
                {
                    json.WriteStartObject();
                }

                json.WritePropertyName(item.Field);
                json.WriteStartObject();
                switch (item.SelectorOperator)
                {
                    case SelectorOperator.Equals:
                        json.WritePropertyName("$eq");
                        json.WriteValue(item.Value);
                        break;

                    case SelectorOperator.Exists:
                        json.WritePropertyName("$exists");
                        json.WriteValue(Convert.ToBoolean(item.Value));
                        break;

                    case SelectorOperator.GreatEqualsThan:
                        json.WritePropertyName("$gte");
                        json.WriteValue(item.Value);
                        break;

                    case SelectorOperator.GreatThen:
                        json.WritePropertyName("$gt");
                        json.WriteValue(item.Value);
                        break;

                    case SelectorOperator.LessEqualsThan:
                        json.WritePropertyName("$lte");
                        json.WriteValue(item.Value);
                        break;

                    case SelectorOperator.LessThen:
                        json.WritePropertyName("$lt");
                        json.WriteValue(item.Value);
                        break;

                    case SelectorOperator.Type:
                        json.WritePropertyName("$type");
                        Types type = (Types)item.Value;
                        json.WriteValue(item.Value.ToString().ToLower());
                        break;
                    /*
                case SelectorOperator.Nor:
                    json.WritePropertyName("$nor");
                    break;

                case SelectorOperator.Not
                    json.WritePropertyName("$eq");
                    break;
                    */
                    case SelectorOperator.NotEquals:
                        json.WritePropertyName("$ne");
                        json.WriteValue(item.Value);
                        break;

                    case SelectorOperator.Regex:
                        json.WritePropertyName("$regex");
                        json.WriteValue(item.Value);
                        break;
                }
                json.WriteEndObject();

                if (selectorCollection.Count > 1)
                {
                    json.WriteEndObject();
                }

                if (selectorCollection.Count > 1 && last)
                {
                    json.WriteEndArray();
                }
            }

            this.EndSelector();
        }

        public override string ToString()
        {
            // Write selectors
            WriteSelectores();

            if (this.fields != null && this.fields.Any())
            {
                json.WritePropertyName("fields");
                json.WriteStartArray();
                foreach (var item in fields)
                {
                    json.WriteValue(item);
                }
                json.WriteEndArray();
            }

            if (this.limit.HasValue)
            {
                json.WritePropertyName("limit");
                json.WriteValue(limit);
            }

            if (this.skip.HasValue)
            {
                json.WritePropertyName("skip");
                json.WriteValue(skip);
            }

            if (!endBracketsUse)
            {
                json.WriteEndObject();
                endBracketsUse = true;
            }

            //JObject jobject = JObject.Parse(sb.ToString());
            //return jobject.Root.ToString(Formatting.None);
            return sb.ToString();
        }

        private void EndSelector()
        {
            json.WriteEndObject();
        }

        private void StartSelector()
        {
            json.WritePropertyName("selector");
            json.WriteStartObject();
        }
    }
}