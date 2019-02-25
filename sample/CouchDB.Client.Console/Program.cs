using CouchDB.Client.FluentMango;
using System.Threading.Tasks;

namespace CouchDB.Client.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            RunAsync().Wait();
        }

        public static async Task RunAsync()
        {
            var find = new FluentMango.FindBuilder()
                .AddSelector("year", 1850)
                .AddSelector("year-eq", FluentMango.SelectorOperator.Equals, 1900)
                .AddSelector("year-lte", FluentMango.SelectorOperator.LessEqualsThan, 1900)
                .AddSelector("year-lt", FluentMango.SelectorOperator.LessThan, 1910)
                .AddSelector("year-neq", 1900)
                .AddSelector("year-gt", FluentMango.SelectorOperator.GreaterThan, 1910)
                .AddSelector("year-gte", FluentMango.SelectorOperator.GreaterEqualsThan, 1910)
                .AddSelector("name-exists", FluentMango.SelectorOperator.Exists, true)
                .AddSelector("name-type", FluentMango.SelectorOperator.Type, "null")
                .AddSelector("year-in", FluentMango.SelectorOperator.In, new int[] { 1984, 1991 })
                .AddSelector("names-nin", FluentMango.SelectorOperator.In, new string[] { "john", "mark" })
                .AddSelector("size", FluentMango.SelectorOperator.Size, 23)
                .AddSelector("mod", FluentMango.SelectorOperator.Mod, new int[] { 4, 0 })
                .AddSelector("regex", FluentMango.SelectorOperator.Regex, "^A")
                .AddSelector("year-not", FluentMango.CombinationOperator.Not, 1901)
                .AddSelector("year-or", FluentMango.CombinationOperator.Or,
                    new Selector[] {
                        new Selector("director", "George Lucas"),
                        new Selector("director", "Steven Spielberg") })
                .AddSelector("year-and", FluentMango.CombinationOperator.And,
                    new Selector[] {
                        new Selector("_id", SelectorOperator.GreaterThan, null),
                        new Selector("director", SelectorOperator.In, new int[] { 2014, 2015 } ) })
                .AddSelector("year-nor", FluentMango.CombinationOperator.Nor,
                    new Selector[] {
                        new Selector("year1", 1901),
                        new Selector("year2", 1905),
                        new Selector("year3", 1907),
                        new Selector("year4", 1909) })
                .AddSelector("genre", FluentMango.CombinationOperator.All, new string[] { "Comedy", "Short" })
                .AddSelector("genre-element-match", FluentMango.CombinationOperator.ElementMatch, "Horror")
                .AddSelector("genre-all-mach", FluentMango.CombinationOperator.AllMatch, "Horror")
                .Limit(20)
                .Skip(10)
                .Sort(new string[] { "name", "username" } );

            var str = find.ToString();

            //    .Create();
            var cs = CouchDB.Client.Helper.GetConnectionString();
            var client = new CouchClient(cs);

            // get database
            var db = await client.GetDatabaseAsync("test");
        }
    }
}