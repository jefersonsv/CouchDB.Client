using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;

namespace Newtonsoft.Json.Linq
{
    public static class JTokenExtensions
    {
        public static string GetString(this JToken token, string path)
        {
            return token.SelectToken(path)?.Value<string>();
        }

        public static JArray GetArray(this JToken token, string path)
        {
            return (JArray) token.SelectToken(path);
        }

        public static DateTime GetDateTime(this JToken token, string path)
        {
            return (DateTime) token.SelectToken(path)?.Value<DateTime>();
        }

        public static void Rename(this JToken token, string newName)
        {
            var parent = token.Parent;
            if (parent == null)
                throw new InvalidOperationException("The parent is missing.");
            var newToken = new JProperty(newName, token);
            parent.Replace(newToken);
        }
    }
}
