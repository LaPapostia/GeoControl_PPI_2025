using Geocontrol_PPI_NET_9.Models.Auth;
using Newtonsoft.Json;

namespace Geocontrol_PPI_NET_9.Web.Services.Auth;

public class UserApiService
{
    private static HttpClient _httpClient = new();

    public UserApiService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    /// <summary>
    /// M�todo para obtener la lista de usuarios desde la API
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<List<User>> GetUsersAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            var response = await _httpClient.GetAsync("api/User", cancellationToken);

            response.EnsureSuccessStatusCode();
            var json = await response.Content.ReadAsStringAsync(cancellationToken);
            var users = JsonConvert.DeserializeObject<List<User>>(json);

            return users ?? [];
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error fetching users: {ex.Message}");
            throw;
        }
    }

    /// <summary>
    /// Method to obtain a single user in base of the identification id
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<User> GetUserAsync(string Identification, CancellationToken cancellationToken = default)
    {
        try
        {
            var response = await _httpClient.GetAsync($"api/User/{Identification}", cancellationToken);

            response.EnsureSuccessStatusCode();
            var json = await response.Content.ReadAsStringAsync(cancellationToken);
            var users = JsonConvert.DeserializeObject<User>(json);
            return users ?? throw new Exception("Respuesta vac�a");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error fetching users: {ex.Message}");
            throw;
        }
    }
}
