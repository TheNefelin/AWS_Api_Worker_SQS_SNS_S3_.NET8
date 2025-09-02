using Amazon.SQS.Model;
using ClassLibrary_Domain.Interfaces;
using ClassLibrary_Infrastructure.Models;
using ClassLibrary_Infrastructure.Services;
using ClassLibrary_Infrastructure.Utils;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace ClassLibrary_Application.Services;

public class DonationProcessor : IDonationProcessor
{
    private readonly IConsoleNotifier _consoleNotifier;
    private readonly ILogger<DonationProcessor> _logger;
    private readonly IAwsSqsService _sqsService;
    private readonly IAwsS3Service _s3Service;
    private readonly IAwsSnsService _snsService;
    private readonly ICompanyRepository _companyRepository;
    private readonly IProductRepository _productRepository;
    private readonly InvoiceGenerator _invoiceGenerator;

    public DonationProcessor(
        IConsoleNotifier consoleNotifier, 
        ILogger<DonationProcessor> logger, 
        IAwsSqsService sqsService,
        IAwsS3Service s3Service,
        IAwsSnsService snsService,
        ICompanyRepository companyRepository,
        IProductRepository productRepository,
        InvoiceGenerator invoiceGenerator
        )
    {
        _consoleNotifier = consoleNotifier;
        _logger = logger;
        _sqsService = sqsService;
        _s3Service = s3Service;
        _snsService = snsService;
        _companyRepository = companyRepository;
        _productRepository = productRepository;
        _invoiceGenerator = invoiceGenerator;
    }

    public async Task ProcessDonationAsync(Message sqsMessage)
    {
        try
        {
            await _consoleNotifier.SendConsoleMessage($"🆔 Message ID: {sqsMessage.MessageId}");

            // Deserializar el cuerpo del mensaje SQS
            var sqsBody = JsonSerializer.Deserialize<AwsSqsMessageBody>(sqsMessage.Body);

            if (sqsBody == null)
            {
                await _consoleNotifier.SendConsoleMessage("❌ Error: No se pudo deserializar el cuerpo del mensaje");
                return;
            }

            await _consoleNotifier.SendConsoleMessage($"📋 Subject: {sqsBody.Subject}");

            // Deserializar el mensaje interno de la donación
            var donation = JsonSerializer.Deserialize<AwsSqsDonationMessage>(sqsBody.Message);

            if (donation == null)
            {
                await _consoleNotifier.SendConsoleMessage("❌ Error: No se pudo deserializar el mensaje de donación");
                return;
            }

            // Mostrar información de la donación
            await _consoleNotifier.SendConsoleMessage($"📧 Email: {donation.Email}");
            await _consoleNotifier.SendConsoleMessage($"💰 Amount: {donation.Amount}");

            // Mostrar atributos si existen
            if (sqsMessage.MessageAttributes != null && sqsMessage.MessageAttributes.Any())
            {
                await _consoleNotifier.SendConsoleMessage("📊 Atributos:");
                foreach (var attr in sqsMessage.MessageAttributes)
                {
                    await _consoleNotifier.SendConsoleMessage($"   • {attr.Key}: {attr.Value.StringValue}");
                }
            }

            // Procesar la donación (lógica de negocio aquí)
            await _consoleNotifier.SendConsoleMessage("⚙️ Procesando donación...");

            await _consoleNotifier.SendConsoleMessage("⚙️ Obteniendo Empresa Random desde RDS...");
            var company = await _companyRepository.GetRandomCompanyAsync();

            await _consoleNotifier.SendConsoleMessage("⚙️ Obteniendo Imagen de la Empresa desde S3 /Images...");
            var imgStream = await _s3Service.GetFileStreamFromBucketAsync(company.Img);

            await _consoleNotifier.SendConsoleMessage($"⚙️ Obteniendo Productos Random desde RDS, Cantidad: {donation.Amount}...");
            var productList = await _productRepository.GetRandomProductsAsync(donation.Amount);

            var InvoiceData = new InvoiceData
            {
                CompanyEmail = company.Email,
                CompanyName = company.Name,
                ComanyImgStream = imgStream,
                Email = donation.Email,
                InvoiceProductList = productList.Select(x => new InvoiceProduct
                {
                    Name = x.Name,
                    Price = x.Price,
                }).ToList(),
            };

            await _consoleNotifier.SendConsoleMessage($"⚙️ Generando PDF para donación - Email: {donation.Email}, Productos: {donation.Amount}...");
            var pdfStream = _invoiceGenerator.CreateStreamPdf(InvoiceData);

            await _consoleNotifier.SendConsoleMessage("⚙️ Guardando PDF en S3 /Docs...");
            var pdfFileName = await _s3Service.SavePdfToBucketAsync(pdfStream);

            await _consoleNotifier.SendConsoleMessage("⚙️ Generando URL de descarga para el PDF...");
            var downloadLink = _s3Service.GeneratePreSignedUrl(pdfFileName, TimeSpan.FromDays(7));

            var awsSnsMessageByEmail = new AwsSnsMessageByEmail 
            {
                Email = donation.Email,
                Subject = "🎉 ¡GRACIAS POR TU DONACIÓN! 🎉",
                Message = $@"
                    🎉 ¡GRACIAS POR TU DONACIÓN! 🎉

                    Hola {donation.Email},

                    ✅ Tu donación fue registrada con éxito.

                    📝 DETALLES DE TU PEDIDO:
                       • Pedido: #{Random.Shared.Next(10000000, 99999999)}
                       • Total: ${InvoiceData.InvoiceProductList.Sum(x => x.Price):N0}
                       • Fecha: {DateTime.Now:dd/MM/yyyy HH:mm}

                    🎁 ¡Muchas gracias por tu generosidad!

                    ---------------------------------------------------------------------------
                    Este correo fue generado automáticamente por nuestro sistema de donaciones.
                    ---------------------------------------------------------------------------

                    🔗 DESCARGA TU FACTURA AQUÍ: {downloadLink}"
            };

            await _consoleNotifier.SendConsoleMessage($"⚙️ Enviando Email a: {donation.Email}...");
            await _snsService.PublishMessageByEmailAsync(awsSnsMessageByEmail);

            await _consoleNotifier.SendConsoleMessage("✅ Donación procesada correctamente");

            // Eliminar mensaje de SQS después de procesarlo
            if (!string.IsNullOrEmpty(sqsMessage.ReceiptHandle))
            {
                await _sqsService.DeleteMessageAsync(sqsMessage.ReceiptHandle);
                await _consoleNotifier.SendConsoleMessage($"🗑️ Mensaje {sqsMessage.MessageId} eliminado de SQS");
            }
        }
        catch (Exception ex)
        {
            await _consoleNotifier.SendConsoleMessage($"❌ Error procesando donación: {ex.Message}");
            _logger.LogError(ex, "Error en DonationProcessor");
            throw;
        }
    }
}
