
REM ----------------------------
REM Construir y levantar AWS_WebApi AWS_WebApp con docker-compose
REM ----------------------------
docker-compose up -d

REM ----------------------------
REM Reconstruir y levantar AWS_WebApi AWS_WebApp con docker-compose
REM ----------------------------
docker-compose up -d --build

REM ----------------------------
REM Detener imagenes
REM ----------------------------
docker-compose down

REM ----------------------------
REM Destruir imagenes
REM ----------------------------
docker images
docker rmi aws_webapi aws_webapp
