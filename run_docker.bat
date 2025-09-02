@echo off

REM ----------------------------
REM Construir y levantar AWS_WebApi
REM ----------------------------
docker build -t aws_webapi:1.0 ./AWS_WebApi
docker run -d -p 8081:8080 --name AWS_WebApi aws_webapi:1.0

REM ----------------------------
REM Construir y levantar AWS_WebApp
REM ----------------------------
docker build -t aws_webapp:1.0 ./AWS_WebApp
docker run -d -p 8080:8080 --name AWS_WebApp aws_webapp:1.0

echo Contenedores levantados!
pause
