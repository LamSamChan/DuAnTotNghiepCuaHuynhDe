using Newtonsoft.Json;
using QuanLyHopDongVaKySo.CLIENT.Models;
using QuanLyHopDongVaKySo.CLIENT.Models.ModelPost;
using QuanLyHopDongVaKySo.CLIENT.Models.ModelPut;
using System.Text;

namespace QuanLyHopDongVaKySo.CLIENT.Services.CustomerServices
{
    public class CustomerService : ICustomerService
    {
        private readonly HttpClient _httpClient;
        public CustomerService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task<int> AddNewCustomer(PostCustomer customer)
        {
            string json = JsonConvert.SerializeObject(customer);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            using (var response = await _httpClient.PostAsync("api/Customers/AddNew", content))
            {
                if (response.IsSuccessStatusCode)
                {
                    return 1;
                }
                return 0;
            }
        }

        public async Task<List<Customer>> GetAllCustomers()
        {
            var response = await _httpClient.GetFromJsonAsync<List<Customer>>("api/Customers");
            return response;
        }

        public async Task<PutCustomer> GetCustomerById(string id)
        {
            var response = await _httpClient.GetFromJsonAsync<PutCustomer>($"api/Customers/{id}");
            return response;
        }

        public async Task<string> UpdateCustomer(PutCustomer customer)
        {
            string json = JsonConvert.SerializeObject(customer);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            using (var response = await _httpClient.PutAsync("api/Customers/Update", content))
            {
                if (response.IsSuccessStatusCode)
                {
                    return customer.CustomerId.ToString();
                }
                return null;
            }
        }
    }
}
