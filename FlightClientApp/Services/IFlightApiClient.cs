using FlightClientApp.Models;
using System.Net.Http;

namespace FlightClientApp.Services
{
    public interface IFlightApiClient
    {
        Task<Flight?> GetFlightByNumberAsync(string flightNumber);

        Task<IEnumerable<Flight>> GetFlightsByDateAsync(DateTime date);

        Task<IEnumerable<Flight>> GetFlightsByDepartureAsync(string city, DateTime date);

        Task<IEnumerable<Flight>> GetFlightsByArrivalAsync(string city, DateTime date);
    }
}
