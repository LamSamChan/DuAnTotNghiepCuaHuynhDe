using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using QuanLyHopDongVaKySo.CLIENT.Constants;
using QuanLyHopDongVaKySo.CLIENT.Models;
using QuanLyHopDongVaKySo.CLIENT.Models.ModelPost;
using QuanLyHopDongVaKySo.CLIENT.Models.ModelPut;
using System.Net.Http.Headers;
using System.Text;

namespace QuanLyHopDongVaKySo.CLIENT.Services.CustomerServices
{
    public class CustomerService : ICustomerService
    {
        private readonly HttpClient _httpClient;
        private readonly IHttpContextAccessor _httpContextAccessor;


        private string token;

        public CustomerService(HttpClient httpClient, IHttpContextAccessor httpContextAccessor)
        {
            _httpClient = httpClient;
            _httpContextAccessor = httpContextAccessor;
        }
        public string Token
        {
            get
            {
                if (!String.IsNullOrEmpty(_httpContextAccessor.HttpContext.Session.GetString("token")))
                {
                    token = _httpContextAccessor.HttpContext.Session.GetString("token");

                }
                else
                {
                    token = _httpContextAccessor.HttpContext.Session.GetString(SessionKey.Customer.CustomerToken);
                }
                return token;
            }
            set { this.token = value; }
        }
        public async Task<int> AddNewCustomer(PostCustomer customer)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token);
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
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token);
            var response = await _httpClient.GetFromJsonAsync<List<Customer>>("api/Customers");
            return response;
        }

        public async Task<PutCustomer> GetCustomerByIdPut(string id)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token);
            var response = await _httpClient.GetFromJsonAsync<PutCustomer>($"api/Customers/{id}");
            return response;
        }

        public async Task<Customer> GetCustomerById(string id)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token);
            var response = await _httpClient.GetFromJsonAsync<Customer>($"api/Customers/{id}");
            return response;
        }

        public async Task<string> UpdateCustomer(PutCustomer customer)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token);
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
