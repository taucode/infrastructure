dotnet restore

dotnet build --configuration Debug
dotnet build --configuration Release

dotnet test -c Debug .\test\TauCode.Infrastructure.Tests\TauCode.Infrastructure.Tests.csproj
dotnet test -c Release .\test\TauCode.Infrastructure.Tests\TauCode.Infrastructure.Tests.csproj

nuget pack nuget\TauCode.Infrastructure.nuspec
