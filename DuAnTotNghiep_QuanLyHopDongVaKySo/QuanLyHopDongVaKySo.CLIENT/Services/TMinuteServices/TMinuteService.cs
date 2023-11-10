using QuanLyHopDongVaKySo_API.Models;

namespace QuanLyHopDongVaKySo.CLIENT.Services.TMinuteServices
{
    public class TMinuteService : ITMinuteService
    {
        private readonly HttpClient _httpClient;
        public TMinuteService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public Task<int> AddNew(PostTMinute tMinute)
        {
            throw new NotImplementedException();
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

        public Task<TemplateMinute> GetById(int id)
        {
            throw new NotImplementedException();
        }

        public Task<int> Update(PutTMinute tMinute)
        {
            throw new NotImplementedException();
        }
    }
}
