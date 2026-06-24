using kursovayaTwo.Models;
using System;
using System.Collections.Generic;
using System.DirectoryServices;
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
                if (responseText != null)
                {
                    Recipe recipe1 = JsonSerializer.Deserialize<Recipe>(responseText)!;
                    if (recipe1 == null) MessageBox.Show(responseText);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
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
                    if (recipe1 == null) MessageBox.Show(responseText);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        public async Task Archive(Recipe recipe)
        {
            try
            {
                var response = await client.PatchAsync("http://localhost:5043/api/Recipe/" + recipe.RecipeId, null);
                if (!response.IsSuccessStatusCode)
                    MessageBox.Show("Ошибка архивации");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        public async Task<List<RecipeComponent>> GetComponents(int id)
        {
            var list = await client.GetFromJsonAsync<List<RecipeComponent>>("http://localhost:5043/api/RecipeComponent/byRecipe/" + id);
            return list ?? new List<RecipeComponent>();
        }
    }
}
