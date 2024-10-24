@ECHO OFF

chcp 65001

dotnet publish fos.markirovka.csproj --configuration Release --runtime win-arm64 --self-contained -p:PublishSingleFile=true
dotnet publish fos.markirovka.csproj --configuration Release --runtime win-amd64 --self-contained -p:PublishSingleFile=true

move bin\Release\net8.0\win-x64\publish\fos.markirovka.exe bin\Release\net8.0\win-x64\publish\fos.markirovka.x64.exe
cd bin\Release\net8.0\win-x64\publish\
tar -a -c -f "..\..\..\..\..\1ОС.Маркировка ЧестныйЗнак.x64.exe.zip" fos.markirovka.x64.exe
cd ..\..\..\..\..\

move bin\Release\net8.0\win-arm64\publish\fos.markirovka.exe bin\Release\net8.0\win-arm64\publish\fos.markirovka.arm64.exe
cd bin\Release\net8.0\win-arm64\publish\
tar -a -c -f "..\..\..\..\..\1ОС.Маркировка ЧестныйЗнак.arm64.exe.zip" fos.markirovka.arm64.exe
cd ..\..\..\..\..\