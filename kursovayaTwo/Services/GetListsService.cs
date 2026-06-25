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
    public class GetListsService
    {
        private HttpClient client = new HttpClient();

        public GetListsService()
        {
            client.DefaultRequestHeaders.Add("Authorization", "Bearer " + RegisterUser.access_token);
        }
        public async Task<List<LabTests>> getTests()
        {
            var list = await client.GetFromJsonAsync<List<LabTests>>("http://localhost:5043/api/LabTest");
            if (list != null) return list;
            return null!;
        }
        public async Task<List<ProductionBatch>> getBatches()
        {
            var list = await client.GetFromJsonAsync<List<ProductionBatch>>("http://localhost:5043/api/ProductionBatch");
            if (list != null) return list;
            return null!;
        }
        public async Task<List<Recipe>> getRecipes()
        {
            var list = await client.GetFromJsonAsync<List<Recipe>>("http://localhost:5043/api/Recipe");
            if (list != null) return list;
            return null!;
        }
        public async Task<List<TechCard>> getCards()
        {
            var list = await client.GetFromJsonAsync<List<TechCard>>("http://localhost:5043/api/TechCard");
            if (list != null) return list;
            return null!;
        }
        public async Task<List<Product>> GetProducts()
        {
            var list = await client.GetFromJsonAsync<List<Product>>("http://localhost:5043/api/Product");
            if (list != null) return list;
            return null!;
        }
        public async Task<List<User>> GetAllUsers()
        {
            var list = await client.GetFromJsonAsync<List<User>>("http://localhost:5043/api/Users");
            if (list != null) return list;
            return null!;
        }
        public async Task ArchiveProduct(Product product)
        {
            try
            {
                var response = await client.PatchAsync("http://localhost:5043/api/Product/" + product.ProductId, null);

                if (!response.IsSuccessStatusCode)
                    await ShowMessage("Ошибка архивации: " + response.StatusCode);
            }
            catch(Exception ex)
            {
                await ShowMessage(ex.Message);
            }
        }
        public async Task<User> GetUser(int id)
        {
            var user = await client.GetFromJsonAsync<User>("http://localhost:5043/api/Users/" + id);
            if(user != null) return user;
            return null!;
        }
        public async Task<UnitsOfMeasure> GetUom(int id)
        {
            var uom = await client.GetFromJsonAsync<UnitsOfMeasure>("http://localhost:5043/api/Uom/" + id);
            if (uom != null) return uom;
            return null!;
        }
        public async Task<List<UnitsOfMeasure>> GetUom()
        {
            var uom = await client.GetFromJsonAsync<List<UnitsOfMeasure>>("http://localhost:5043/api/Uom" );
            if (uom != null) return uom;
            return null!;
        }
        public async Task<List<TechStep>> GetStep()
        {
            var list = await client.GetFromJsonAsync<List<TechStep>>("http://localhost:5043/api/TechStep");
            if (list != null) return list;
            return null!;
        }
        public async Task<List<Equipment>> GetEquipment()
        {
            var list = await client.GetFromJsonAsync<List<Equipment>>("http://localhost:5043/api/Equipment");
            if (list != null) return list;
            return null!;
        }
        public async Task<List<BatchStepExecution>> GetStepExecutions()
        {
            var list = await client.GetFromJsonAsync<List<BatchStepExecution>>("http://localhost:5043/api/BatchStepExecution");
            if (list != null) return list;
            return null!;
        }
        public async Task<List<LabTests>> GetLabTests()
        {
            var list = await client.GetFromJsonAsync<List<LabTests>>("http://localhost:5043/api/LabTest");
            if (list != null) return list;
            return null!;
        }
        public async Task<List<ExtruderEvent>> GetExtruderEvents()
        {
            var list = await client.GetFromJsonAsync<List<ExtruderEvent>>("http://localhost:5043/api/ExtruderEvent");
            if (list != null) return list;
            return null!;
        }
        public async Task<List<RawMaterial>> GetRawMaterials()
        {
            var list = await client.GetFromJsonAsync<List<RawMaterial>>("http://localhost:5043/api/RawMaterial");
            if (list != null) return list;
            return null!;
        }
        public async Task<List<MaterialBatch>> GetMaterialBatches()
        {
            var list = await client.GetFromJsonAsync<List<MaterialBatch>>("http://localhost:5043/api/MaterialBatch");
            if (list != null) return list;
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
