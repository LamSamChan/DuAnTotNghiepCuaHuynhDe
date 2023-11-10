using Newtonsoft.Json;
using QuanLyHopDongVaKySo.CLIENT.Models;
using API = QuanLyHopDongVaKySo_API.Models;
using QuanLyHopDongVaKySo_API.Models.ViewPost;
using QuanLyHopDongVaKySo_API.Models.ViewPuts;
using System.Net.Http;
using System.Text;

namespace QuanLyHopDongVaKySo.CLIENT.Services.TOSServices
{
    public class TOSService : ITOSService
    {
        private readonly HttpClient _httpClient;
        public TOSService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task<int> AddNew(PostTOS typeOfService)
        {
            string json = JsonConvert.SerializeObject(typeOfService);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            try
            {
                using (var reponse = await _httpClient.PostAsync("api/TypeOfServices/AddNew", content))
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

        public async Task<List<TypeOfService>> GetAll()
        {
            var reponse = await _httpClient.GetFromJsonAsync<List<TypeOfService>>("api/TypeOfServices");
            return reponse;
        }

        public async Task<TypeOfService> GetById(int typeOfService_ID)
        {
            var reponse = await _httpClient.GetFromJsonAsync<TypeOfService>($"api/TypeOfServices/{typeOfService_ID}");
            return reponse;
        }

        public Task<int> Update(PutTOS typeOfService)
        {
            throw new NotImplementedException();
        }
    }
}
