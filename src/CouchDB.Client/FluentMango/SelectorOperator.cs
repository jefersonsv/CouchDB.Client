using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CouchDB.Client.FluentMango
{
    public enum SelectorOperator
    {
        GreatThen,
        GreatEqualsThan,
        LessThen,
        LessEqualsThan,
        NotEquals,
        Equals,
        Not,
        Nor,
        Exists,
        Regex,
        Type
    }
}