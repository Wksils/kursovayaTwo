using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using kursovayaTwo.Models;
using kursovayaTwo.Services;
using kursovayaTwo.Views.Windows;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace kursovayaTwo.ViewModel
{
    public partial class RecipeViewModel : ObservableObject
    {
        private GetListsService service;
        private RecipeService recipeService;
        private List<Recipe> allRecipe;
        [ObservableProperty]
        private ObservableCollection<RecipeComponent> components;
        [ObservableProperty]
        private ObservableCollection<Recipe> recipes;
        [ObservableProperty]
        private bool isFilterOpen;

        public RecipeViewModel()
        {
            service = new GetListsService();
            recipeService = new RecipeService();
            Load();
        }
        private void ApplyFilter()
        {
            var filtred = allRecipe.AsEnumerable();
            var checkedStatuses = StatusFilters.Where(p => p.IsChacked).Select(p => p.Value).ToList();
            if (checkedStatuses.Any())
                filtred = filtred.Where(p => checkedStatuses.Contains(p.Status));
            var checkedVersion = VersionFilters.Where(p => p.IsChacked).Select(p => p.Value).ToList();
            if(checkedVersion.Any()) 
                filtred = filtred.Where(p => checkedVersion.Contains(p.Version));
            var checkedProducts = ProductFilters.Where(p => p.IsChacked).Select(p => p.Value).ToList();
            if (checkedProducts.Any())
                filtred = filtred.Where(p => checkedProducts.Contains(p.ProductId.ToString())).ToList();
            Recipes = new ObservableCollection<Recipe>(filtred.ToList());

        }
        public List<FilterOption> VersionFilters { get; set; } = new();
        public List<FilterOption> ProductFilters { get; set; } = new();
        [RelayCommand]
        private void ToggleFilter()
        {
            isFilterOpen = !isFilterOpen;
            OnPropertyChanged(nameof(isFilterOpen));
        }
        [RelayCommand]
        private void ApplyFilterOptions()
        {
            isFilterOpen = false;
            OnPropertyChanged(nameof(isFilterOpen));
            ApplyFilter();
        }
        public List<FilterOption> StatusFilters { get; set; } = new()
        {
            new FilterOption {Value = "draft" },
            new FilterOption {Value = "approved" },
            new FilterOption {Value = "archived" }
        };
        private void Load()
        {
            //Recipes = new ObservableCollection<Recipe>(getRecipes());
            var recipes = getRecipes();
            allRecipe = recipes;
            Recipes = new ObservableCollection<Recipe>(recipes);
            VersionFilters = recipes.Select(p => p.Version).Distinct().Select(p => new FilterOption { Value = p}).ToList();
            var products = Task.Run(() => service.GetProducts()).Result;
            ProductFilters = products.Select(p => new FilterOption { Value = p.ProductId.ToString(), Display = p.Name }).ToList();
            OnPropertyChanged(nameof(ProductFilters));
            OnPropertyChanged(nameof(VersionFilters));

        }
        private List<Recipe> getRecipes()
        {
            Task<List<Recipe>> recipes = Task.Run(() => service.getRecipes());
            return recipes.Result;
        }
        [RelayCommand]
        private async Task Add()
        {
            try
            {
                AddEditRecipe window = new AddEditRecipe(new Recipe());
                if(window.ShowDialog() == true)
                {
                    Recipe recipe = window.Recipe;
                    await recipeService.AddRecipe(recipe);
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
            Recipe recipe = (Recipe)param;
            AddEditRecipe window = new AddEditRecipe(recipe);
            if(window.ShowDialog() == true)
            {
                await recipeService.EditRecipe(recipe);
                Load();
            }
        }
        [RelayCommand]
        private async Task Read(object param)
        {
            if(param is Recipe recipe)
            {
                var window = new RecipeInfo(recipe, this);
                window.ShowDialog();
            }
        }
        [RelayCommand]
        private async Task Archive(object param)
        {
            if(param is Recipe recipe)
            {
                var result = MessageBox.Show("Вы уверены что хотите архивировать рецептуру?", "Подтверждение", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                if(result == MessageBoxResult.Yes)
                {
                    await recipeService.Archive(recipe);
                    Load();
                    Application.Current.Windows.OfType<RecipeInfo>().FirstOrDefault()?.Close();
                    
                }
            }
        }
    }
}
