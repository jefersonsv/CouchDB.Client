# CouchDB.Client

This client access the Apache CouchDB (relax) for platform .NET Standard 2.0
Builded under the API 2.1.0 this driver works together with RestSharp Simple Http Client and Json.NET popular high-performance JSON framework for .NET.

# Aditional feature futher more API

* Force update (get the last revision and update document)
* Force delete (get the last revision and delete document)

# Getting started

Connecto to localhost server

```c#
var client = new CouchDB.Client.CouchClient();
```

# Sample usage

```c#
// create database
var created = await client.CreateDatabaseAsync("test");

// list all databases
var alldbs = await client.ListAllDbsAsync();

// get created database
var db = await client.GetDatabaseAsync("test");

// create document
var doc = new
{
    _id = "testid",
    machine = Environment.MachineName,
    year = DateTime.Now.Year
};
var inserted = await db.InsertAsync(doc);

// get document by id
var getdoc = await db.GetAsync(inserted.Id);

// select all
var five = await db.SelectAsync();

// select all limit 5 documents
var five = await db.SelectAsync(5);
```

# Nuget

CouchDB can be installed by Nuget package manager.

```
PM> Install-Package CouchDB.Client
```

# Features

> To use features of client level you must to get instance of client using `new CouchClient();`

## Client level
- CreateDatabaseAsync
- GetAllDbsAsync
- GetDatabaseInfoAsync
- DeleteDatabaseAsync

> To use features of database level you must to get instance of database using `GetDatabaseAsync()`

## Database level
- BulkInsertAsync
- DeleteAsync
  - BatchMode
- DumpAsync
- ForceDeleteAsync
- ForceMergeAsync
- ForceUpdateAsync
- GetAsync (get a document)
  -  Show Revisions
- ExistAsync (check if document exists)
- GetInfoAsync
- GetRevision
- GetRevisionInfoAsync
- InsertAsync
  - Generating ID
  - Specifying ID
  - BatchMode
- InsertRevisionAsync
  - BatchMode
- SelectAsync
- UpdateAsync

## Fluent Mango

### Selector Operator
  - Equals,
  - GreaterThan,
  - GreaterEqualsThan,
  - LessThan,
  - LessEqualsThan,
  - NotEquals,
  - Exists,
  - Type,
  - In,
  - Nin,
  - Size,
  - Mod,
  - Regex

### Condition Operator
  - And
  - Or
  - Not
  - Nor
  - All
  - ElementMatch
  - AllMatch

### Types
  - Null
  - Boolean
  - Number
  - String
  - Array
  - Object

### Example of MangoBuilder

The bellow code is all-in-one for declaration example purpose. Isn't common to use manny selectors together
```c#
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
        .AddSelector("mod", FluentMango.SelectorOperator.Mod, new int[] { 4, 0 } )
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
        .AddSelector("genre-all-mach", FluentMango.CombinationOperator.AllMatch, "Horror");
```

```json
  {
	"selector": {
		"year": {
			"$eq": 1850
		},
		"year-eq": {
			"$eq": 1900
		},
		"year-lte": {
			"$lte": 1900
		},
		"year-lt": {
			"$lt": 1910
		},
		"year-neq": {
			"$eq": 1900
		},
		"year-gt": {
			"$gt": 1910
		},
		"year-gte": {
			"$gte": 1910
		},
		"name-exists": {
			"$exists": "true"
		},
		"name-type": {
			"$type": "null"
		},
		"year-in": {
			"$in": [1984,
			1991]
		},
		"names-nin": {
			"$in": ["john",
			"mark"]
		},
		"size": {
			"$size": 23
		},
		"mod": {
			"$mod": [4,
			0]
		},
		"regex": {
			"$regex": "^A"
		},
		"$not": {
			"year-not": 1901
		},
		"$or": [{
			"director": "George Lucas"
		},
		{
			"director": "Steven Spielberg"
		}],
		"$and": [{
			"_id": {
				"$gt": null
			}
		},
		{
			"director": {
				"$in": [2014,
				2015]
			}
		}],
		"$nor": [{
			"year1": 1901
		},
		{
			"year2": 1905
		},
		{
			"year3": 1907
		},
		{
			"year4": 1909
		}],
		"genre": {
			"$all": ["Comedy",
			"Short"]
		},
		"genre-element-match": {
			"$elemMatch": {
				"$eq": "Horror"
			}
		},
		"genre-all-mach": {
			"$allMatch": {
				"$eq": "Horror"
			}
		}
	}
}
```