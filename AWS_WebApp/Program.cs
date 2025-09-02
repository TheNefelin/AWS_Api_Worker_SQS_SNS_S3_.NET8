using Amazon;
using Amazon.S3;
using Amazon.SimpleNotificationService;
using Amazon.SQS;
using AWS_WebApp.Hubs;
using AWS_WebApp.Services;
using ClassLibrary_Application.Services;
using ClassLibrary_Domain.Interfaces;
using ClassLibrary_Infrastructure.Config;
using ClassLibrary_Infrastructure.Data;
using ClassLibrary_Infrastructure.Repositories;
using ClassLibrary_Infrastructure.Services;
using ClassLibrary_Infrastructure.Utils;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// ============================================================================
// VARIABLES DE ENTORNO
// ============================================================================
var DB_HOST = builder.Configuration["DB_HOST"] ?? "localhost";
var DB_PORT = builder.Configuration["DB_PORT"] ?? "5432";
var DB_NAME = builder.Configuration["DB_NAME"] ?? "postgres";
var DB_USER = builder.Configuration["DB_USER"] ?? "postgres";
var DB_PASS = builder.Configuration["DB_PASS"] ?? "testing";

var AWS_REGION = builder.Configuration["AWS_REGION"] ?? "us-east-1";
var AWS_SQS_URL = builder.Configuration["AWS_SQS_URL"] ?? "https://sqs.us-east-1.amazonaws.com/123/my-sqs";
var AWS_SNS_TOPIC_ARN = builder.Configuration["AWS_SNS_TOPIC_ARN"] ?? "arn:aws:sns:us-east-1:123:my-sns";
var AWS_S3_BUCKET_NAME = builder.Configuration["AWS_S3_BUCKET_NAME"] ?? "monolito-storage";

// ============================================================================
// CONFIGURACIÓN BASE DE DATOS
// ============================================================================
var connectionString = $"Host={DB_HOST};Port={DB_PORT};Database={DB_NAME};Username={DB_USER};Password={DB_PASS}";
builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseNpgsql(connectionString);
});

// ============================================================================
// CONFIGURACIÓN REPOSITORIES
// ============================================================================
builder.Services.AddScoped<ICompanyRepository, CompanyRepository>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();

// ============================================================================
// CONFIGURACIÓN SERVICIOS DE INFRAESTRUCTURE
// ============================================================================
var awsRegion = RegionEndpoint.GetBySystemName(AWS_REGION);
var awsSettings = new AwsSettings
{
    S3BucketName = AWS_S3_BUCKET_NAME,
    SnsTopicArn = AWS_SNS_TOPIC_ARN,
    SqsUrl = AWS_SQS_URL,
};
builder.Services.AddSingleton(awsSettings);
builder.Services.AddSingleton<IAmazonSimpleNotificationService>(new AmazonSimpleNotificationServiceClient(awsRegion));
builder.Services.AddSingleton<IAmazonSQS>(new AmazonSQSClient(awsRegion));
builder.Services.AddSingleton<IAmazonS3>(new AmazonS3Client(awsRegion));
builder.Services.AddScoped<IAwsSnsService, AwsSnsService>();
builder.Services.AddScoped<IAwsSqsService, AwsSqsService>();
builder.Services.AddScoped<IAwsS3Service, AwsS3Service>();

// ============================================================================
// CONFIGURACIÓN DE SERVICIOS
// ============================================================================
builder.Services.AddSignalR();
builder.Services.AddScoped<InvoiceGenerator>();
builder.Services.AddScoped<IDonationProcessor, DonationProcessor>();
builder.Services.AddSingleton<IConsoleNotifier, ConsoleNotifier>();
builder.Services.AddHostedService<SqsBackgroundService>();

var app = builder.Build();

// ============================================================================
// CONFIGURACIÓN INICIO INDEX.HTML
// ============================================================================
app.UseDefaultFiles();

app.UseStaticFiles();
app.UseRouting();

// ============================================================================
// CONFIGURACIÓN SERVICIOS CONSOLE HUB
// ============================================================================
app.MapHub<ConsoleHub>("/consoleHub");

// ============================================================================
// INICIAR SIMULACION DE MENSAJES AUTOMATICOS (Funciona)
// ============================================================================
//var notifier = app.Services.GetRequiredService<IConsoleNotifier>();
//var signal = Task.Run(async () =>
//{
//    int counter = 1;
//    while (true)
//    {
//        await notifier.SendConsoleMessage($"Mensaje de prueba #{counter}");
//        counter++;
//        await Task.Delay(3000);
//    }
//});

app.Run();
