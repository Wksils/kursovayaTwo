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
                MainWindow main = new MainWindow();
                main.Show();
                window.Close();
            }
           // else MessageBox.Show("Неверный логин или пароль");
        }
    }
}
