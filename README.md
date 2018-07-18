# CouchDB.Client

This client access the Apache CouchDB (relax) from platform .NET Core 1.1. Builded under the API 2.1.0 this driver works together with RestSharp Simple Http Client and Json.NET popular high-performance JSON framework for .NET.

# Aditional feature futher more API

* Force update (get the last revision and update document)
* Force delete (get the last revision and delete document)

# Nuget

CouchDB can be installed by Nuget package manager.

```
PM> Install-Package CouchDB.Client
```

# Getting started

```c#
var client = new CouchDB.Client();

```

# Features

## Database level
- CreateDatabaseAsync
- GetAllDbsAsync
- GetDatabaseInfoAsync
- DeleteDatabaseAsync

## Document level
- BulkInsertAsync
- DeleteAsync
  - BatchMode
- DumpAsync
- ForceDeleteAsync
- ForceMergeAsync
- ForceUpdateAsync 
- GetAsync
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

# Sample usage


```c#
// create database
var created = await client.CreateDatabaseAsync("test");

// list all databases
var alldbs = await client.ListAllDbsAsync();

// get created database
var db = client.GetDatabase("test");


```