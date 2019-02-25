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

- FindBuilder
  - GreatThen
  - GreatEqualsThan
  - LessThen
  - LessEqualsThan
  - NotEquals
  - Equals
  - Not
  - Nor
  - Exists
  - Regex
  - Type
- Types
  - Null
  - Boolean
  - Number
  - String
  - Array
  - Object
