using CashRegister.Shared;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Cashdesk.Logic
{
    public class CashdeskLogic
    {
        private ObservableCollection<Product> products;

        // i tried to extract the logic - but it got to complicated
        private readonly HttpClient httpClient = new HttpClient { BaseAddress = new Uri("http://localhost:5000") };

        public async Task<ObservableCollection<Product>> GetProductsAsync()
        {
            var response = await httpClient.GetAsync("/api/products");
            response.EnsureSuccessStatusCode();
            var responseBody = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<ObservableCollection<Product>>(responseBody, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            return result;
        }


    }
}
