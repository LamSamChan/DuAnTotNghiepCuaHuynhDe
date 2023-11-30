using Newtonsoft.Json;
using QuanLyHopDongVaKySo.SigningWithUsbToken.InstanceData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyHopDongVaKySo.SigningWithUsbToken.Repository
{
    public class VerifyRepository
    {
        public HttpClient _httpClient;
        public HttpResponseMessage _httpResponseMessage;
        public VerifyRepository()
        {
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri("https://localhost:7286/");

            //_httpClient.BaseAddress = new Uri("https://techsealapi.azurewebsites.net/");

            _httpClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
        }

        public async Task<string> VerifyCustomer(string identification)
        {
            string token = null;
            _httpResponseMessage = await _httpClient.GetAsync($"api/Auth/CustomerVerify/{identification}");
            var json = await _httpResponseMessage.Content.ReadAsStringAsync();
            token = JsonConvert.DeserializeObject<string>(json);
            DataStore.Instance.Token = token;
            return token;
        }
    }
}
