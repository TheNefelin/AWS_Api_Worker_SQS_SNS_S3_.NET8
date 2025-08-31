using Amazon;
using Amazon.SimpleNotificationService;
using ClassLibrary_Application.Services;
using ClassLibrary_Core.Interfaces;
using ClassLibrary_Infrastructure.Config;
using ClassLibrary_Infrastructure.Data;
using ClassLibrary_Infrastructure.Repositories;
using ClassLibrary_Infrastructure.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// ============================================================================
// VARIABLES DE ENTORNO
// ============================================================================
var DB_HOST = builder.Configuration["DB_HOST"] ?? "localhost";
var DB_PORT = builder.Configuration["DB_PORT"] ?? "5432";
var DB_NAME = builder.Configuration["DB_NAME"] ?? "postgres";
var DB_USER = builder.Configuration["DB_USER"] ?? "postgres";
var DB_PASS = builder.Configuration["DB_PASS"] ?? "testing";

var AWS_REGION = builder.Configuration["AWS_REGION"] ?? "us-east-1";
var AWS_SNS_TOPIC_ARN = builder.Configuration["AWS_SNS_TOPIC_ARN"] ?? "arn:aws:sns:us-east-1:123:my-sns";

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
    SnsTopicArn = AWS_SNS_TOPIC_ARN
};
builder.Services.AddSingleton(awsSettings);
builder.Services.AddSingleton<IAmazonSimpleNotificationService>(new AmazonSimpleNotificationServiceClient(awsRegion));
builder.Services.AddScoped<IAwsSnsService, AwsSnsService>();

// ============================================================================
// CONFIGURACIÓN SERVICIOS DE APLICACIÓN
// ============================================================================
builder.Services.AddScoped<ICompanyService, CompanyService>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<IDonationService, DonationService>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// ============================================================================
// CONFIGURACIÓN PIPELINE
// ============================================================================
// Configurar Swagger para todos los entornos
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("./swagger/v1/swagger.json", "Donations API v1");
    c.RoutePrefix = string.Empty;
    c.DisplayRequestDuration();
});

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
