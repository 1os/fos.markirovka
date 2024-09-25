@ECHO OFF

dotnet publish fos.markirovka.csproj --configuration Release --runtime win-arm64 --self-contained
dotnet publish fos.markirovka.csproj --configuration Release --runtime win-amd64 --self-contained
dotnet publish fos.markirovka.csproj --configuration Release --runtime win-x86 --self-contained