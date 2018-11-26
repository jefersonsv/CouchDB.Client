using EnsureThat;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Schema;
using Newtonsoft.Json.Schema.Generation;
using System;
using System.IO;

namespace CouchDB.Client
{
    public static class Helper
    {
        public static bool IsValidJson(this string stringValue)
        {
            if (string.IsNullOrWhiteSpace(stringValue))
            {
                return false;
            }

            var value = stringValue.Trim();

            if ((value.StartsWith("{") && value.EndsWith("}")) || //For object
                (value.StartsWith("[") && value.EndsWith("]"))) //For array
            {
                try
                {
                    var obj = JToken.Parse(value);
                    return true;
                }
                catch (JsonReaderException)
                {
                    return false;
                }
            }

            return false;
        }

        internal static T TryParse<T>(string jsonData) where T : new()
        {
            JSchemaGenerator generator = new JSchemaGenerator();
            JSchema parsedSchema = generator.Generate(typeof(T));
            JObject jObject = JObject.Parse(jsonData);

            return jObject.IsValid(parsedSchema) ?
                JsonConvert.DeserializeObject<T>(jsonData) : default(T);
        }

        public static string EncodeID(string unencodedID)
        {
            return System.Net.WebUtility.UrlEncode(unencodedID);
        }

        public static string DecodeID(string encodedID)
        {
            return System.Net.WebUtility.UrlDecode(encodedID);
        }

        public static string GetConnectionString()
        {
            return GetConnectionString("COUCH_DB");
        }

        public static string GetConnectionString(string name)
        {
            Ensure.That(name).IsNotNullOrWhiteSpace();

            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appSettings.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables();

            var configuration = builder.Build();
            var connectionString = configuration.GetSection("COUCH_DB").Value;

            if (string.IsNullOrEmpty(connectionString))
            {
                throw new InvalidOperationException("Nothing connectrion string has been setup");
            }

            return connectionString;
        }
    }
}
