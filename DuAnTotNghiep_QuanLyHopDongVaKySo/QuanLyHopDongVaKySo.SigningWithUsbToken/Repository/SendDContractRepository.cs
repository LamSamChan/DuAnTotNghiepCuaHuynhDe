using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using QuanLyHopDongVaKySo.SigningWithUsbToken.InstanceData;
using QuanLyHopDongVaKySo.SigningWithUsbToken.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyHopDongVaKySo.SigningWithUsbToken.Repository
{
    public class SendDContractRepository
    {
        public HttpClient _httpClient;
        public HttpResponseMessage _httpResponseMessage;
        public SendDContractRepository()
        {
            _httpClient = new HttpClient();
            //api
            _httpClient.BaseAddress = new Uri("https://localhost:7063/");
            //
            //_httpClient.BaseAddress = new Uri("https://techseal.azurewebsites.net/");

            _httpClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
        }

        public async Task<int> PostContract(DoneContract doneContract)
        {
            int result = 0;
            string token = DataStore.Instance.Token;
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            //_httpClient.Timeout = TimeSpan.FromMinutes(5);
            string json = JsonConvert.SerializeObject(doneContract);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            _httpResponseMessage = await _httpClient.PostAsync($"api/UsbToken/SignContractWithUsbToken", content);
            if (_httpResponseMessage.IsSuccessStatusCode)
            {
                result = 1;
                return result;
            }
            else
            {
                return result;
            }


        } 
    }
}
