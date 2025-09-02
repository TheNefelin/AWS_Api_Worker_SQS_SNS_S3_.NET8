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
docker build -t aws_webapi:1.0 ./AWS_WebApi

# Limpiar solo cache intermedia
docker builder prune -f

docker build -t aws_webapp:1.0 ./AWS_WebApp

# Limpiar solo cache intermedia
docker builder prune -f

# Mostrar espacio después de builds
df -h

# Limpiar carpeta clonada
cd ..
rm -rf AWS_Api_Worker_SQS_SNS_S3_.NET8
