
using Geocontrol_PPI_NET_9.Models;
using Geocontrol_PPI_NET_9.Models.Auth;
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

    }
}
