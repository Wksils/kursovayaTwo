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
    public class BatchService
    {
        HttpClient client = new HttpClient();
        public BatchService() 
        {
            client.DefaultRequestHeaders.Add("Authorization", "Bearer " + RegisterUser.access_token);
        }
        public async Task AddBatch(MaterialBatch  batch)
        {
            try
            {
                JsonContent content = JsonContent.Create(batch);
                var response = await client.PostAsync("http://localhost:5043/api/MaterialBatch", content);
                string responseText = await response.Content.ReadAsStringAsync();
                if(responseText != null)
                {
                    MaterialBatch resp = JsonSerializer.Deserialize<MaterialBatch>(responseText)!;
                    if (resp == null) await ShowMessage(responseText);
                }
            }
            catch(Exception ex)
            {
                await ShowMessage(ex.Message);
            }
        }
        public async Task EditBatch(MaterialBatch batch)
        {
            try
            {
                JsonContent content = JsonContent.Create(batch);
                var response = await client.PutAsync("http://localhost:5043/api/MaterialBatch/" + batch.BatchId, content);
                string responseText = await response.Content.ReadAsStringAsync();
                if(responseText != null)
                {
                    MaterialBatch resp = JsonSerializer.Deserialize<MaterialBatch>(responseText)!;
                    if (resp == null) await ShowMessage(responseText);
                }
            }
            catch(Exception ex)
            {
                await ShowMessage(ex.Message);
            }
        }
        public async Task<RawMaterial> GetMaterial(int id)
        {
            var material = await client.GetFromJsonAsync<RawMaterial>("http://localhost:5043/api/RawMaterial/" + id);
            if (material != null) return material;
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
