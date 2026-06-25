using Avalonia.Controls;
using Avalonia.Interactivity;
using kursovayaTwo.Models;
using kursovayaTwo.Services;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace kursovayaTwo.View.Windows;

public partial class AddEditRecipe : Window
{
    public Recipe Recipe { get; set; }
    public List<User> Users { get; set; }
    public List<Product> Products { get; set; }

    private Recipe editCopy;
    private GetListsService service;

    public AddEditRecipe(Recipe recipe)
    {
        InitializeComponent();

        Recipe = recipe;

        editCopy = new Recipe
        {
            RecipeId = recipe.RecipeId,
            ProductId = recipe.ProductId,
            Version = recipe.Version,
            Status = recipe.Status,
            ApprovedAt = recipe.ApprovedAt,
            ApprovedBy = recipe.ApprovedBy,
            CreatedBy = recipe.CreatedBy,
            CreatedAt = recipe.CreatedAt,
            Notes = recipe.Notes
        };

        btn.Content = Recipe.RecipeId != null ? "Сохранить" : "Создать";

        DataContext = editCopy;

        service = new GetListsService();

        Users = Task.Run(() => service.GetAllUsers()).Result;
        Products = Task.Run(() => service.GetProducts()).Result;

        Load();
    }

    private async Task Load()
    {
        List<Recipe> recipes = await service.getRecipes();
    }

    private void btn_Click(object? sender, RoutedEventArgs e)
    {
        Recipe.ProductId = editCopy.ProductId;
        Recipe.Version = editCopy.Version;
        Recipe.Status = editCopy.Status;
        Recipe.ApprovedAt = editCopy.ApprovedAt;
        Recipe.ApprovedBy = editCopy.ApprovedBy;
        Recipe.CreatedBy = editCopy.CreatedBy;
        Recipe.CreatedAt = editCopy.CreatedAt;
        Recipe.Notes = editCopy.Notes;

        //DialogResult = true;
    }
}