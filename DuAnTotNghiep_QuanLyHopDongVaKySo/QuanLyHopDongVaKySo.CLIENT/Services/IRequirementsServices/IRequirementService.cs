using QuanLyHopDongVaKySo_API.Models;

namespace QuanLyHopDongVaKySo.CLIENT.Services.IRequirementsServices
{
    public class IRequirementService : IIRequirementService
    {
        private readonly HttpClient _httpClient;
        public IRequirementService(HttpClient httpClient) {
            _httpClient = httpClient;
        }
        public async Task<List<InstallationRequirement>> GetAll()
        {
            var response = await _httpClient.GetFromJsonAsync<List<InstallationRequirement>>("api/IRequirements");
            return response;
        }
    }
}
