using FlightClientApp.Models;

namespace FlightClientApp.Services
{
    public class FlightApiClient : IFlightApiClient
    {
        private readonly HttpClient _httpClient;


        public FlightApiClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }


        public async Task<Flight?> GetFlightByNumberAsync(string flightNumber)
        {
            return await _httpClient.GetFromJsonAsync<Flight>($"api/flights/{flightNumber}");
        }

        public async Task<IEnumerable<Flight>> GetFlightsByDateAsync(DateTime date)
        {
            return await _httpClient.GetFromJsonAsync<IEnumerable<Flight>>(
                $"api/flights?date={date:yyyy-MM-dd}"
            ) ?? new List<Flight>();
        }

        public async Task<IEnumerable<Flight>> GetFlightsByDepartureAsync(string city, DateTime date)
        {
            return await _httpClient.GetFromJsonAsync<IEnumerable<Flight>>(
                $"api/flights/departure?city={Uri.EscapeDataString(city)}&date={date:yyyy-MM-dd}"
            ) ?? new List<Flight>();
        }

        public async Task<IEnumerable<Flight>> GetFlightsByArrivalAsync(string city, DateTime date)
        {
            return await _httpClient.GetFromJsonAsync<IEnumerable<Flight>>(
                $"api/flights/arrival?city={Uri.EscapeDataString(city)}&date={date:yyyy-MM-dd}"
            ) ?? new List<Flight>();
        }
    }
}
