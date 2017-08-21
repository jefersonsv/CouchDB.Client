ECHO OFF
REM http://docs.nuget.org/ndocs/tools/nuget.exe-cli-reference
nuget\nuget.exe update -self

if not exist nuget\package mkdir nuget\package
if not exist nuget\package\lib mkdir nuget\package\lib
if not exist nuget\package\lib\4.5 mkdir nuget\package\lib\4.5

REM Find msbuild
REM reg.exe query "HKLM\SOFTWARE\Microsoft\MSBuild\ToolsVersions\4.0" /v MSBuildToolsPath
C:\Windows\Microsoft.NET\Framework64\v4.0.30319\msbuild src\CouchDB.Client.sln -p:Configuration=Release
copy src\CouchDB.Client\bin\Release\CouchDB.Client.dll nuget\package\lib\4.5\

nuget\nuget.exe pack nuget\package.nuspec -BasePath nuget\package -OutputDirectory nuget\

REM Push
REM https://www.nuget.org/packages/manage/upload