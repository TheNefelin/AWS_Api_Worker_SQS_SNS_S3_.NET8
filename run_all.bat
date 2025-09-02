@echo off
REM Levantar AWS_WebApi
start "AWS_WebApi" dotnet run --project AWS_WebApi\AWS_WebApi.csproj

REM Levantar AWS_WebApp
start "AWS_WebApp" dotnet run --project AWS_WebApp\AWS_WebApp.csproj

echo Ambos servicios levantados.
pause
