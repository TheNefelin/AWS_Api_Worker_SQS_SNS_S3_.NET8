# AWS WebApi Worker (SQS + SNS + S3) .NET 8

### Project Structure
```
AWS_ApiWeb_Worker_SQS_SNS_S3_.NET8/
│
├── AWS_WebApi/
│	├── Controllers/
│	│	├── CompaniesController.cs
│	│	├── DonationController.cs
│	│	└── ProductsController.cs
│	├── Dockerfile
│	└── Program.cs
│
├── AWS_WebApp/
│	├── wwwroot
│	│	└── index.html
│	├── Hubs/
│	│	└── ConsoleHub.cs
│	├── Services/
│	│	├── ConsoleNotifier.cs
│	│	└── SqsBackgroundService.cs
│	├── Dockerfile
│	└── Program.cs
│
├── ClassLibrary_Application/
│	├── DTOs
│	│	├── CompanyDTO.cs
│	│	└── ProductDTO.cs
│	├── Mappers
│	│	├── CompanyMapper.cs
│	│	└── ProductMapper.cs
│	├── Models
│	│	├── ApiResponse.cs
│	│	└── Donation.cs
│	└── Models
│		├── CompanyService.cs
│		├── DonationService.cs
│		├── ICompanyService.cs
│		├── IConsoleNotifier.cs
│		├── IDonationService.cs
│		├── IProductService.cs
│		└── ProductService.cs
│
├── ClassLibrary_Domain/
│	├── Entities
│	│	├── Company.cs
│	│	└── Product.cs
│	└── Interfaces
│		├── ICompanyRepository.cs
│		└── IProductRepository.cs
│
├── ClassLibrary_Infrastructure/
│	├── Config
│	│	└── AwsSettings.cs
│	├── Data
│	│	└── AppDbContext.cs
│	├── Models
│	│	├── AwsSnsDonation.cs
│	│	├── AwsSnsEmail.cs
│	│	├── AwsSnsMessage.cs
│	│	└── AwsSnsMessageByEmail.cs
│	├── Repositories
│	│	├── CompanyRepository.cs
│	│	└── ProductRepository.cs
│	└── Services
│		├── AwsS3Service.cs
│		├── AwsSnsService.cs
│		├── AwsSqsService.cs
│		├── IAwsS3Service.cs
│		├── IAwsSnsService.cs
│		└── IAwsSqsService.cs
│
└── AWS_ApiWeb_Worker_SQS_SNS_S3_.NET8.sln
```

### Environment Variables
- WebApi
```csharp
var DB_HOST = builder.Configuration["DB_HOST"] ?? "localhost";
var DB_PORT = builder.Configuration["DB_PORT"] ?? "5432";
var DB_NAME = builder.Configuration["DB_NAME"] ?? "postgres";
var DB_USER = builder.Configuration["DB_USER"] ?? "postgres";
var DB_PASS = builder.Configuration["DB_PASS"] ?? "testing";

var AWS_REGION = builder.Configuration["AWS_REGION"] ?? "us-east-1";
var AWS_SNS_TOPIC_ARN = builder.Configuration["AWS_SNS_TOPIC_ARN"] ?? "arn:aws:sns:us-east-1:123:my-sns";
```
- WebApp
```csharp
var AWS_REGION = builder.Configuration["AWS_REGION"] ?? "us-east-1";
var AWS_SQS_URL = builder.Configuration["AWS_SQS_URL"] ?? "https://sqs.us-east-1.amazonaws.com/123/my-sqs";
var AWS_SNS_TOPIC_ARN = builder.Configuration["AWS_SNS_TOPIC_ARN"] ?? "arn:aws:sns:us-east-1:123:my-sns";
var AWS_S3_BUCKET_NAME = builder.Configuration["AWS_S3_BUCKET_NAME"] ?? "monolito-storage";
```

# AWS - Amazon Web Services
## SQS

## SNS

## Subscription
- Subscription filter policy (SQS)
```json
{
 "action": [
    "process"
 ]
}
```

- Subscription filter policy (email)
```json
{
  "target": [
    "test@email.com",
    "all"
  ]
}
```

---