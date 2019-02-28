﻿namespace Sales.Services
{
    using System.Collections.Generic;
    using System.Net.Http;
    using System.Threading.Tasks;
    using Common.Models;
    using Newtonsoft.Json;
    using Plugin.Connectivity;

    public class ApiService
    {
        public async Task<Response> GetList<T>(string urlBase,string prefix,string controller)
        {           
            try
            {
                var client = new HttpClient();
                client.BaseAddress = new System.Uri(urlBase);
                var url = $"{prefix}{controller}";
                var response = await client.GetAsync(url);
                var answer = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    return new Response
                    {
                        IsSuccess = false,
                        Message = answer.ToString(),
                    };
                }

                var list = JsonConvert.DeserializeObject<List<T>>(answer);
                return new Response
                {
                    IsSuccess =true,
                    Result = list,
                };
            }
            catch (System.Exception ex)
            {

                return new Response
                {
                    IsSuccess = false,
                    Message = ex.Message, //+ client.BaseAddress+url,
                };
            }

        }

        public async Task<Response> CheckConnection()
        {
            if (!CrossConnectivity.Current.IsConnected)
            {
                return new Response
                {
                    IsSuccess = false,
                    Message = "Please turn on your Internet Settings!",//Languages.InternetSettings,
                };
            }

            var isReachable = await CrossConnectivity.Current.IsRemoteReachable("google.com");
            if (!isReachable)
            {
                return new Response
                {
                    IsSuccess = false,
                    Message = "No Internet Connection!",//Languages.NoConnection,
                };
            }

            return new Response
            {
                IsSuccess = true,
               
            };
        }
    }
}
