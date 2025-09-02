﻿using ClassLibrary_Infrastructure.Models;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace ClassLibrary_Infrastructure.Utils;

public class InvoiceGenerator
{
    public Stream CreateStreamPdf(InvoiceData invoiceData)
    {
        // Asegúrate de usar la versión Community de la librería
        // Nota: Solo necesitas establecer esto una vez en tu aplicación, no en cada llamada.
        QuestPDF.Settings.License = LicenseType.Community;

        // Define el documento
        var factura = Document.Create(container =>
        {
            Random random = new Random();
            int num = random.Next(10000000, 99999999 + 1);

            // Define el estilo de página
            container.Page(page =>
            {
                page.Size(PageSizes.A4);
                page.Margin(2, Unit.Centimetre);
                page.DefaultTextStyle(x => x.FontSize(10));

                // ---- CABECERA DE LA FACTURA ----
                page.Header()
                    .Row(row =>
                    {
                        row.RelativeItem().Column(column =>
                        {
                            column.Item().Text("Factura de Venta").FontSize(20).SemiBold();
                            column.Item().Text($"Empresa: {invoiceData.CompanyName}");
                            column.Item().Text("Calle Falsa 123");
                            column.Item().Text("Teléfono: 555-1234");
                            column.Item().Text($"Correo: {invoiceData.CompanyEmail}");
                        });

                        if (invoiceData.ComanyImgStream != null )
                            row.ConstantItem(100).Image(invoiceData.ComanyImgStream).FitArea();
                    });

                // ---- CONTENIDO DE LA FACTURA ----
                page.Content()
                    .PaddingVertical(1, Unit.Centimetre)
                    .Column(column =>
                    {
                        // ---- DETALLES DEL CLIENTE Y FECHA ----
                        column.Item()
                            .Table(table =>
                            {
                                table.ColumnsDefinition(columns =>
                                {
                                    columns.RelativeColumn();
                                    columns.RelativeColumn();
                                });

                                table.Cell().Text("Cliente:");
                                table.Cell().Text($"Número de Factura: {num}");
                                table.Cell().Text($"{invoiceData.Email}");
                                table.Cell().Text("Fecha de Emisión: " + DateTime.Now.ToShortDateString());
                            });

                        // Espacio entre secciones
                        column.Item().PaddingVertical(1, Unit.Centimetre);

                        // ---- TABLA DE ARTÍCULOS ----
                        column.Item()
                            .Table(table =>
                            {
                                table.ColumnsDefinition(columns =>
                                {
                                    columns.RelativeColumn();
                                    columns.ConstantColumn(60);
                                    columns.ConstantColumn(80);
                                    columns.ConstantColumn(80);
                                });

                                table.Header(header =>
                                {
                                    header.Cell().Text("Descripción").Bold();
                                    header.Cell().Text("Cant.").Bold();
                                    header.Cell().Text("P. Unit.").Bold();
                                    header.Cell().Text("Total").Bold();
                                });

                                // Filas de productos
                                foreach (var item in invoiceData.InvoiceProductList)
                                {
                                    table.Cell().Text(item.Name);
                                    table.Cell().Text("1");
                                    table.Cell().Text(item.Price.ToString());
                                    table.Cell().Text(item.Price.ToString());
                                }
                            });

                        // Espacio entre secciones
                        column.Item().PaddingVertical(1, Unit.Centimetre);

                        // ---- TOTALES ----
                        column.Item()
                            .AlignRight()
                            .Text($"Total: {invoiceData.InvoiceProductList.Sum(p => p.Price).ToString()}").FontSize(14).SemiBold();
                    });

                // ---- PIE DE PÁGINA ----
                page.Footer().AlignCenter().Text("Gracias por su compra.");
            });
        });

        // Generamos el PDF en un MemoryStream en lugar de guardarlo en un archivo.
        var stream = new MemoryStream();
        factura.GeneratePdf(stream);
        stream.Position = 0; // Reiniciamos la posición del stream para poder leerlo.
        return stream;
    }
}
