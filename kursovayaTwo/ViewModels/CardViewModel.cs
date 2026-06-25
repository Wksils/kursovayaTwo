using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using kursovayaTwo.Models;
using kursovayaTwo.Services;
using kursovayaTwo.View.Windows;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace kursovayaTwo.ViewModel
{
    public partial class CardViewModel : ObservableObject
    {
        private GetListsService listService;
        private CardService service;
        private List<TechCard> cards;
        [ObservableProperty]
        private ObservableCollection<TechStep> steps;
        [ObservableProperty]
        private ObservableCollection<TechCard> listCards;

        public CardViewModel()
        {
            listService = new GetListsService();
            service = new CardService();
            Load();
        }
        private void Load()
        {
            var ccards = getCards();
            cards = ccards;
            ListCards = new ObservableCollection<TechCard>(ccards); 
        }
        private List<TechCard> getCards()
        {
            Task<List<TechCard>> cards = Task.Run(()=> listService.getCards());
            return cards.Result;
        }
        [RelayCommand]
        private async Task Add()
        {
            try
            {
                AddEditCardWindow window = new AddEditCardWindow(new TechCard());
                var mainWindow = (App.Current?.ApplicationLifetime as IClassicDesktopStyleApplicationLifetime)?.MainWindow;
                var result = await window.ShowDialog<bool?>(mainWindow);
                if(result == true)
                {
                    TechCard card = window.Card;
                    await service.AddCard(card);
                }
            }
            finally
            {
                Load();
            }
        }
        [RelayCommand]
        private async Task Edit(object param)
        {
            TechCard card = (TechCard)param;
            AddEditCardWindow window = new AddEditCardWindow(card);
            var mainWindow = (App.Current?.ApplicationLifetime as IClassicDesktopStyleApplicationLifetime)?.MainWindow;
            var result = await window.ShowDialog<bool?>(mainWindow);
            if (result == true)
            {
                await service.EditCard(card);
                Load();
            }
        }
        [RelayCommand]
        private async Task Read(object param)
        {
            if(param is TechCard card)
            {
                var window = new CardInfoWindow(card, this);
                await window.ShowDialog((App.Current?.ApplicationLifetime as IClassicDesktopStyleApplicationLifetime)?.MainWindow);
            }
        }
        [RelayCommand]
        private async Task Archive(object param)
        {
            if(param is TechCard card)
            {
                var dialog = new Window
                {
                    Title = "Подтверждение",
                    Width = 300,
                    Height = 120,
                    Content = new TextBlock
                    {
                        Text = "Архивировать Карту?",
                        Margin = new Avalonia.Thickness(10)
                    }
                };
                var result = false;
                dialog.KeyDown += (_, e) =>
                {
                    if (e.Key == Avalonia.Input.Key.Enter) { result = true; dialog.Close(); }
                    if (e.Key == Avalonia.Input.Key.Escape) { dialog.Close(); }
                };
                dialog.ShowDialog((App.Current.ApplicationLifetime as IClassicDesktopStyleApplicationLifetime)?.MainWindow);
                if (result)
                {
                    await service.ArchiveCard(card);
                    Load();
                }
            }
        }
    }
}
