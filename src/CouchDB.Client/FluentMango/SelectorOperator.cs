namespace CouchDB.Client.FluentMango
{
    /// <summary>
    /// <see cref="http://docs.couchdb.org/en/2.2.0/api/database/find.html#condition-operators"/>
    /// </summary>
    public enum SelectorOperator
    {
        /// <summary>
        /// The field is equal to the argument
        /// </summary>
        Equals,

        /// <summary>
        /// The field is greater than the to the argument
        /// </summary>
        GreaterThan,

        /// <summary>
        /// The field is greater than or equal to the argument
        /// </summary>
        GreaterEqualsThan,

        /// <summary>
        /// The field is less than the argument
        /// </summary>
        LessThan,

        /// <summary>
        /// The field is less than or equal to the argument
        /// </summary>
        LessEqualsThan,

        /// <summary>
        /// The field is not equal to the argument
        /// </summary>
        NotEquals,

        /// <summary>
        /// Check whether the field exists or not, regardless of its value
        /// </summary>
        Exists,
        
        /// <summary>
        /// Check the document field’s type. Valid values are "null", "boolean", "number", "string", "array", and "object".
        /// </summary>
        Type,

        /// <summary>
        /// The document field must exist in the list provided
        /// </summary>
        In,

        /// <summary>
        /// The document field not must exist in the list provided
        /// </summary>
        Nin,

        /// <summary>
        /// Special condition to match the length of an array field in a document. Non-array fields cannot match this condition
        /// </summary>
        Size,

        /// <summary>
        /// Divisor and Remainder are both positive or negative integers. Non-integer values result in a 404. Matches documents where field % Divisor == Remainder is true, and only when the document field is an integer
        /// </summary>
        Mod,

        /// <summary>
        /// A regular expression pattern to match against the document field. Only matches when the field is a string value and matches the supplied regular expression. The matching algorithms are based on the Perl Compatible Regular Expression (PCRE) library. For more information about what is implemented, see the see the Erlang Regular Expression
        /// </summary>
        Regex
    }
}