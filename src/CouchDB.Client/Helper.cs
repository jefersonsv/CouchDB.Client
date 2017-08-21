using EnsureThat;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace CouchDB.Client
{
    public static class Helper
    {
        public static string EncodeID(string id)
        {
            return System.Net.WebUtility.UrlEncode(id);
        }

        public static string DecodeID(string id)
        {
            return System.Net.WebUtility.UrlDecode(id);
        }

        public static string GetConnectionString()
        {
            return GetConnectionString("COUCH_DB");
        }

        public static string GetConnectionString(string name)
        {
            EnsureArg.IsNotNull(name);

            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appSettings.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables();

            var configuration = builder.Build();
            var connectionString = configuration.GetSection("COUCH_DB").Value;

            if (string.IsNullOrEmpty(connectionString))
            {
                throw new Exception("Nothing connectrion string has been setup");
            }

            return connectionString;
        }
    }
}
