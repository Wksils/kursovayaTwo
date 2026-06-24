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
    public class ProductService
    {
        HttpClient client = new HttpClient();

        public ProductService()
        {
             client.DefaultRequestHeaders.Add("Authorization", "Bearer " + RegisterUser.access_token);
        }
        public async Task AddProduct(Product product)
        {
            try
            {
                JsonContent content = JsonContent.Create(product);
                var responce = await client.PostAsync("http://localhost:5043/api/Product", content);
                string responseText = await responce.Content.ReadAsStringAsync();
                if(responseText != null)
                {
                    Product resp = JsonSerializer.Deserialize<Product>(responseText)!;
                    if (resp == null) MessageBox.Show(responseText);
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        public async Task EditProduct(Product product)
        {
            try
            {
                JsonContent content = JsonContent.Create(product);
                var response = await client.PutAsync("http://localhost:5043/api/Product/" + product.ProductId, content);
                string responseText = await response.Content.ReadAsStringAsync();
                if(responseText != null)
                {
                    Product resp = JsonSerializer.Deserialize<Product>(responseText)!;
                    if (resp == null) MessageBox.Show(responseText);
                }
            }
            catch( Exception ex ) 
            {
                MessageBox.Show(ex.Message);
            }
        }
        public async Task<Product> GetProduct(int id)
        {
            //client.DefaultRequestHeaders.Add("Authorization", "Bearer " + RegisterUser.access_token);
            var product = await client.GetFromJsonAsync<Product>("http://localhost:5043/api/Product/" + id);
            if (product != null) return product;
            return null!;
        }
    }
}
