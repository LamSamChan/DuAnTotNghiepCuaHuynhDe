using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using QuanLyHopDongVaKySo.SigningWithUsbToken.InstanceData;
using QuanLyHopDongVaKySo.SigningWithUsbToken.Models;
using QuanLyHopDongVaKySo.SigningWithUsbTokenI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyHopDongVaKySo.SigningWithUsbToken.Repository
{
    public class SendDMinuteRepository
    {
        public HttpClient _httpClient;
        public HttpResponseMessage _httpResponseMessage;
        public SendDMinuteRepository()
        {
            _httpClient = new HttpClient();
            //api
            _httpClient.BaseAddress = new Uri("https://localhost:7063/");
            //
            //_httpClient.BaseAddress = new Uri("https://techseal.azurewebsites.net/");

            _httpClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
        }

        public async Task<int> PostDMinute(DoneMinute doneMinute)
        {
            int result = 0;
            string token = DataStore.Instance.Token;
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            //_httpClient.Timeout = TimeSpan.FromMinutes(5);
            string json = JsonConvert.SerializeObject(doneMinute);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            _httpResponseMessage = await _httpClient.PostAsync($"api/UsbToken/SignMinuteWithUsbToken", content);
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
