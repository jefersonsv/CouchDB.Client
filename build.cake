///////////////////////////////////////////////////////////////////////////////
// ARGUMENTS
///////////////////////////////////////////////////////////////////////////////
var target = Argument("target", "Default");
var configuration = Argument("configuration", "Release");
var nogit = false;

var fullFrameworkTarget = "net452";
var netStandardTarget = "netstandard2.0";
var netCoreTarget = "netcoreapp2.0";
var nugetSource = "https://api.nuget.org/v3/index.json";
var version = string.Empty;

// Directories
var output = Directory("build");
var outputBinaries = output + Directory("binaries");
var outputBinariesCore = outputBinaries + Directory(netCoreTarget);
var outputBinariesNetstandard = outputBinaries + Directory(netStandardTarget);
var outputPackages = output + Directory("packages");
var outputNuGet = output + Directory("nuget");

// Files
var zipFile = outputPackages + File("CouchDB-Client-Latest.zip");

///////////////////////////////////////////////////////////////////////////////
// SETUP / TEARDOWN
///////////////////////////////////////////////////////////////////////////////

Setup(ctx =>
{
   // Executed BEFORE the first task.
   Information("Cleaning Folders");

   CleanDirectories(new DirectoryPath[] {
            output,
            outputBinaries,
            outputPackages,
            outputNuGet,
            outputBinariesCore,
            outputBinariesNetstandard
        });

    CleanDirectories("./src/**/" + configuration);
    CleanDirectories("./test/**/" + configuration);
    CleanDirectories("./samples/**/" + configuration);
});

// Teardown(ctx =>
// {
//    // Executed AFTER the last task.
//    Information("Finished running tasks.");
// });

///////////////////////////////////////////////////////////////////////////////
// TASKS
///////////////////////////////////////////////////////////////////////////////
Task("Restore-NuGet-Packages")
    .Description("Restores NuGet packages for all projects")
    .Does(() =>
    {
        DotNetCoreRestore(new DotNetCoreRestoreSettings {
            ArgumentCustomization = args => {
                args.Append("--verbosity minimal");
                return args;
            }
        });

        Information("NuGet packages restored");
    });
    
Task("Compile")
    .Description("Builds all the projects in the solution")
    .IsDependentOn("Restore-NuGet-Packages")
    .Does(() =>
    {
        var projects =
            GetFiles("./**/*.csproj");

        foreach(var project in projects)
        {
            var content =
                System.IO.File.ReadAllText(project.FullPath, Encoding.UTF8);

            if (IsRunningOnUnix() && content.Contains(">" + fullFrameworkTarget + "<"))
            {
                Information(project.GetFilename() + " only supports " +fullFrameworkTarget + " and cannot be built on *nix. Skipping.");
                continue;
            }

            DotNetCoreBuild(project.GetDirectory().FullPath, new DotNetCoreBuildSettings {
                ArgumentCustomization = args => {
                    if (IsRunningOnUnix())
                    {
                        args.Append(string.Concat("-f ", project.GetDirectory().GetDirectoryName().Contains(".Tests") ? netCoreTarget : netStandardTarget));
                    }

                    return args;
                },
                Configuration = configuration
            });

            Information("Project Compiled: " + project);
        }
    });

Task("Publish")
    .Description("Gathers output files and copies them to the output folder")
    .IsDependentOn("Compile")
    .Does(() =>
    {
        // Copy netcore binaries.
        CopyFiles(GetFiles("./src/**/bin/" + configuration + "/" + netCoreTarget + "/*.dll")
            + GetFiles("./src/**/bin/" + configuration + "/" + netCoreTarget + "/*.xml")
            + GetFiles("./src/**/bin/" + configuration + "/" + netCoreTarget + "/*.pdb")
            , outputBinariesCore);

        // Copy netstandard binaries.
        CopyFiles(GetFiles("./src/**/bin/" + configuration + "/" + netStandardTarget + "/*.dll")
            + GetFiles("./src/**/bin/" + configuration + "/" + netStandardTarget + "/*.xml")
            + GetFiles("./src/**/bin/" + configuration + "/" + netStandardTarget + "/*.pdb")
            , outputBinariesNetstandard);

        Information("Binaries published: ./" + outputBinaries);
    });

Task("Package-Zip")
    .Description("Zips up the built binaries for easy distribution")
    .IsDependentOn("Publish")
    .Does(() =>
    {
        var files =
            GetFiles(outputBinaries.Path.FullPath + "/**/*");

        Zip(outputBinaries, zipFile, files);

        Information("Zip file generated: ./" + zipFile);
    });

Task("Update-Version")
    .IsDependentOn("Package-Zip")
    .Does(() =>
    {
        var projects =
            GetFiles("./**/*.csproj");

        foreach(var projectInfo in projects)
        {
            var project =
                System.IO.File.ReadAllText(projectInfo.ToString(), Encoding.UTF8);

            var projectVersion =
                new System.Text.RegularExpressions.Regex(@"<Version>(?<ver>.+)<\/Version>");

            var match = projectVersion.Match(project);
            if (match.Success)
            {
                var currentVersion = match.Groups["ver"].Value;
                var nextVersion = string.Empty;
                var ver = System.Version.Parse(currentVersion);

                // Increase version
                if (ver.MinorRevision > -1)
                    nextVersion = string.Format("{0}.{1}.{2}.{3}", ver.Major, ver.Minor, ver.Build, ver.MinorRevision + 1);
                else if (ver.Build > -1)
                    nextVersion = string.Format("{0}.{1}.{2}", ver.Major, ver.Minor, ver.Build + 1);
                else if (ver.Minor > -1)
                    nextVersion = string.Format("{0}.{1}", ver.Major, ver.Minor + 1);
                //else
                //throw new InvalidOperationException("The version number is not valid");

                if (!string.IsNullOrWhiteSpace(nextVersion))
                {
                    project =
                        projectVersion.Replace(project, string.Concat("<Version>", nextVersion, "</Version>"));

                    System.IO.File.WriteAllText(projectInfo.ToString(), project, Encoding.UTF8);
                }
            }
        }
    });

Task("Package-NuGet")
    .Description("Generates NuGet packages for each project")
    .IsDependentOn("Update-Version")
    .Does(() =>
    {
        foreach(var project in GetFiles("./src/**/*.csproj"))
        {
            Information("Packaging " + project.GetFilename().FullPath);

            var content =
                System.IO.File.ReadAllText(project.FullPath, Encoding.UTF8);

            if (IsRunningOnUnix() && content.Contains(">" + fullFrameworkTarget + "<"))
            {
                Information(project.GetFilename() + " only supports " +fullFrameworkTarget + " and cannot be packaged on *nix. Skipping.");
                continue;
            }

            DotNetCorePack(project.GetDirectory().FullPath, new DotNetCorePackSettings {
                Configuration = configuration,
                OutputDirectory = outputNuGet
            });

            Information("Package generated: " + project.GetDirectory().FullPath);
        }
    });

Task("Publish-NuGet")
    .Description("Pushes the nuget packages in the nuget folder to a NuGet source. Also publishes the packages into the feeds.")
    .IsDependentOn("Package-NuGet")
    .Does(() =>
    {
        var packages =
            GetFiles(outputNuGet.Path.FullPath + "/*.nupkg") -
            GetFiles(outputNuGet.Path.FullPath + "/*.symbols.nupkg");

        foreach(var package in packages)
        {
            NuGetPush(package, new NuGetPushSettings {
                Source = nugetSource
                // ApiKey = apiKey
            });

            Information("Nuget pushed: " + package);
        }
    });

Task("Prepare-Release")
    .IsDependentOn("Publish-NuGet")
    .Does(() =>
    {
        // Add
        foreach (var file in GetFiles("./**/*.csproj"))
        {
            if (nogit)
            {
                Information("git " + string.Format("add {0}", file.FullPath));
            }
            else
            {
                StartProcess("git", new ProcessSettings {
                    Arguments = string.Format("add {0}", file.FullPath)
                });
            }
        }

        // Commit
        if (nogit)
        {
            Information("git " + string.Format("commit -m \"Updated version to {0}\"", version));
        }
        else
        {
            StartProcess("git", new ProcessSettings {
                Arguments = string.Format("commit -m \"Updated version to {0}\"", version)
            });
        }

        // Tag
        if (nogit)
        {
            Information("git " + string.Format("tag \"v{0}\"", version));
        }
        else
        {
            StartProcess("git", new ProcessSettings {
                Arguments = string.Format("tag \"v{0}\"", version)
            });
        }

        //Push
        if (nogit)
        {
            Information("git push origin master");
            Information("git push --tags");
        }
        else
        {
            StartProcess("git", new ProcessSettings {
                Arguments = "push origin master"
            });

            StartProcess("git", new ProcessSettings {
                Arguments = "push --tags"
            });
        }
    });

Task("Default")
    .IsDependentOn("Compile")
    .Does(() => {
        Information("Compilation Executed!");
    });

RunTarget(target);