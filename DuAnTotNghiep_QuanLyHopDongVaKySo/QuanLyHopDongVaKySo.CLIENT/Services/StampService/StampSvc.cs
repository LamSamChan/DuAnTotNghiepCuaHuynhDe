using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using QuanLyHopDongVaKySo_API.Database;
using QuanLyHopDongVaKySo_API.Models;
using System.Data;
using System.Text;

namespace QuanLyHopDongVaKySo.CLIENT.Services.StampService
{
    public class StampSvc : IStampSvc
    {
        private readonly HttpClient _httpClient;
        public StampSvc(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task<int> AddNew(Stamp stamp)
        {
            string json = JsonConvert.SerializeObject(stamp);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            try
            {
                using (var reponse = await _httpClient.PostAsync("api/Stamp/AddNew", content))
                {

                    if (reponse.IsSuccessStatusCode)
                    {
                        return 1;
                    }
                    else
                    {
                        return 0;
                    }
                }

            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        public async Task<List<Stamp>> GetAll()
        {
            var reponse = await _httpClient.GetFromJsonAsync<List<Stamp>>("api/Stamp");
            return reponse;
        }

        public async Task<Stamp> GetById(int id)
        {
            var reponse = await _httpClient.GetFromJsonAsync<Stamp>($"api/Stamp/{id}");
            return reponse;
        }

        public async Task<int> Update(Stamp stamp)
        {
            var content = new StringContent(JsonConvert.SerializeObject(stamp), Encoding.UTF8, "application/json");
            try
            {
                var reponse = await _httpClient.PutAsync("api/Stamp/Update", content);
                if (reponse.IsSuccessStatusCode)
                {
                    return 1;
                }
                else
                {
                    return 0;
                }

            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        public async Task<int> Delete(int id)
        {
            var response = await _httpClient.DeleteAsync($"api/Stamp/Delete/{id}");

            if (response.IsSuccessStatusCode)
            {
                return id;
            }
            return 0;
        }
    }
}
