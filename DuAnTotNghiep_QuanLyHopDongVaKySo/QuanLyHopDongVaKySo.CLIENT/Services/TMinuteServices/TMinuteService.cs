using Newtonsoft.Json;
using QuanLyHopDongVaKySo_API.Models;
using System.Text;

namespace QuanLyHopDongVaKySo.CLIENT.Services.TMinuteServices
{
    public class TMinuteService : ITMinuteService
    {
        private readonly HttpClient _httpClient;
        public TMinuteService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task<int> AddNew(PostTMinute tMinute)
        {
            var json = JsonConvert.SerializeObject(tMinute);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            try
            {
                var reponse = await _httpClient.PostAsync("api/TMinute", content);
                if (reponse.IsSuccessStatusCode)
                {
                    int tMinuteID = 0;
                    bool result = Int32.TryParse(await reponse.Content.ReadAsStringAsync(), out tMinuteID);
                    if (result)
                    {
                        return tMinuteID;
                    }
                    else
                    {
                        return 0;
                    }
                }
                else { return 0; }
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        public Task<bool> Delete(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<List<TemplateMinute>> GetAll()
        {
            var reponse = await _httpClient.GetFromJsonAsync<List<TemplateMinute>>("api/TMinute");
            return reponse;
        }

        public async Task<TemplateMinute> GetById(int id)
        {
            var reponse = await _httpClient.GetFromJsonAsync<TemplateMinute>($"api/TMinute/{id}");
            return reponse;
        }

        public async Task<int> Update(PutTMinute tMinute)
        {
            var content = new StringContent(JsonConvert.SerializeObject(tMinute), Encoding.UTF8, "application/json");
            try
            {
                var reponse = await _httpClient.PutAsync("api/TMinute", content);
                if (reponse.IsSuccessStatusCode)
                {
                    var tc = await reponse.Content.ReadAsStringAsync();
                    return int.Parse(tc);
                }
                else { return 0; }
            }
            catch (Exception ex)
            {
                return 0;
            }
        }
    }
}
