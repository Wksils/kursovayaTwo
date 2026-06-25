using Avalonia.Controls;
using Avalonia.Interactivity;
using kursovayaTwo.Models;
using kursovayaTwo.Services;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace kursovayaTwo.View.Windows;

public partial class AddEditProduct : Window
{
    public Product Product { get; set; }

    private Product _editCopy;

    private ProductService service;

    private GetListsService listsService;

    public List<string> ProductTypes { get; set; } =
    [
        "порошок",
        "раствор",
        "мазь",
        "сироп",
        "капсулы",
        "таблетки"
    ];

    public List<string> ReleaseForms { get; set; } =
    [
        "банка",
        "пакет",
        "туба",
        "ампула",
        "флакон",
        "блистер"
    ];

    public List<string> Statuses { get; set; } =
    [
        "active",
        "development"
    ];

    public AddEditProduct(Product product)
    {
        InitializeComponent();

        Product = product;

        _editCopy = new Product
        {
            ProductId = product.ProductId,
            Code = product.Code,
            Name = product.Name,
            ProductType = product.ProductType,
            ReleaseForm = product.ReleaseForm,
            Status = product.Status,
            CreatedAt = product.CreatedAt
        };

        btn.Content = Product.Name != null
            ? "Сохранить"
            : "Создать";

        DataContext = _editCopy;

        service = new ProductService();
        listsService = new GetListsService();

        _ = Load();
    }

    private async Task Load()
    {
        await listsService.GetProducts();
    }

    private async void btn_Click(object? sender, RoutedEventArgs e)
    {
        if (string.IsNullOrWhiteSpace(_editCopy.Code) ||
            string.IsNullOrWhiteSpace(_editCopy.Name))
        {
            var msgBox = new Window
            {
                Width = 350,
                Height = 120,
                Title = "Ошибка",
                Content = new TextBlock
                {
                    Text = "Заполните код и название",
                    VerticalAlignment = Avalonia.Layout.VerticalAlignment.Center,
                    HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Center
                }
            };

            await msgBox.ShowDialog(this);
            return;
        }

        Product.Code = _editCopy.Code;
        Product.Name = _editCopy.Name;
        Product.ProductType = _editCopy.ProductType;
        Product.ReleaseForm = _editCopy.ReleaseForm;
        Product.Status = _editCopy.Status;

        Close(true);
    }
}