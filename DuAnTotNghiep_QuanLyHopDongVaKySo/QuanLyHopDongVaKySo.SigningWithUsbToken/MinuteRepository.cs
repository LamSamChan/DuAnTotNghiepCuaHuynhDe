﻿using Newtonsoft.Json;
using QuanLyHopDongVaKySo.SigningWithUsbToken.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyHopDongVaKySo.SigningWithUsbToken
{
    public class MinuteRepository
    {
        public HttpClient _httpClient;
        public HttpResponseMessage _httpResponseMessage;
        public MinuteRepository()
        {
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri("https://localhost:7286/");

            //_httpClient.BaseAddress = new Uri("https://techsealapi.azurewebsites.net/");

            _httpClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
        }

        public async Task<PendingMinute> GetPMinuteById(int id)
        {
            _httpResponseMessage = await _httpClient.GetAsync($"api/PMinute/{id}");
            var json = await _httpResponseMessage.Content.ReadAsStringAsync();
            var pMinute = JsonConvert.DeserializeObject<PendingMinute>(json);
            return pMinute;
        }

    }
}