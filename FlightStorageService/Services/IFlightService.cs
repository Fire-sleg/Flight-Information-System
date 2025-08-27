using FlightStorageService.Models;

namespace FlightStorageService.Services
{
    public interface IFlightService
    {
        Task<Flight> GetFlightByNumberAsync(string flightNumber);
        Task<IEnumerable<Flight>> GetFlightsByDateAsync(DateTime date);
        Task<IEnumerable<Flight>> GetFlightsByDepartureCityAndDateAsync(string city, DateTime date);
        Task<IEnumerable<Flight>> GetFlightsByArrivalCityAndDateAsync(string city, DateTime date);
    }
}
