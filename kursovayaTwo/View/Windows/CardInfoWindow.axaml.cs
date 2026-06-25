using Avalonia.Controls;
using Avalonia.Interactivity;
using kursovayaTwo.Models;
using kursovayaTwo.Services;
using kursovayaTwo.ViewModel;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace kursovayaTwo.View.Windows;

public partial class CardInfoWindow : Window
{
    public TechCard TechCard { get; set; }

    private CardService service;

    public List<TechStep> Steps { get; set; }

    private CardViewModel viewModel;

    public CardInfoWindow(TechCard card, CardViewModel cardViewModel)
    {
        InitializeComponent();

        TechCard = card;
        viewModel = cardViewModel;

        service = new CardService();

        Steps = Task.Run(() => service.GetSteps(card.CardId)).Result;

        DataContext = this;
    }

    private void Button_Click(object? sender, RoutedEventArgs e)
    {
        viewModel.ArchiveCommand.Execute(TechCard);
        Close();
    }
}