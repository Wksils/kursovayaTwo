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
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace kursovayaTwo.ViewModel
{
    public partial class ProdictionViewModel : ObservableObject
    {
        private GetListsService listsService;
        private ProductService ProductService;
        private List<ProductRow> _allRows;
        [ObservableProperty]
        private ProductRow selectedRow;
        [ObservableProperty]
        private ObservableCollection<ProductRow> productRows;
        [ObservableProperty]
        private ObservableCollection<string> codeList;
        [ObservableProperty]
        private ObservableCollection<string> nameList;
        [ObservableProperty]
        private string selectedCode;
        [ObservableProperty]
        private string selectedName;
        [ObservableProperty]
        private bool isFilterOpen;

        public ProdictionViewModel()
        {
            listsService = new GetListsService();
            ProductService = new ProductService();
            Load();
        }
        private void Load()
        {
            var products = Task.Run(() => listsService.GetProducts()).Result;
            var recipes = Task.Run(() => listsService.getRecipes()).Result;
            var techCards = Task.Run(() => listsService.getCards()).Result;
            var batches = Task.Run(() => listsService.getBatches()).Result;

            _allRows = products.Select(p => new ProductRow
            {
                Product = p,
                Recipes = recipes.Where(r => r.ProductId == p.ProductId).ToList()!,
                TechCards = techCards.Where(t => t.ProductId == p.ProductId).ToList()!,
                Batch = batches.FirstOrDefault(b => b.ProductId == p.ProductId)!
            }).ToList();

            ProductRows = new ObservableCollection<ProductRow>(_allRows);
            CodeList = new ObservableCollection<string>(_allRows.Select(r => r.Product.Code).Distinct());
            NameList = new ObservableCollection<string>(_allRows.Select(r => r.Product.Name).Distinct());
        }
        partial void OnSelectedCodeChanged(string value) => ApplyFilter();
        partial void OnSelectedNameChanged(string value) => ApplyFilter();
        private void ApplyFilter()
        {
            var filtered = _allRows.AsEnumerable();
            if (!string.IsNullOrEmpty(SelectedCode))
                filtered = filtered.Where(r => r.Product.Code == SelectedCode);
            if (!string.IsNullOrEmpty(SelectedName))
                filtered = filtered.Where(r => r.Product.Name == SelectedName);
            var checkedStatuses = StatusFilters.Where(f => f.IsChacked).Select(f => f.Value).ToList();
            if (checkedStatuses.Any())
                filtered = filtered.Where(r => checkedStatuses.Contains(r.Product.Status));

            var checkedTypes = TypeFilters.Where(f => f.IsChacked).Select(f => f.Value).ToList();
            if (checkedTypes.Any())
                filtered = filtered.Where(r => checkedTypes.Contains(r.Product.ProductType));
            ProductRows = new ObservableCollection<ProductRow>(filtered.ToList());
        }
        partial void OnSelectedRowChanged(ProductRow value)
        {
            if (value == null) return;
            var window = new ProductWindow(value, this);
            window.Show();
            SelectedRow = null!;
        }
        public List<FilterOption> StatusFilters { get; set; } = new()
        {
            new FilterOption { Value = "active" },
            new FilterOption { Value = "development" },
            new FilterOption { Value = "discontinued" }
        };
        public List<FilterOption> TypeFilters { get; set; } = new()
        {
            new FilterOption { Value = "порошок" },
            new FilterOption { Value = "раствор" },
            new FilterOption { Value = "мазь" },
            new FilterOption { Value = "сироп" },
            new FilterOption { Value = "капсулы" },
            new FilterOption { Value = "таблетки" }
        };
        [RelayCommand]
        private void ToggleFilter()
        {
            isFilterOpen = !isFilterOpen;
            OnPropertyChanged(nameof(IsFilterOpen));
        }
        [RelayCommand]
        private void ApplyFilterOptions()
        {
            isFilterOpen = false;
            OnPropertyChanged(nameof(IsFilterOpen));
            ApplyFilter(); 
        }
        [RelayCommand]
        private async Task Edit(object param)
        {
            Product product = (Product)param;
            AddEditProduct window = new AddEditProduct(product);
            var mainWindow = (App.Current?.ApplicationLifetime as IClassicDesktopStyleApplicationLifetime)?.MainWindow;
            var result = await window.ShowDialog<bool?>(mainWindow);
            if (result == true)
            {
                await ProductService.EditProduct(product);
                Load();
            }
        }
        [RelayCommand]
        private async Task Add()
        {
            try
            {
                AddEditProduct window = new AddEditProduct(new Product());
                var mainWindow = (App.Current?.ApplicationLifetime as IClassicDesktopStyleApplicationLifetime)?.MainWindow;
                var result = await window.ShowDialog<bool?>(mainWindow);
                if (result == true)
                {
                    Product product = window.Product;
                    await ProductService.AddProduct(product);
                }
            }
            finally
            {
                Load();
            }
        }

        [RelayCommand]
        private async Task Read(object param)
        {
            if (param is ProductRow row)
            {
                var window = new ProductWindow(row, this);
                await window.ShowDialog((App.Current.ApplicationLifetime as IClassicDesktopStyleApplicationLifetime)?.MainWindow);
            }
        }
        [RelayCommand]
        private async Task Archive(object param)
        {
            if (param is Product product)
            {
                var dialog = new Window
                {
                    Title = "Подтверждение",
                    Width = 300,
                    Height = 120,
                    Content = new TextBlock
                    {
                        Text = "Архивировать Продукт?",
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
                    await listsService.ArchiveProduct(product);
                    Load();
                }
            }
        }
    }
}
