dotnet publish -c release -f net6.0 -r win-x64 --self-contained true -p:PublishSingleFile=True -o bin\Release\net6.0\publish\Davis-win-x64\

dotnet publish -c release -f net6.0 -r linux-x64 --self-contained true -p:PublishSingleFile=True -o bin\Release\net6.0\publish\Davis-linux-x64\
