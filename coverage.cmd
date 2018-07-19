REM https://github.com/lucaslorentz/minicover
REM https://github.com/lucaslorentz/minicover/blob/master/sample/build.sh

cd test
cd minicover
dotnet restore
dotnet build

rem workdir will write coverage.json
dotnet minicover instrument --workdir .\ --assemblies ../../test/**/bin/**/*.dll --sources ../../src/**/*.cs 

dotnet minicover reset

rem for project in test/**/*.csproj; do dotnet test --no-build $project; done

dotnet test --no-build ..\..\test\CouchDB.Client.Test\CouchDB.Client.Test.csproj
dotnet minicover uninstrument --workdir ./
dotnet minicover report --workdir ./ --threshold 25
cd ..
cd ..