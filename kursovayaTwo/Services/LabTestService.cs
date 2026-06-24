using kursovayaTwo.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;

namespace kursovayaTwo.Services
{
    public class LabTestService
    {
        HttpClient client = new HttpClient();
        public LabTestService() 
        {
            client.DefaultRequestHeaders.Add("Authorization", "Bearer " + RegisterUser.access_token);
        }
        public async Task AddTest(LabTests test)
        {
            try
            {
                JsonContent content = JsonContent.Create(test);
                var response = await client.PostAsync("http://localhost:5043/api/LabTest", content);
                string responseText = await response.Content.ReadAsStringAsync();
                if(responseText != null)
                {
                    LabTests resp = JsonSerializer.Deserialize<LabTests>(responseText)!;
                    if (resp == null) MessageBox.Show(responseText);
                }
            }
            catch(Exception ex) 
            {
                MessageBox.Show(ex.Message);
            }
        }
        public async Task EditTest(LabTests test)
        {
            try
            {
                JsonContent content = JsonContent.Create(test);
                var response = await client.PutAsync("http://localhost:5043/api/LabTest/" + test.TestId, content);
                string responseText = await response.Content.ReadAsStringAsync();
                if (responseText != null)
                {
                    LabTests resp = JsonSerializer.Deserialize<LabTests>(responseText)!;
                    if(resp == null)MessageBox.Show(responseText);
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
