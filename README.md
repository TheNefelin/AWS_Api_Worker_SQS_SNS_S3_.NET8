# AWS WebApi Worker (SQS + SNS + S3) .NET 8

```sh
tr -d '\r' < run_docker_aws.sh > run_docker_aws_fixed.sh
sh run_docker_aws_fixed.sh
```

### Project Structure
```
AWS_ApiWeb_Worker_SQS_SNS_S3_.NET8/
│
├── AWS_WebApi/
│   ├── Controllers/
│   │   ├── CompaniesController.cs
│   │   ├── DonationController.cs
│   │   └── ProductsController.cs
│   ├── Dockerfile
│   └── Program.cs
│
├── AWS_WebApp/
│   ├── wwwroot
│   │   └── index.html
│   ├── Hubs/
│   │   └── ConsoleHub.cs
│   ├── Services/
│   │   ├── ConsoleNotifier.cs
│   │   └── SqsBackgroundService.cs
│   ├── Dockerfile
│   └── Program.cs
│
├── ClassLibrary_Application/
│   ├── DTOs
│   │   ├── CompanyDTO.cs
│   │   └── ProductDTO.cs
│   ├── Mappers
│   │   ├── CompanyMapper.cs
│   │   └── ProductMapper.cs
│   ├── Models
│   │   └── ApiResponse.cs
│   └── Models
│       ├── CompanyService.cs
│       ├── DonationProcessor.cs
│       ├── DonationService.cs
│       ├── ICompanyService.cs
│       ├── IConsoleNotifier.cs
│       ├── IDonationProcessor.cs
│       ├── IDonationService.cs
│       ├── IProductService.cs
│       └── ProductService.cs
│
├── ClassLibrary_Domain/
│   ├── Entities
│   │   ├── Company.cs
│   │   └── Product.cs
│   └── Interfaces
│       ├── ICompanyRepository.cs
│       └── IProductRepository.cs
│
├── ClassLibrary_Infrastructure/
│   ├── Config
│   │	└── AwsSettings.cs
│   ├── Data
│   │	└── AppDbContext.cs
│   ├── Models
│   │	├── AwsSnsDonation.cs
│   │	├── AwsSnsEmail.cs
│   │	├── AwsSnsMessage.cs
│   │	├── AwsSnsMessageByEmail.cs
│   │	├── AwsSqsDonationMessage.cs
│   │	├── AwsSqsMessageBody.cs
│   │	├── InvoiceData.cs
│   │	└── InvoiceProduct.cs
│   ├── Repositories
│   │	├── CompanyRepository.cs
│   │	└── ProductRepository.cs
│   ├── Services
│   │   ├── AwsS3Service.cs
│   │   ├── AwsSnsService.cs
│   │   ├── AwsSqsService.cs
│   │   ├── IAwsS3Service.cs
│   │   ├── IAwsSnsService.cs
│   │   └── IAwsSqsService.cs
│   └── Utils
│       └── InvoiceGenerator.cs
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
var DB_HOST = builder.Configuration["DB_HOST"] ?? "localhost";
var DB_PORT = builder.Configuration["DB_PORT"] ?? "5432";
var DB_NAME = builder.Configuration["DB_NAME"] ?? "postgres";
var DB_USER = builder.Configuration["DB_USER"] ?? "postgres";
var DB_PASS = builder.Configuration["DB_PASS"] ?? "testing";

var AWS_REGION = builder.Configuration["AWS_REGION"] ?? "us-east-1";
var AWS_SQS_URL = builder.Configuration["AWS_SQS_URL"] ?? "https://sqs.us-east-1.amazonaws.com/123/my-sqs";
var AWS_SNS_TOPIC_ARN = builder.Configuration["AWS_SNS_TOPIC_ARN"] ?? "arn:aws:sns:us-east-1:123:my-sns";
var AWS_S3_BUCKET_NAME = builder.Configuration["AWS_S3_BUCKET_NAME"] ?? "monolito-storage";
```

# AWS - Amazon Web Services
## Seciruty Group
### SG RDS
- **Name**: monolito-sg-rds
- **Inbound rules**:
  - PostgreSQL
    - Type: PostgreSQL
    - Port range: 5432
    - Destination: 0.0.0.0/0

### SG WEB
- **Name**: monolito-sg-web
- **Inbound rules**:
  - SSH
    - Type: SSH
    - Port range: 22
    - Destination: 0.0.0.0/0
  - HTTP
    - Type: HTTP
    - Port range: 80
    - Destination: 0.0.0.0/0

## S3 Bucket
### Bucket
- **Name**: monolito-storage
- **Block all public access**: check

```bash
monolito-storage/
├── docs/
└── images/
```

## RDS
### PostgreSQL
- **Engine type**: PostgreSQL
- **DB instance**: monolito-pgdb-rds
- **Master username**: postgres
- **Credentials management**: ********
- **Instance configuration**: db.t3.micro
- **Allocated storage**: 20 GiB
- **Security groups**: monolito-sg-rds 

## SQS
### Topics
- **Type**: Standard
- **Name**: monolito-sqs
- **Visibility timeout**: 30 Seconds
- **Message retention period**: 4 Days
- **Delivery delay**: 0
- **Receive message wait time**: 0
- **Maximum message size**: 1024 KiB

## SNS
### Topics
- **Topics**: Standard
- **Name**: monolito-sns

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
