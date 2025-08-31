using Amazon;
using Amazon.SimpleNotificationService;
using Amazon.SQS;
using AWS_WebApp.Hubs;
using AWS_WebApp.Services;
using ClassLibrary_Application.Services;
using ClassLibrary_Infrastructure.Config;
using ClassLibrary_Infrastructure.Services;
using Microsoft.AspNetCore.SignalR;

var builder = WebApplication.CreateBuilder(args);

// ============================================================================
// VARIABLES DE ENTORNO
// ============================================================================
var AWS_REGION = builder.Configuration["AWS_REGION"] ?? "us-east-1";
var AWS_SQS_URL = builder.Configuration["AWS_SQS_URL"] ?? "https://sqs.us-east-1.amazonaws.com/123/my-sqs";
var AWS_SNS_TOPIC_ARN = builder.Configuration["AWS_SNS_TOPIC_ARN"] ?? "arn:aws:sns:us-east-1:123:my-sns";
var AWS_S3_BUCKET_NAME = builder.Configuration["AWS_S3_BUCKET_NAME"] ?? "monolito-storage";

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
builder.Services.AddSingleton<IAwsSnsService, AwsSnsService>();
builder.Services.AddSingleton<IAwsSqsService, AwsSqsService>();

// ============================================================================
// CONFIGURACIÓN DE SERVICIOS
// ============================================================================
builder.Services.AddSignalR();
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
