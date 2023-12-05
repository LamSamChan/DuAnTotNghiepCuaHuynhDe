using RestSharp;

namespace QuanLyHopDongVaKySo_API.Helpers
{
    public class ApiResponse
    {
        public string url { get; set; }
        public string key { get; set; }
        public string shrtlnk { get; set; }
    }
    public interface IShortLinkHelper
    {
        public Task<string> GenerateShortUrl(string url);
    }
    public class ShortLinkHelper : IShortLinkHelper
    {
        private readonly IConfiguration _configuration;
        public ShortLinkHelper(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public async Task<string> GenerateShortUrl(string url)
        {
            // Khởi tạo RestClient với URL của API
            var client = new RestClient(_configuration["ShorLinkAPI:APIUrl"]);

            // Khởi tạo yêu cầu POST
            var request = new RestRequest() { Method = Method.Post };

            // Thêm header
            request.AddHeader("api-key", _configuration["ShorLinkAPI:APIKey"]);
            request.AddHeader("Accept", "application/json");
            request.AddHeader("Content-Type", "application/json");

            // Thêm đối tượng truyền vào yêu cầu
            request.AddJsonBody(new { url = $"{url}" });

            // Thực hiện yêu cầu và nhận phản hồi
            var response = await client.ExecuteAsync(request);

            // Kiểm tra xem yêu cầu có thành công không (mã trạng thái 200 OK)
            if (response.IsSuccessStatusCode)
            {
                // Chuyển đổi phản hồi từ JSON sang đối tượng
                var responseObject = Newtonsoft.Json.JsonConvert.DeserializeObject<ApiResponse>(response.Content ?? string.Empty);

                // Lấy giá trị "shrtlnk" từ đối tượng trả về
                string shrtlnk = responseObject.shrtlnk;

                return shrtlnk;
            }
            else
            {
                return null;
            }
        }
    }
}
