@echo off
for %%i in (protobuf/*.proto) do (
    protoc -I=./protobuf --csharp_out=../Server/MyGame/Protobuf %%i
    protoc -I=./protobuf --csharp_out=../Client/Assets/Scripts/Protobuf %%i
    echo exchange %%i To csharp file successfully!  
)

@pause