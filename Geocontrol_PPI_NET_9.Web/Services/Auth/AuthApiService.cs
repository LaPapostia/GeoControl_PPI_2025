
using Geocontrol_PPI_NET_9.Models.Auth;
using global::Geocontrol_PPI_NET_9.Models.Auth;
using Newtonsoft.Json;
namespace Geocontrol_PPI_NET_9.Web.Services.Auth
{
    public class AuthApiService
    {
        private static HttpClient _httpClient = new();

        public AuthApiService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        /// <summary>
        /// Método para obtener la lista de usuarios desde la API
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<AuthResult> Autentication(Authentication auth, CancellationToken cancellationToken = default)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync("api/Login", auth, cancellationToken);
                response.EnsureSuccessStatusCode();
                var json = await response.Content.ReadAsStringAsync(cancellationToken);
                var result = JsonConvert.DeserializeObject<AuthResult>(json);
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
        public async Task<AuthCode> CreateAuthCode(AuthCode authCode, CancellationToken cancellationToken = default)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync("api/AuthCode", authCode, cancellationToken);
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
        public async Task<AuthCode> ValidateAuthCode(AuthCode authCode, CancellationToken cancellationToken = default)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync("api/AuthCode/Validate", authCode, cancellationToken);
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
    }
}
