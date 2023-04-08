dotnet restore

dotnet build TauCode.Infrastructure.sln -c Debug
dotnet build TauCode.Infrastructure.sln -c Release

dotnet test TauCode.Infrastructure.sln -c Debug
dotnet test TauCode.Infrastructure.sln -c Release

nuget pack nuget\TauCode.Infrastructure.nuspec