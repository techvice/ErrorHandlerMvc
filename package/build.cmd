mkdir input\lib\net451
del /Q input\lib\net451\*.*

msbuild ..\src\ErrorHandlerMvc\ErrorHandlerMvc.csproj /p:Configuration=Release;OutputPath=..\..\package\input\lib\net451

mkdir output
..\tools\nuget.exe pack /o output input\ErrorHandlerMvc.nuspec