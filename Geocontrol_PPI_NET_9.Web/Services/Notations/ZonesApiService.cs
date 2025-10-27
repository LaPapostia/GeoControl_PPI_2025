using Geocontrol_PPI_NET_9.Models.Auth;
using Geocontrol_PPI_NET_9.Models.Notations;
using Newtonsoft.Json;

namespace Geocontrol_PPI_NET_9.Web.Services.Notations
{
    public class ZonesApiService
    {
        private static HttpClient _httpClient = new HttpClient();

        public ZonesApiService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<Zone>> ObtainZones(CancellationToken cancellationToken = default)
        {
            var response = await _httpClient.GetAsync("api/Zones", cancellationToken);
            var json = await response.Content.ReadAsStringAsync(cancellationToken);
            var zones = JsonConvert.DeserializeObject<List<Zone>>(json);
            return zones ?? [];
        }

        public async Task CreateZone(Zone zone, CancellationToken cancellationToken = default)
        {
            var response = await _httpClient.PostAsJsonAsync("api/Zones", zone, cancellationToken);
            response.EnsureSuccessStatusCode();
        }


    }
}
