using Newtonsoft.Json;
using QuanLyHopDongVaKySo_API.Models;
using System.Text;

namespace QuanLyHopDongVaKySo.CLIENT.Services.TContractServices
{
    public class TContractService : ITContractService
    {
        private readonly HttpClient _httpClient;
        public TContractService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }


        public async Task<int> addAsnyc(PostTContract tContract)
        {
            var content = new StringContent(JsonConvert.SerializeObject(tContract), Encoding.UTF8, "application/json");
            try
            {
                var reponse = await _httpClient.PostAsync("api/TContract", content);
                if (reponse.IsSuccessStatusCode)
                {
                    var tc = await reponse.Content.ReadAsStringAsync();
                    return int.Parse(tc);
                }
                else{ return 0; }
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        public async Task<bool> deleteAsnyc(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<List<TemplateContract>> getAllAsnyc()
        {
            try
            {
                var reponse = await _httpClient.GetFromJsonAsync<List<TemplateContract>>("api/TContract");
                return reponse;
            }
            catch (Exception ex)
            {
                return new List<TemplateContract>();
            }
        }

        public async Task<TemplateContract> getByIdAsnyc(int id)
        {
            var reponse = await _httpClient.GetFromJsonAsync<TemplateContract>($"api/TContract/{id}");
            return reponse;
        }

        public async Task<int> updateAsnyc(PutTContract tContract)
        {
            var content = new StringContent(JsonConvert.SerializeObject(tContract), Encoding.UTF8, "application/json");
            try
            {
                var reponse = await _httpClient.PutAsync("api/TContract", content);
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
