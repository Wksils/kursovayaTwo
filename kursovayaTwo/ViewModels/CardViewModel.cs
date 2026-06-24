using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using kursovayaTwo.Models;
using kursovayaTwo.Services;
using kursovayaTwo.Views.Windows;
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
                if(window.ShowDialog() == true)
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
            if(window.ShowDialog() == true)
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
                window.ShowDialog();
            }
        }
        [RelayCommand]
        private async Task Archive(object param)
        {
            if(param is TechCard card)
            {
                //var result = MessageBox.Show("Вы уверены что хотите архивировать карту?", "Подтверждение", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                //if(result == MessageBoxResult.Yes)
                //{
                //    await service.ArchiveCard(card);
                //    Load();
                //    Application.Current.Windows.OfType<CardInfoWindow>().FirstOrDefault()?.Close();
                //}
            }
        }
    }
}
