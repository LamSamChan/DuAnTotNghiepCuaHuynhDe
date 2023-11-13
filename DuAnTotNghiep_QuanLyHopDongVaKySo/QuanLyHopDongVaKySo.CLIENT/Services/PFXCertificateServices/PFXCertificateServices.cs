using Newtonsoft.Json;
using QuanLyHopDongVaKySo_API.Models;
using QuanLyHopDongVaKySo_API.Models.ViewDeletes;
using QuanLyHopDongVaKySo_API.Models.ViewPost;
using QuanLyHopDongVaKySo_API.Services.PFXCertificateService;
using System.Net.Http.Json;
using System.Text;

namespace QuanLyHopDongVaKySo.CLIENT.Services.PFXCertificateServices
{
    public class PFXCertificateServices : IPFXCertificateServices
    {
        private readonly HttpClient _httpClient;
        public PFXCertificateServices(HttpClient httpClient) {
            _httpClient = httpClient;
        }


        public async Task<List<PFXCertificate>> GetAll()
        {
            var response = await _httpClient.GetFromJsonAsync<List<PFXCertificate>>("api/PFXCertificates");
            return response;
        }

        public async Task<List<PFXCertificate>> GetAllAboutToExpire()
        {
            var response = await _httpClient.GetFromJsonAsync<List<PFXCertificate>>("api/PFXCertificates/AboutToExpire");
            return response;
        }

        public async Task<List<PFXCertificate>> GetAllExpire()
        {
            var response = await _httpClient.GetFromJsonAsync<List<PFXCertificate>>("api/PFXCertificates/Expire");
            return response;
        }

        public async Task<PFXCertificate> GetById(string serial)
        {
            var response = await _httpClient.GetFromJsonAsync<PFXCertificate>($"api/PFXCertificates/{serial}");
            return response;
        }

        public async Task<string> UpdateNotAfter(string serial)
        {
            using (var response = await _httpClient.PostAsJsonAsync<string>($"api/PFXCertificates/UpdateNotAfter",serial))
            {
                if (response.IsSuccessStatusCode)
                {
                    return serial;
                }
                return null;
            }
        }

        public async Task<string> UploadSignatureImage(string serial, string base64StringImage)
        {
            PostUploadSignatureImage image = new PostUploadSignatureImage();
            image.Serial = serial;
            image.Base64String = base64StringImage;
            using (var response = await _httpClient.PutAsJsonAsync<PostUploadSignatureImage>($"api/PFXCertificates/UploadImage", image))
            {
                if (response.IsSuccessStatusCode)
                {
                    return serial;
                }
                return null;
            }
        }


        public async Task<string> DeleteImage(string serial, string filePath)
        {
            DeleteSignatureImage image = new DeleteSignatureImage();
            image.Serial = serial;
            image.FilePath = filePath;
            using (var response = await _httpClient.PutAsJsonAsync<DeleteSignatureImage>($"api/PFXCertificates/DeleteImage", image))
            {
                if (response.IsSuccessStatusCode)
                {
                    return serial;
                }
                return null;
            }

        }

        public Task<string> UpdateInfoToDatabase(PFXCertificate pfxCertificate)
        {
            throw new NotImplementedException();
        }
    }
}
