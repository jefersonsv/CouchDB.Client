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

