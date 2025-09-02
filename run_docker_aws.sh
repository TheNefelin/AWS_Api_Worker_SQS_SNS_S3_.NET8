#!/bin/bash

# Limpiar Docker (opcional, solo contenedores y recursos huérfanos)
docker system prune -a --volumes -f

# Mostrar espacio disponible
df -h

# Clonar repositorio limpio
rm -rf AWS_Api_Worker_SQS_SNS_S3_.NET8
git clone https://github.com/TheNefelin/AWS_Api_Worker_SQS_SNS_S3_.NET8.git

cd AWS_Api_Worker_SQS_SNS_S3_.NET8

# Build de imágenes
docker build -f AWS_WebApi/Dockerfile -t aws_webapi:latest .

# Limpiar solo cache intermedia
df -h
docker builder prune -f
df -h

docker build -f AWS_WebApp/Dockerfile -t aws_webapp:latest .

# Limpiar solo cache intermedia
df -h
docker builder prune -f
df -h

# Mostrar espacio después de builds
df -hclear

# Limpiar carpeta clonada
cd ..
rm -rf AWS_Api_Worker_SQS_SNS_S3_.NET8

docker images
