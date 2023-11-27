using Newtonsoft.Json;
using QuanLyHopDongVaKySo.SigningWithUsbToken.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyHopDongVaKySo.SigningWithUsbToken
{
    public class ContractRepository
    {
        public HttpClient _httpClient;
        public HttpResponseMessage _httpResponseMessage;
        public ContractRepository()
        {
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri("https://localhost:7286/");

            //_httpClient.BaseAddress = new Uri("https://techsealapi.azurewebsites.net/");

            _httpClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
        }

        public async Task<PContract> GetPContractById(int id)
        {
            _httpResponseMessage = await _httpClient.GetAsync($"api/PContract/{id}");
            var json = await _httpResponseMessage.Content.ReadAsStringAsync();
            var pContract = JsonConvert.DeserializeObject<PContract>(json);
            return pContract;
        }

    }
}
