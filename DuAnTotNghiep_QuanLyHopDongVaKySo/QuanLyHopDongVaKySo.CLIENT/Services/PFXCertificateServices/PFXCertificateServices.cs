using Newtonsoft.Json;
using QuanLyHopDongVaKySo.CLIENT.Constants;
using QuanLyHopDongVaKySo_API.Models;
using QuanLyHopDongVaKySo_API.Models.ViewPost;
using QuanLyHopDongVaKySo_API.Services.PFXCertificateService;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;

namespace QuanLyHopDongVaKySo.CLIENT.Services.PFXCertificateServices
{
    public class PFXCertificateServices : IPFXCertificateServices
    {
        private readonly HttpClient _httpClient;
        private readonly IHttpContextAccessor _httpContextAccessor;


        private string token;

        public PFXCertificateServices(HttpClient httpClient, IHttpContextAccessor httpContextAccessor)
        {
            _httpClient = httpClient;
            _httpContextAccessor = httpContextAccessor;
        }
        public string Token
        {
            get
            {
                if (!String.IsNullOrEmpty(_httpContextAccessor.HttpContext.Session.GetString("token")))
                {
                    token = _httpContextAccessor.HttpContext.Session.GetString("token");

                }
                else
                {
                    token = _httpContextAccessor.HttpContext.Session.GetString(SessionKey.Customer.CustomerToken);
                }
                return token;
            }
            set { this.token = value; }
        }


        public async Task<List<PFXCertificate>> GetAll()
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token);
            var response = await _httpClient.GetFromJsonAsync<List<PFXCertificate>>("api/PFXCertificates");
            return response;
        }

        public async Task<List<PFXCertificate>> GetAllAboutToExpire()
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token);
            var response = await _httpClient.GetFromJsonAsync<List<PFXCertificate>>("api/PFXCertificates/AboutToExpire");
            return response;
        }

        public async Task<List<PFXCertificate>> GetAllExpire()
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token);
            var response = await _httpClient.GetFromJsonAsync<List<PFXCertificate>>("api/PFXCertificates/Expire");
            return response;
        }

        public async Task<PFXCertificate> GetById(string serial)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token);
            var response = await _httpClient.GetFromJsonAsync<PFXCertificate>($"api/PFXCertificates/{serial}");
            return response;
        }

        public async Task<string> Update(PFXCertificate certificate)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token);
            using (var response = await _httpClient.PutAsJsonAsync<PFXCertificate>($"api/PFXCertificates/Update", certificate))
            {
                if (response.IsSuccessStatusCode)
                {
                    return certificate.Serial;
                }
                return null;
            }
               
        }

        public async Task<string> UpdateNotAfter(string serial)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token);
            using (var response = await _httpClient.PostAsJsonAsync<string>($"api/PFXCertificates/UpdateNotAfter",serial))
            {
                if (response.IsSuccessStatusCode)
                {
                    return serial;
                }
                return null;
            }
        }

        /*public async Task<string> UploadSignatureImage(string serial, string base64StringImage)
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

        }*/
    }
}
