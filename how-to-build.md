How to build
============

*NOTE* These instructions are *only* for building with Cake - if you just want to build manually you can do so just by loading the solution into Visual Studio 2017 and pressing build :-)

Building
--------

1. At the command prompt, navigate to the root folder (should contain build.cake)
2. To run the default build (which will compile, test and package) type the following command:
  * On Windows cmd type:
    ./build --targetversion="2.1.2"
  * On Windows PowerShell type: 
    ./build.ps1 --targetversion="2.1.2"
  * On Linux/MacOS type:
    ./build.sh

*NOTE* On Linux/MacOS you need to have Mono >= 4.4 installed because targets .NET 4.5.2 for the full-framework build

After the build has completed, there will be a new folder in the root called "build". It contains the following folders:

  * binaries -> All the assembilies and their dependencies
  * packages -> Zip file containing the binaries (other configurations might be added in the future)
  * nuget -> NuGet packages generated from this build