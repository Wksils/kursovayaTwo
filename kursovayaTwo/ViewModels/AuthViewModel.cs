using Avalonia.Controls.ApplicationLifetimes;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using kursovayaTwo.Models;
using kursovayaTwo.Services;
using kursovayaTwo.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.Xml;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
//using System.Windows.Controls;

namespace kursovayaTwo.ViewModel
{
    public partial class AuthViewModel : ObservableObject 
    {
        private string? password_;
        public string? Password
        {
            get => password_;
            set
            {
                password_ = value;
                OnPropertyChanged();
            }
        }
        private readonly AuthService service;
        [ObservableProperty]
        private string login;
        [ObservableProperty]
        private LoginViews window;

        public AuthViewModel(LoginViews _window)
        {
            service = new AuthService();
            window = _window;
        }
        [RelayCommand]
        public async Task Auth(object parametr)
        {
            SignInUser user = new SignInUser();
            user.Login = login;
            user.Password = Password;
            Responce response = await service.SignIn(user);
            if (response != null)
            {
                var desktop = App.Current?.ApplicationLifetime as IClassicDesktopStyleApplicationLifetime;
                if (desktop == null) return;

                var mainWindow = new MainWindow();
                mainWindow.Show();
                var oldMainWindow = desktop.MainWindow;
                desktop.MainWindow = mainWindow;
                oldMainWindow?.Close();
            }
            //else MessageBox.Show("Неверный логин или пароль");
        }
    }
}
