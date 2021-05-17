using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace vsg.ApiAuth.JWT.App
{
    class Program
    {
        static string AuthUri = "https://localhost:5001/api/auth/login";
        static string TargetUri = "https://localhost:5001/weatherforecast";
        async static Task Main(string[] args)
        {
            Console.WriteLine("Authorizing the user with username and passord");
            LoginResult loginResult = await GetToken("test1", "password1");
            Console.WriteLine("Access Token");
            Console.WriteLine(loginResult.AccessToken);
            Console.WriteLine("Refresh Token");
            Console.WriteLine(loginResult.RefreshToken);

            Console.WriteLine("\n\nCalling Weather Api");
            string data = await CallAuthorizedEndPoint(loginResult);
            Console.WriteLine(data);
            Console.ReadLine();
        }
        
        async static Task<LoginResult> GetToken(string username, string password)
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    LoginRequest request = new() { UserName = username, Password = password };
                    var json = JsonSerializer.Serialize(request);
                    var data = new StringContent(json, Encoding.UTF8, "application/json");
                    var response = await client.PostAsync(AuthUri, data);
                    if (response.StatusCode != System.Net.HttpStatusCode.OK) throw new Exception();
                    var result = System.Text.Json.JsonSerializer.Deserialize<LoginResult>(response.Content.ReadAsStringAsync().Result);
                    return result;                    
                }
            }
            catch (Exception)
            {
                return null;
            }           
        }
        async static Task<string> CallAuthorizedEndPoint(LoginResult loginResult)
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Authorization =
                        new AuthenticationHeaderValue("Bearer", loginResult.AccessToken);
                    var response = await client.GetAsync(TargetUri);
                    if (response.StatusCode != System.Net.HttpStatusCode.OK) throw new Exception();
                    var result = response.Content.ReadAsStringAsync().Result;
                    return result;
                }
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }
    }
    public class LoginRequest
    {
        [Required]
        [JsonPropertyName("username")]
        public string UserName { get; set; }

        [Required]
        [JsonPropertyName("password")]
        public string Password { get; set; }
    }
    public class LoginResult
    {
        [JsonPropertyName("username")]
        public string UserName { get; set; }

        [JsonPropertyName("role")]
        public string Role { get; set; }

        [JsonPropertyName("originalUserName")]
        public string OriginalUserName { get; set; }

        [JsonPropertyName("accessToken")]
        public string AccessToken { get; set; }

        [JsonPropertyName("refreshToken")]
        public string RefreshToken { get; set; }
    }
}
