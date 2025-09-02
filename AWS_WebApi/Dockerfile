# Fase de construcción ==========================================
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS builder
WORKDIR /src

# Copiar solo el .csproj primero (optimiza cache)
COPY ["AWS_Api_Worker_SQS_SNS_S3_.NET8.sln", "."]
COPY ["AWS_WebApi/AWS_WebApi.csproj", "AWS_WebApi/"]
RUN dotnet restore "AWS_WebApi/AWS_WebApi.csproj"

# Copiar el resto del código
COPY . .
WORKDIR "/src/AWS_WebApi"

# Publicar en modo release
RUN dotnet publish "AWS_WebApi.csproj" -c Release -o /app/publish /p:UseAppHost=false

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
ENV AWS_SNS_TOPIC_ARN=arn:aws:sns:us-east-1:123:my-sns

# Configurar Kestrel para escuchar en el puerto 8080
ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080

ENTRYPOINT ["dotnet", "AWS_WebApi.dll"]
