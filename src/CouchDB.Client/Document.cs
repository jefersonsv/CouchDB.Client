using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net;
using System.Text;


namespace CouchDB.Client
{
    public class CouchResponse
    {
        /// <summary>
        /// http://stackoverflow.com/questions/38558844/jcontainer-jobject-jtoken-and-linq-confusion
        /// </summary>
        public JToken Json { get; }
        public JArray Docs { get; }
        public dynamic Dynamic { get; }
        public string Id { get; }
        public string Rev { get; }
        public bool Deleted { get; }
        public string Warning { get; }
        public string ErrorMessage { get; set; }
        public IList<Parameter> Headers { get; protected internal set; }
        public IList<RestResponseCookie> Cookies { get; protected internal set; }
        public string Server { get; set; }
        public Uri ResponseUri { get; set; }
        public byte[] RawBytes { get; set; }
        public string StatusDescription { get; set; }
        public HttpStatusCode StatusCode { get; set; }
        public string Content { get; set; }
        public string ContentEncoding { get; set; }
        public long ContentLength { get; set; }
        public string ContentType { get; set; }
        public IRestRequest Request { get; set; }
        public Exception ErrorException { get; set; }

        [Obsolete("This property is deprecated")]
        public Version ProtocolVersion { get; set; }

        //public bool HasAttachments { get; set; }

        public Collection<RevisionInfo> Revisions { get; set; }

        public CouchResponse(IRestResponse response)
        {
            Revisions = new Collection<RevisionInfo>();

            var isJson = Helper.IsValidJson(response.Content);
            if (isJson && response.ResponseStatus != ResponseStatus.Error)
            {
                this.Json = JToken.Parse(response.Content);
                this.Json?.SelectToken("id")?.Rename("_id");
                this.Json?.SelectToken("rev")?.Rename("_rev");

                this.Dynamic = (dynamic) this.Json;

                this.Id = Helper.DecodeID(this.Json.GetString("_id"));
                this.Rev = this.Json.GetString("_rev");
                this.Deleted = bool.Parse(this.Json.GetString("_deleted") ?? "false");
                this.Warning = this.Json.GetString("warning");

                var docs = this.Json.GetArray("docs");
                var rows = this.Json.GetArray("rows");
                this.Docs = docs == null ? rows : docs;

                //this.HasAttachments = this.Json["_attachments"] != null;

                // Get Revision List
                var revisionInfoArr = this.Json.GetArray("_revs_info");

                if (revisionInfoArr != null)
                {
                    foreach (var item in revisionInfoArr.Children())
                    {
                        Revisions.Add(new RevisionInfo()
                        {
                            Revision = item.GetString("rev"),
                            Status = item.GetString("status")
                        });
                    }
                }

                this.Content = this.Json.ToString();
            }
            
            

            this.ContentEncoding = response.ContentEncoding;
            this.ContentLength = response.ContentLength;
            this.ContentType = response.ContentType;
            this.Cookies = response.Cookies;
            this.ErrorException = response.ErrorException;
            this.ErrorMessage = response.ErrorMessage;
            this.Headers = response.Headers;
            this.ProtocolVersion = response.ProtocolVersion;
            this.RawBytes = response.RawBytes;
            // this.ResponseStatus = response.ResponseStatus; // Its can confuse the user
            this.StatusCode = response.StatusCode;
            this.StatusDescription = response.StatusDescription;
        }
    }
}
