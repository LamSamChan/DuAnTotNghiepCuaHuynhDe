using QuanLyHopDongVaKySo_API.Models;
using QuanLyHopDongVaKySo_API.Models.ViewPuts;
using QuanLyHopDongVaKySo_API.Models.ViewPost;
namespace QuanLyHopDongVaKySo.CLIENT.Services.CustomerServices
{
    public class CustomerService : ICustomerService
    {
        private readonly HttpClient _httpClient;
        public CustomerService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task<string> AddNewCustomer(PostCustomer customer)
        {
            var reponse = await _httpClient.PostAsJsonAsync("api/Customers/AddNew", customer);
            reponse.EnsureSuccessStatusCode();
            return await reponse.Content.ReadAsStringAsync();
        }

        public async Task<List<Customer>> GetAllCustomers()
        {
            var response = await _httpClient.GetFromJsonAsync<List<Customer>>("api/Customers");
            return response;
        }

        public async Task<Customer> GetCustomerById(string id)
        {
            var response = await _httpClient.GetFromJsonAsync<Customer>($"api/Customers/{id}");
            return response;
        }

        public async Task<string> UpdateCustomer(PutCustomer customer)
        {
            var reponse = await _httpClient.PutAsJsonAsync("api/Customers/Update", customer);
            reponse.EnsureSuccessStatusCode();
            return await reponse.Content.ReadAsStringAsync();
        }
    }
}
