using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using QuanLyHopDongVaKySo.SigningWithUsbToken.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using QuanLyHopDongVaKySo.SigningWithUsbToken.InstanceData;

namespace QuanLyHopDongVaKySo.SigningWithUsbToken.Repository
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

        public async Task<PendingContract> GetPContractById(string customerId, int id)
        {
            string token = DataStore.Instance.Token;
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            _httpResponseMessage = await _httpClient.GetAsync($"api/PContract/GetForWinForm/{customerId}/{id}");
            var json = await _httpResponseMessage.Content.ReadAsStringAsync();
            var pContract = JsonConvert.DeserializeObject<PendingContract>(json);

            if (pContract.DirectorSignedId == null) {
                return new PendingContract();
            }
            return pContract;
        }

    }
}
