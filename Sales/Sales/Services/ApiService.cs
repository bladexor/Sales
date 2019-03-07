namespace Sales.Services
{
    using System;
    using System.Collections.Generic;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Text;
    using System.Threading.Tasks;
    using Common.Models;
    using Helpers;
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

        public async Task<Response> GetList<T>(string urlBase, string prefix, string controller,
            string tokenType, string accessToken)
        {
            try
            {
                var client = new HttpClient();
                client.BaseAddress = new System.Uri(urlBase);
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(tokenType, accessToken);
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
                    IsSuccess = true,
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

        public async Task<Response> Post<T>(string urlBase,string prefix, string controller, T model)
        {
            try
            {
                var request = JsonConvert.SerializeObject(model);
                var content = new StringContent(request, Encoding.UTF8, "application/json");

                var client = new HttpClient();
                client.BaseAddress = new System.Uri(urlBase);
                var url = $"{prefix}{controller}";
                var response = await client.PostAsync(url,content);
                var answer = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    return new Response
                    {
                        IsSuccess = false,
                        Message = answer.ToString(),
                    };
                }

                var obj = JsonConvert.DeserializeObject<T>(answer);
                return new Response
                {
                    IsSuccess = true,
                    Result = obj,
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
        public async Task<Response> Post<T>(string urlBase, string prefix, string controller, T model, 
            string tokenType, string accessToken)
        {
            try
            {
                var request = JsonConvert.SerializeObject(model);
                var content = new StringContent(request, Encoding.UTF8, "application/json");

                var client = new HttpClient();
                client.BaseAddress = new System.Uri(urlBase);
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(tokenType, accessToken);
                var url = $"{prefix}{controller}";
                var response = await client.PostAsync(url, content);
                var answer = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    return new Response
                    {
                        IsSuccess = false,
                        Message = answer.ToString(),
                    };
                }

                var obj = JsonConvert.DeserializeObject<T>(answer);
                return new Response
                {
                    IsSuccess = true,
                    Result = obj,
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

        public async Task<Response> Put<T>(string urlBase, string prefix, string controller, T model,int id)
        {
            try
            {
                var request = JsonConvert.SerializeObject(model);
                var content = new StringContent(request, Encoding.UTF8, "application/json");

                var client = new HttpClient();
                client.BaseAddress = new System.Uri(urlBase);
                var url = $"{prefix}{controller}/{id}";
                var response = await client.PutAsync(url, content);
                var answer = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    return new Response
                    {
                        IsSuccess = false,
                        Message = answer.ToString(),
                    };
                }

                var obj = JsonConvert.DeserializeObject<T>(answer);
                return new Response
                {
                    IsSuccess = true,
                    Result = obj,
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
        public async Task<Response> Put<T>(string urlBase, string prefix, string controller, T model, int id,
            string tokenType, string accessToken)
        {
            try
            {
                var request = JsonConvert.SerializeObject(model);
                var content = new StringContent(request, Encoding.UTF8, "application/json");

                var client = new HttpClient();
                client.BaseAddress = new System.Uri(urlBase);
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(tokenType, accessToken);
                var url = $"{prefix}{controller}/{id}";
                var response = await client.PutAsync(url, content);
                var answer = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    return new Response
                    {
                        IsSuccess = false,
                        Message = answer.ToString(),
                    };
                }

                var obj = JsonConvert.DeserializeObject<T>(answer);
                return new Response
                {
                    IsSuccess = true,
                    Result = obj,
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

        public async Task<Response> Delete(string urlBase, string prefix, string controller, int id)
        {
            try
            {
                var client = new HttpClient();
                client.BaseAddress = new System.Uri(urlBase);
                var url = $"{prefix}{controller}/{id}";
                var response = await client.DeleteAsync(url);
                var answer = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    return new Response
                    {
                        IsSuccess = false,
                        Message = answer.ToString(),
                    };
                }

                return new Response
                {
                    IsSuccess = true,
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
        public async Task<Response> Delete(string urlBase, string prefix, string controller, int id,
            string tokenType, string accessToken)
        {
            try
            {
                var client = new HttpClient();
                client.BaseAddress = new System.Uri(urlBase);
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(tokenType, accessToken);
                var url = $"{prefix}{controller}/{id}";
                var response = await client.DeleteAsync(url);
                var answer = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    return new Response
                    {
                        IsSuccess = false,
                        Message = answer.ToString(),
                    };
                }

                return new Response
                {
                    IsSuccess = true,
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
                    Message = Languages.TurnOnInternet, //Languages.InternetSettings,
                };
            }

            var isReachable = await CrossConnectivity.Current.IsRemoteReachable("192.168.1.201",80,8000);

            if (!isReachable)
            {
                return new Response
                {
                    IsSuccess = false,
                    Message = Languages.NoInternet,//Languages.NoConnection,
                };
            }

            return new Response
            {
                IsSuccess = true,
               
            };
        }

        public async Task<Response> GetUser(string urlBase, string prefix, string controller,string email, string tokenType, string accessToken)
        {
            try
            {
                var getUserRequest = new GetUserRequest
                {
                    Email = email,
                };

                var request = JsonConvert.SerializeObject(getUserRequest);
                var content = new StringContent(request, Encoding.UTF8, "application/json");

                var client = new HttpClient();
                client.BaseAddress = new Uri(urlBase);
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(tokenType, accessToken);
                var url = $"{prefix}{controller}";
                var response = await client.PostAsync(url,content);
                var answer = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    return new Response
                    {
                        IsSuccess = false,
                        Message = answer,
                    };
                }

                var user = JsonConvert.DeserializeObject<MyUserASP>(answer);
                return new Response
                {
                    IsSuccess = true,
                    Result = user,
                };
            }
            catch (Exception ex)
            {
                return new Response
                {
                    IsSuccess = false,
                    Result = ex.Message,
                };
            }
        }

        public async Task<TokenResponse> GetToken(
            string urlBase,
            string username,
            string password)
        {
            try
            {
                var client = new HttpClient();
                client.BaseAddress = new Uri(urlBase);
                var response = await client.PostAsync("Token",
                    new StringContent(string.Format(
                        "grant_type=password&username={0}&password={1}",
                        username, password),
                        Encoding.UTF8, "application/x-www-form-urlencoded"));
                var resultJSON = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<TokenResponse>(resultJSON);
                return result;
            }
            catch 
            {

                return null;
            }
            

        }
    }
}
