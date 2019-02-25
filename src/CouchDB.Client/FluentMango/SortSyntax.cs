using System;
using System.Collections.Generic;
using System.Text;

namespace CouchDB.Client.FluentMango
{
    public class SortSyntax
    {
        public string Field;
        public  SortOrder Order;

        public SortSyntax(string field, SortOrder order)
        {
            this.Field = field;
            this.Order = order;
        }
    }
}
