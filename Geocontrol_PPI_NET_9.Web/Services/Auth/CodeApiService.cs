
using Geocontrol_PPI_NET_9.Models;
using Geocontrol_PPI_NET_9.Models.Auth;
using Newtonsoft.Json;
namespace Geocontrol_PPI_NET_9.Web.Services.Auth
{
    public class CodeApiService
    {
        private static HttpClient _httpClient = new();

        public CodeApiService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        /// <summary>
        /// Método para obtener la lista de usuarios desde la API
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<AuthCode> CreateAuthCode(AuthCode authCode, CancellationToken cancellationToken = default)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync("api/Code", authCode, cancellationToken);
                response.EnsureSuccessStatusCode();
                var json = await response.Content.ReadAsStringAsync(cancellationToken);
                var result = JsonConvert.DeserializeObject<AuthCode>(json);
                return result ?? throw new Exception("Respuesta vacía");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching users: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// Método para obtener la lista de usuarios desde la API
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<bool> ValidateAuthCode(AuthCode authCode, CancellationToken cancellationToken = default)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync("api/Code/Validate", authCode, cancellationToken);
                response.EnsureSuccessStatusCode();
                var json = await response.Content.ReadAsStringAsync(cancellationToken);
                var result = JsonConvert.DeserializeObject<StandardResponse>(json);
                return result?.result ?? throw new Exception("Respuesta vacía");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching users: {ex.Message}");
                throw;
            }
        }


    }
}
