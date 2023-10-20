using Microsoft.Extensions.Options;
using Newtonsoft.Json.Linq;
using QuanLyHopDongVaKySo_API.Models;
using RestSharp;
using System.Runtime;
using System.Text;
using System.Text.RegularExpressions;

namespace QuanLyHopDongVaKySo_API.Helpers
{
    public interface ISendMailHelper
    {
        Task<string> BuildMessage(string subject, string senderName, string receiverName, string from, string to, string htmlContent);

        Task<string> GetTokenAsync();

        Task<string> SendMail(SendMail mail);
    }

    public class SendMailHelper : ISendMailHelper
    {
        private readonly IConfiguration _configuration;
        public SendMailHelper(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<string> BuildMessage(string subject, string senderName, string receiverName, string from, string to, string htmlContent)
        {
            // First, base64 encode the HTML content
            var base64HtmlContent = Convert.ToBase64String(Encoding.UTF8.GetBytes(htmlContent));
            var inputBytes = Encoding.UTF8.GetBytes(Encoding.UTF8.GetString(Encoding.UTF8.GetBytes(subject)));
            var base64Subject = Convert.ToBase64String(inputBytes)
              .Replace('+', '-')
              .Replace('/', '_')
              .Replace("=", "");

            // Then, create the MIME message
            var mimeMessage = string.Join("\n",
              new string[]
                      {
                        "MIME-Version: 1.0", "Content-type: text/html; charset=utf-8", "Content-Transfer-Encoding: base64",
                        $"From: {senderName} <{from}>", $"To: {receiverName} <{to}>",
                        $"Subject: =?UTF-8?B?{base64Subject}?=", "", base64HtmlContent
              });

            // Finally, base64url encode the entire MIME message
            var base64MimeMessage = Convert.ToBase64String(Encoding.UTF8.GetBytes(mimeMessage));
            var base64UrlMimeMessage = Regex.Replace(base64MimeMessage, @"\+", "-");
            base64UrlMimeMessage = Regex.Replace(base64UrlMimeMessage, @"\/", "_");
            base64UrlMimeMessage = Regex.Replace(base64UrlMimeMessage, @"=+$", "");

            return base64UrlMimeMessage;
        }

        public async Task<string> GetTokenAsync()
        {
            var client = new RestClient(_configuration["GmailAPI:Oauth2"] + "/token");
            var request = new RestRequest() { Method = Method.Post };

            request.AddParameter("client_id", _configuration["GmailAPI:ClientID"]);
            request.AddParameter("client_secret", _configuration["GmailAPI:ClientSecret"]);
            request.AddParameter("refresh_token", _configuration["GmailAPI:RefreshToken"]);
            request.AddParameter("grant_type", _configuration["GmailAPI:GrandType"]);

            var response = await client.ExecuteAsync(request);

            var jObject = JObject.Parse(response.Content ?? string.Empty);

            return jObject.ContainsKey("access_token") ? (string)jObject["access_token"]! : "";
        }

        public async Task<string> SendMail(SendMail mail)
        {
            try
            {
                var accessToken = await GetTokenAsync();
                var client = new RestClient(_configuration["GmailAPI:GmailAPIUrl"] + "/users/me/messages/send");
                var request = new RestRequest() { Method = Method.Post };
                var messageTask = BuildMessage(mail.Subject, _configuration["GmailAPI:SenderName"], mail.ReceiverName,
                    _configuration["GmailAPI:From"], mail.ToMail, mail.HtmlContent);

                var message = await messageTask;

                request.AddHeader("Authorization", "Bearer " + accessToken);
                request.AddJsonBody(new
                {
                    raw = message
                });

                var response = await client.ExecuteAsync(request);

                var json = JObject.Parse(response.Content);

                return json.ToString();
            }
            catch (Exception ex)
            {

                return null;
            }
            
        }
    }
}