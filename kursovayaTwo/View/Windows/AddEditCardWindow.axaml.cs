using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using kursovayaTwo.Models;
using kursovayaTwo.Services;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace kursovayaTwo.View.Windows;

public partial class AddEditCardWindow : Window
{
    public TechCard Card { get; set; }

    public List<User> Users { get; set; }

    public List<Product> Products { get; set; }

    private TechCard cardCopy;

    private readonly GetListsService service;
    public bool IsActive { get; set; }

    public AddEditCardWindow(TechCard card)
    {
        InitializeComponent();

        Card = card;

        cardCopy = new TechCard
        {
            CardId = card.CardId,
            ProductId = card.ProductId,
            Version = card.Version,
            Status = card.Status,
            IsActive = card.IsActive,
            ApprovedAt = card.ApprovedAt,
            ApprovedBy = card.ApprovedBy,
            CreatedAt = card.CreatedAt,
            CreatedBy = card.CreatedBy,
            Notes = card.Notes
        };

        service = new GetListsService();
        Card.IsActive = cardCopy.IsActive;
        Users = Task.Run(() => service.GetAllUsers()).Result;
        Products = Task.Run(() => service.GetProducts()).Result;

        btn.Content = Card.CardId != null
            ? "Сохранить"
            : "Создать";

        ch1.IsVisible = Card.CardId != null;

        DataContext = cardCopy;
        
        _ = Load();
    }

    private async Task Load()
    {
        await service.getCards();
    }

    private void btn_Click(object? sender, RoutedEventArgs e)
    {
        Card.CardId = cardCopy.CardId;
        Card.ProductId = cardCopy.ProductId;
        Card.Version = cardCopy.Version;
        Card.Status = cardCopy.Status;
        Card.IsActive = cardCopy.IsActive;
        Card.ApprovedAt = cardCopy.ApprovedAt;
        Card.ApprovedBy = cardCopy.ApprovedBy;
        Card.CreatedBy = cardCopy.CreatedBy;
        Card.CreatedAt = cardCopy.CreatedAt;
        Card.Notes = cardCopy.Notes;

        Close(true);
    }

    

}