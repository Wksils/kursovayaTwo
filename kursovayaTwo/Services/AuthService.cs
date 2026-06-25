using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using kursovayaTwo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;

namespace kursovayaTwo.Services
{
    public class AuthService
    {
        private HttpClient client = new HttpClient();
        public async Task<Responce> SignIn(SignInUser user)
        {
            JsonContent content = JsonContent.Create(user);
            using var response = await client.PostAsync("http://localhost:5043/api/Account/token", content);
            string responseText = await response.Content.ReadAsStringAsync();
            if(response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                Responce resp = JsonSerializer.Deserialize<Responce>(responseText)!;
                RegisterUser.usernsme = resp.username;
                RegisterUser.access_token = resp.access_token;
                return resp;
            }
            return null!;
        }
        public async Task<User> getUser()
        {
            try
            {
                client.DefaultRequestHeaders.Add("Authorization", "Bearer " + RegisterUser.access_token);
                var response = await client.GetFromJsonAsync<User>("http://localhost:5043/api/Account/info");
                if (response != null) return response;
            }
            catch(HttpRequestException ex)
            {
                await ShowMessage(ex.Message);
            }
            return null!;
        }
        private async Task ShowMessage(string text)
        {
            var window = new Window
            {
                Width = 300,
                Height = 150,
                Title = "Ошибка",
                Content = new TextBlock
                {
                    Text = text,
                    Margin = new Avalonia.Thickness(10)
                }

            };
            var owner = (App.Current.ApplicationLifetime as IClassicDesktopStyleApplicationLifetime).MainWindow;
            await window.ShowDialog(owner);
        }
    }
}
