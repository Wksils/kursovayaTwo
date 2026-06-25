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
    public class RecipeService
    {
        HttpClient client = new HttpClient();

        public RecipeService()
        {
            client.DefaultRequestHeaders.Add("Authorization", "Bearer " + RegisterUser.access_token);
        }
        public async Task AddRecipe(Recipe recipe)
        {
            try
            {
                recipe.RecipeId = 0;
                JsonContent content = JsonContent.Create(recipe);
                var response = await client.PostAsync("http://localhost:5043/api/Recipe", content);
                string responseText = await response.Content.ReadAsStringAsync();
                if(!string.IsNullOrWhiteSpace(responseText))
                {
                    Recipe recipe1 = JsonSerializer.Deserialize<Recipe>(responseText)!;
                    if(recipe1 == null)
                    {
                        await ShowMessage(responseText);
                    }
                }
            }
            catch (Exception ex)
            {
                await ShowMessage(ex.Message);
            }
        }
        public async Task EditRecipe(Recipe recipe)
        {
            try
            {
                JsonContent content = JsonContent.Create(recipe);
                var response = await client.PutAsync("http://localhost:5043/api/Recipe/" + recipe.RecipeId, content);
                string responseText = await response.Content.ReadAsStringAsync();
                if (responseText != null)
                {
                    Recipe recipe1 = JsonSerializer.Deserialize<Recipe>(responseText)!;
                    if (recipe1 == null) await ShowMessage(responseText);
                }
            }
            catch (Exception ex)
            {
                await ShowMessage(ex.Message);
            }
        }
        public async Task Archive(Recipe recipe)
        {
            try
            {
                var response = await client.PatchAsync("http://localhost:5043/api/Recipe/" + recipe.RecipeId, null);
                if (!response.IsSuccessStatusCode)
                    await ShowMessage("Ошибка архивации");
            }
            catch (Exception ex)
            {
                await ShowMessage(ex.Message);
            }
        }
        public async Task<List<RecipeComponent>> GetComponents(int id)
        {
            var list = await client.GetFromJsonAsync<List<RecipeComponent>>("http://localhost:5043/api/RecipeComponent/byRecipe/" + id);
            return list ?? new List<RecipeComponent>();
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


