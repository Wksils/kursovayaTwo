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
    public class CardService
    {
        HttpClient client = new HttpClient();
        public CardService()
        {
            client.DefaultRequestHeaders.Add("Authorization", "Bearer " + RegisterUser.access_token);    
        }
        public async Task AddCard(TechCard card)
        {
            try
            {
                JsonContent content = JsonContent.Create(card);
                var response = await client.PostAsync("http://localhost:5043/api/TechCard", content);
                string responseText = await response.Content.ReadAsStringAsync();
                if(responseText != null)
                {
                    TechCard resp = JsonSerializer.Deserialize<TechCard>(responseText)!;
                    if (resp == null) MessageBox.Show(responseText);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        public async Task EditCard(TechCard card)
        {
            try
            {
                JsonContent content = JsonContent.Create(card);
                var response = await client.PutAsync("http://localhost:5043/api/TechCard/" + card.CardId, content);
                string responseText = await response.Content.ReadAsStringAsync();
                if(responseText != null)
                {
                    TechCard resp = JsonSerializer.Deserialize<TechCard>(responseText)!;
                    if (resp == null) MessageBox.Show(responseText);
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        public async Task ArchiveCard(TechCard card)
        {
            try
            {
                var response = await client.PatchAsync("http://localhost:5043/api/TechCard/" + card.CardId, null);
                if(!response.IsSuccessStatusCode)
                    MessageBox.Show("Ошибка архивации: " + response.StatusCode);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        public async Task<List<TechStep>> GetSteps(int id)
        {
            var list = await client.GetFromJsonAsync<List<TechStep>>("http://localhost:5043/api/TechStep/byCard/" + id);
            return list ?? new List<TechStep>();
        }
    }
}
