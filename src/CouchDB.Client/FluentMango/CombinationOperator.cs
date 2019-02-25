namespace CouchDB.Client.FluentMango
{
    /// <summary>
    /// <see cref="http://docs.couchdb.org/en/2.2.0/api/database/find.html#combination-operators"/>
    /// </summary>
    public enum CombinationOperator
    {
        /// <summary>
        /// Matches if all the selectors in the array match.
        /// </summary>
        And,

        /// <summary>
        /// Matches if any of the selectors in the array match. All selectors must use the same index.
        /// </summary>
        Or,

        /// <summary>
        /// Matches if the given selector does not match.
        /// </summary>
        Not,

        /// <summary>
        /// Matches if none of the selectors in the array match.
        /// </summary>
        Nor,

        /// <summary>
        /// Matches an array value if it contains all the elements of the argument array.
        /// </summary>
        All,

        /// <summary>
        /// Matches and returns all documents that contain an array field with at least one element that matches all the specified query criteria.
        /// </summary>
        ElementMatch,

        /// <summary>
        /// Matches and returns all documents that contain an array field with all its elements matching all the specified query criteria.
        /// </summary>
        AllMatch
    }
}