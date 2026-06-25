using Avalonia.Controls;
using Avalonia.Interactivity;
using kursovayaTwo.Models;
using kursovayaTwo.Services;
using kursovayaTwo.ViewModel;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace kursovayaTwo.View.Windows;

public partial class RecipeInfo : Window
{
    public Recipe Recipe { get; set; }

    private RecipeService service;
    public List<RecipeComponent> Components { get; set; }

    private RecipeViewModel viewModel;

    public RecipeInfo(Recipe recipe, RecipeViewModel recipeView)
    {
        InitializeComponent();

        Recipe = recipe;
        viewModel = recipeView;

        service = new RecipeService();
        Components = Task.Run(() => service.GetComponents(recipe.RecipeId ?? 0)).Result;

        DataContext = this;
    }

    private void Button_Click(object? sender, RoutedEventArgs e)
    {
        viewModel.ArchiveCommand.Execute(Recipe);
        Close();
    }
}