using FlightStorageService.Models;

namespace FlightStorageService.Repositories
{
    public interface IFlightRepository
    {
        Task<Flight> GetFlightByNumberAsync(string flightNumber);
        Task<IEnumerable<Flight>> GetFlightsByDateAsync(DateTime date);
        Task<IEnumerable<Flight>> GetFlightsByDepartureCityAndDateAsync(string city, DateTime date);
        Task<IEnumerable<Flight>> GetFlightsByArrivalCityAndDateAsync(string city, DateTime date);
        Task AddFlightAsync(Flight flight); 
        Task CleanupOldFlightsAsync(); 
    }
}
