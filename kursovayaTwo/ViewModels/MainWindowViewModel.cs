using Avalonia.Controls;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using kursovayaTwo.Models;
using kursovayaTwo.Services;
using kursovayaTwo.View;
using kursovayaTwo.ViewModel;
using System.Threading.Tasks;

namespace kursovayaTwo.ViewModel;

public partial class MainWindowViewModel : ObservableObject
{
    private readonly AuthService authService;
    private readonly Window currentWindow;

[ObservableProperty]
    private User currentUser;

    [ObservableProperty]
    private UserControl selectedPage;

    public MainWindowViewModel(Window currentWindow)
    {
        this.currentWindow = currentWindow;

        authService = new AuthService();

        selectedPage = new HomePage();
        selectedPage.DataContext = new HomeViewModel();

        currentUser = LoadCurrentUser();
    }

    private User LoadCurrentUser()
    {
        Task<User> task = Task.Run(() => authService.getUser());
        return task.Result;
    }

    [RelayCommand]
    private void HomePage()
    {
        var page = new HomePage();
        OpenPage(page, new HomeViewModel());
    }

    [RelayCommand]
    private void ProductionPage()
    {
        var page = new ProductionPage();
        OpenPage(page, new ProdictionViewModel());
    }

    [RelayCommand]
    private void RecipesRage()
    {
        var page = new RecipesPage();
        OpenPage(page, new RecipeViewModel());
    }

    [RelayCommand]
    private void ReportRage()
    {
        var page = new reportRage();
        OpenPage(page, new ReportViewModel());
    }

    [RelayCommand]
    private void BatchPage()
    {
        var page = new BatchPage();
        OpenPage(page, new BatchViewModel());
    }

    [RelayCommand]
    private void CardPage()
    {
        var page = new TechCardPage();
        OpenPage(page, new CardViewModel());
    }

    private void OpenPage(UserControl page, ObservableObject viewModel)
    {
        page.DataContext = viewModel;
        SelectedPage = page;
    }

    [RelayCommand]
    private void Logout()
    {
        LoginViews login = new LoginViews();

        login.Show();

        currentWindow.Close();
    }

}
