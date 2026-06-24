using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using kursovayaTwo.ViewModel;

namespace kursovayaTwo.View;

public partial class LoginViews : Window
{
    public LoginViews()
    {
        InitializeComponent();
        DataContext = new AuthViewModel(this);
    }
}