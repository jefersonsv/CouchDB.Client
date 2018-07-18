#tool nuget:?package=NUnit.ConsoleRunner&version=3.4.0
//////////////////////////////////////////////////////////////////////
// ARGUMENTS
//////////////////////////////////////////////////////////////////////

var target = Argument("target", "Default");
var configuration = Argument("configuration", "Release");

//////////////////////////////////////////////////////////////////////
// PREPARATION
//////////////////////////////////////////////////////////////////////

// Directories
var output = Directory("./build");
var binariesBinaries = output + Directory("binaries") + Directory(configuration);
var publishBinaries = output + Directory("publish") + Directory(configuration);
var packagesBinaries = output + Directory("packages") + Directory(configuration);

// Define variables
var version = Argument<string>("targetversion", "2.0.0-pre" + (EnvironmentVariable("APPVEYOR_BUILD_NUMBER") ?? "0"));
var projectFiles = GetFiles("./src/**/*.csproj");


//////////////////////////////////////////////////////////////////////
// TASKS
//////////////////////////////////////////////////////////////////////

Task("Clean")
    .Does(() =>
{
	CleanDirectories(new DirectoryPath[] {
		binariesBinaries,
		publishBinaries
	});
});

Task("Restore-NuGet-Packages")
    .IsDependentOn("Clean")
    .Does(() =>
{
	DotNetCoreRestore("./src"); // use it instead of NuGetRestore("./folder");
});

Task("Build")
    .IsDependentOn("Restore-NuGet-Packages")
    .Does(() =>
{
    if(IsRunningOnWindows())
    {
      // Use MSBuild
      //MSBuild("./CouchDB.Client.sln", settings =>
        //settings.SetConfiguration(configuration));
		
		/*
		 var settings = new DotNetCoreBuildSettings
		 {
			 Framework = "netcoreapp2.0",
			 Configuration = configuration,
			 OutputDirectory = binariesBinaries
		 };
		 
		 var settingsStandard = new DotNetCoreBuildSettings
		 {
			 Framework = "netstandard2.0",
			 Configuration = configuration,
			 OutputDirectory = binariesBinaries
		 };
*/
		 DotNetCoreBuild("./src/CouchDB.Client.sln");
			  
		// http://cakebuild.net/api/Cake.Common.Tools.DotNetCore/DotNetCoreAliases/0D79A1B5
		//DotNetCoreBuild("./src/CouchDB.Client.Core/CouchDB.Client.Core.csproj");
		
		//DotNetCoreBuild("./src/CouchDB.Client.Core.Console/CouchDB.Client.Core.Console.csproj");

    }
	/*
    else
    {
      // Use XBuild
      XBuild("./CouchDB.Client.sln", settings =>
        settings.SetConfiguration(configuration));
    }
	*/
});

Task("Package")
	.IsDependentOn("Build")
	.Does(() =>
{
	 
	 var settings = new DotNetCorePackSettings
     {
         Configuration = configuration,
         OutputDirectory = packagesBinaries
     };
	 
	DotNetCorePack("./src/CouchDB.Client/CouchDB.Client.csproj", settings);
	 
});

Task("Publish")
	.IsDependentOn("Package")
	.Does(() =>
{
	 
	 var settings = new DotNetCorePublishSettings
     {
         Framework = "netstandard2.0",
         Configuration = configuration,
         OutputDirectory = publishBinaries
     };

     DotNetCorePublish("./src/CouchDB.Client/CouchDB.Client.csproj", settings);
});

/*
Task("Run-Unit-Tests")
    .IsDependentOn("Build")
    .Does(() =>
{
    NUnit3("./src/** /bin/" + configuration + "/*.Tests.dll", new NUnit3Settings {
        NoResults = true
        });
});
*/

Task("Update-Version")
    .Does(() =>
    {
        Information("Setting version to " + version);

        if(string.IsNullOrWhiteSpace(version)) {
            throw new CakeException("No version specified! You need to pass in --targetversion=\"x.y.z\"");
        }

        UpdateProjectJsonVersion(version, projectFiles);
    });
	
//////////////////////////////////////////////////////////////////////
// TASK TARGETS
//////////////////////////////////////////////////////////////////////

Task("Default")
	.IsDependentOn("Update-Version")
	.IsDependentOn("Publish");
    //.IsDependentOn("Run-Unit-Tests");

//////////////////////////////////////////////////////////////////////
// EXECUTION
//////////////////////////////////////////////////////////////////////

RunTarget(target);

public static void UpdateProjectJsonVersion(string version, FilePathCollection filePaths)
{
    foreach (var file in filePaths)
    {
        var project =
            System.IO.File.ReadAllText(file.FullPath, Encoding.UTF8);

		// https://msdn.microsoft.com/pt-br/library/ewy2t5e0(v=vs.110).aspx
        var projectVersion =
            new System.Text.RegularExpressions.Regex("(?<open><Version>).+(?<close><\\/Version>)");

		project = projectVersion.Replace(project, "${open}" + version + "${close}");

        System.IO.File.WriteAllText(file.FullPath, project, Encoding.UTF8);
    }
}
