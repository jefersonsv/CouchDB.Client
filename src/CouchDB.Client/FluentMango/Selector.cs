using System;
using System.Collections.Generic;
using System.Text;

namespace CouchDB.Client.FluentMango
{
    public class Selector
    {
        public string Field { get; set; }
        public SelectorOperator SelectorOperator { get; set; }
        public object Value { get; set; }
    }
}
