# Fase de construcción ==========================================
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS builder
WORKDIR /src

# Copiar solo el .csproj primero (optimiza cache)
COPY ["AWS_Api_Worker_SQS_SNS_S3_.NET8.sln", "."]
COPY ["AWS_WebApp/AWS_WebApp.csproj", "AWS_WebApp/"]
RUN dotnet restore "AWS_Api_Worker_SQS_SNS_S3_.NET8.sln"

# Copiar el resto del código
COPY . .
WORKDIR "/src/AWS_WebApp"

# Publicar en modo release
RUN dotnet publish "AWS_WebApp.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Fase final ====================================================
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=builder /app/publish .

# Variables de entorno
ENV DB_HOST=localhost
ENV DB_PORT=5432
ENV DB_NAME=postgres
ENV DB_USER=postgres
ENV DB_PASS=testing
ENV AWS_REGION=us-east-1
ENV AWS_SQS_URL=https://sqs.us-east-1.amazonaws.com/123/my-sqs
ENV AWS_SNS_TOPIC_ARN=arn:aws:sns:us-east-1:123:my-sns
ENV AWS_S3_BUCKET_NAME=monolito-storage

# Configurar Kestrel para escuchar en el puerto 8080
ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080

ENTRYPOINT ["dotnet", "AWS_WebApp.dll"]
