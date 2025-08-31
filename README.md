# AWS WebApi Worker SQS SNS S3 .NET 8

### Estructura del Proyecto
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