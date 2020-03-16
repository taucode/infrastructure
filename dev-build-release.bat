dotnet restore

dotnet build --configuration Debug
dotnet build --configuration Release

dotnet test -c Debug .\tests\TauCode.Infrastructure.Tests\TauCode.Infrastructure.Tests.csproj
dotnet test -c Release .\tests\TauCode.Infrastructure.Tests\TauCode.Infrastructure.Tests.csproj

nuget pack nuget\TauCode.Infrastructure.nuspec
