using QuestPDF.Fluent;
using QuestPDF.Infrastructure;
using Website_Progress.ModelsDTO;

namespace Website_Progress
{
    public class InvoiceDocument : IDocument
    {
        private readonly Order _order;

        public InvoiceDocument(Order order)
        {
            _order = order;
        }

        public DocumentMetadata GetMetadata() => DocumentMetadata.Default;

        public void Compose(IDocumentContainer container)
        {
            container.Page(page =>
            {
                page.Margin(25);

                page.Content().Column(col =>
                {
                    col.Spacing(15);

                    // ===== ЗАГОЛОВОК НА ВСЮ ШИРИНУ =====
                    col.Item().AlignCenter().Text("БЛАНК ПРЕДВАРИТЕЛЬНОГО ЗАКАЗА")
                        .FontSize(22)
                        .Bold();

                    col.Item().LineHorizontal(1);

                    // ===== ИНФОРМАЦИЯ =====
                    col.Item().Row(row =>
                    {
                        row.RelativeItem().Column(left =>
                        {
                            left.Item().Text($"Контактное лицо: {_order.DeliveryUser.Name}");
                            left.Item().Text($"Email: {_order.DeliveryUser.Email}");
                            left.Item().Text($"Телефон: {_order.DeliveryUser.Phone}");
                            left.Item().Text($"Дата создания: {_order.CreationDateTime:dd.MM.yyyy HH:mm}");
                            left.Item().Text($"Адрес: {_order.DeliveryUser.Address}");
                            left.Item().Text($"Комментарий: {_order.DeliveryUser.Comment}");
                        });

                        row.RelativeItem().Column(right =>
                        {
                            right.Item().Text($"Статус: {_order.Status}");
                            right.Item().Text($"Количество позиций: {_order.Items.Count}");
                            right.Item().Text($"Общее количество: {_order.Items.Sum(x => x.Quantity)}");
                        });
                    });

                    col.Item().LineHorizontal(1);

                    // ===== ТАБЛИЦА =====
                    col.Item().Table(table =>
                    {
                        table.ColumnsDefinition(columns =>
                        {
                            columns.ConstantColumn(30);
                            columns.RelativeColumn(3);
                            columns.ConstantColumn(60);
                            columns.ConstantColumn(80);
                            columns.ConstantColumn(100);
                        });

                        table.Header(header =>
                        {
                            header.Cell().Element(CellStyle).Text("№").Bold();
                            header.Cell().Element(CellStyle).Text("Наименование").Bold();
                            header.Cell().Element(CellStyle).Text("Кол-во").Bold();
                            header.Cell().Element(CellStyle).Text("Цена").Bold();
                            header.Cell().Element(CellStyle).Text("Сумма").Bold();
                        });

                        int index = 1;

                        foreach (var item in _order.Items)
                        {
                            table.Cell().Element(CellStyle).Text(index++.ToString());
                            table.Cell().Element(CellStyle).Text(item.Product.Name);
                            table.Cell().Element(CellStyle).Text(item.Quantity.ToString());
                            table.Cell().Element(CellStyle).Text(item.Product.Cost.ToString("0.00"));
                            table.Cell().Element(CellStyle)
                                .Text((item.Product.Cost * item.Quantity).ToString("0.00"));
                        }
                    });

                    // ===== ИТОГО =====
                    col.Item().AlignRight()
                        .Text($"Итого заказа на сумму {_order.Items.Sum(x => x.Product.Cost * x.Quantity):0.00} руб.")
                        .FontSize(14)
                        .Bold();

                    col.Item().PaddingTop(20);

                    // ===== ТЕКСТ ПРО ОПЛАТУ =====
                    col.Item().Text("Оплату необходимо произвести в течение 3 (трёх) календарных дней с момента выставления данного предварительного счёта.")
                        .FontSize(10)
                        .Italic();

                    col.Item().PaddingTop(25);

                    // ===== ПОДПИСИ =====
                    col.Item().Row(row =>
                    {
                        row.RelativeItem().Text("Кто принял заказ __________________________");
                        row.RelativeItem().Text("Подпись клиента __________________________");
                    });
                });
            });
        }

        private static IContainer CellStyle(IContainer container)
        {
            return container
                .Border(1)
                .Padding(5);
        }
    }
}