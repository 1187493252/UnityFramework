@echo off
::本文件在根目录
set "PROTOC_EXE=protoc.exe"
::proto文件目录
set "WORK_DIR=%cd%\Proto"
::CS文件输出目录
set "CS_OUT_PATH=%cd%\ProtobufCS"


for /f "delims=" %%i in ('dir /b Proto "Proto/*.proto"') do (
echo gen %%i...
%PROTOC_EXE%  --proto_path="%WORK_DIR%" --csharp_out="%CS_OUT_PATH%" "%WORK_DIR%\%%i")
)

echo finish

pause
