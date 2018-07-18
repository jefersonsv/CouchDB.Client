using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CouchDB.Client
{
    public partial class CouchDatabase
    {
        /// <summary>
        /// http://docs.couchdb.org/en/2.0.0/api/ddoc/common.html
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<CouchResponse> ForceDeleteAsync(string id)
        {
            var get = await this.GetAsync(Helper.EncodeID(id));
            return await this.DeleteAsync(Helper.EncodeID(id), get.Rev);
        }
    }
}
