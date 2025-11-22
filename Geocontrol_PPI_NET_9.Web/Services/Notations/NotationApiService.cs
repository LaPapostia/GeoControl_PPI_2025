using Geocontrol_PPI_NET_9.Models.Auth;
using Geocontrol_PPI_NET_9.Models.Notations;
using Newtonsoft.Json;

namespace Geocontrol_PPI_NET_9.Web.Services.Notations
{
    public class NotationApiService
    {
        private static HttpClient _httpClient = new();
        private static string _url = "api/Notation";

        public NotationApiService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<Notation>> ObtainNotations(
            string? identification,
            int? zone_id,
            string? start_date,
            string? finish_date,
            CancellationToken cancellationToken = default)
        {
            var query = $"?identification={identification}&zone_id={zone_id}&start_date={start_date}&finish_date={finish_date}";
            var response = await _httpClient.GetAsync($"{_url}{query}", cancellationToken);

            response.EnsureSuccessStatusCode(); // Lanza excepción si no es 2xx

            var json = await response.Content.ReadAsStringAsync(cancellationToken);
            var notations = JsonConvert.DeserializeObject<List<Notation>>(json);
            return notations ?? [];
        }


        public async Task CreateNotation(Notation notation, CancellationToken cancellationToken = default)
        {
            var response = await _httpClient.PostAsJsonAsync(_url, notation, cancellationToken);
            response.EnsureSuccessStatusCode();
        }


    }
}
