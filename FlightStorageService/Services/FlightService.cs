using FlightStorageService.Models;
using FlightStorageService.Repositories;

namespace FlightStorageService.Services
{
    public class FlightService : IFlightService
    {
        private readonly IFlightRepository _repository;
        private readonly ILogger<FlightService> _logger;

        public FlightService(IFlightRepository repository, ILogger<FlightService> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task<Flight> GetFlightByNumberAsync(string flightNumber)
        {
            if (string.IsNullOrWhiteSpace(flightNumber))
            {
                throw new ArgumentException("Flight number cannot be empty.", nameof(flightNumber));
            }

            var flight = await _repository.GetFlightByNumberAsync(flightNumber);
            if (flight == null)
            {
                _logger.LogInformation("Flight not found: {FlightNumber}", flightNumber);
            }
            return flight;
        }

        public async Task<IEnumerable<Flight>> GetFlightsByDateAsync(DateTime date)
        {
            // Ensure date is within 7 days, as per data restriction
            if (date < DateTime.UtcNow.Date || date > DateTime.UtcNow.Date.AddDays(7))
            {
                throw new ArgumentOutOfRangeException(nameof(date), "Date must be within the next 7 days.");
            }

            return await _repository.GetFlightsByDateAsync(date);
        }

        public async Task<IEnumerable<Flight>> GetFlightsByDepartureCityAndDateAsync(string city, DateTime date)
        {
            if (string.IsNullOrWhiteSpace(city))
            {
                throw new ArgumentException("City cannot be empty.", nameof(city));
            }

            if (date < DateTime.UtcNow.Date || date > DateTime.UtcNow.Date.AddDays(7))
            {
                throw new ArgumentOutOfRangeException(nameof(date), "Date must be within the next 7 days.");
            }

            return await _repository.GetFlightsByDepartureCityAndDateAsync(city, date);
        }

        public async Task<IEnumerable<Flight>> GetFlightsByArrivalCityAndDateAsync(string city, DateTime date)
        {
            if (string.IsNullOrWhiteSpace(city))
            {
                throw new ArgumentException("City cannot be empty.", nameof(city));
            }

            if (date < DateTime.UtcNow.Date || date > DateTime.UtcNow.Date.AddDays(7))
            {
                throw new ArgumentOutOfRangeException(nameof(date), "Date must be within the next 7 days.");
            }

            return await _repository.GetFlightsByArrivalCityAndDateAsync(city, date);
        }
    }
}
