using FlightStorageService.Models;
using Microsoft.Data.SqlClient;
using System.Data;


namespace FlightStorageService.Repositories
{
    public class FlightRepository : IFlightRepository
    {
        private readonly string _connectionString;
        private readonly ILogger<FlightRepository> _logger;

        public FlightRepository(IConfiguration configuration, ILogger<FlightRepository> logger)
        {
            _connectionString = configuration.GetConnectionString("FlightsDb") ?? throw new ArgumentNullException("Connection string 'FlightsDb' not found.");
            _logger = logger;
        }

        private async Task<Flight> MapReaderToFlightAsync(SqlDataReader reader)
        {
            if (!await reader.ReadAsync())
            {
                return null;
            }

            return new Flight
            {
                FlightNumber = reader.GetString(0),
                DepartureDateTime = reader.GetDateTime(1),
                DepartureAirportCity = reader.GetString(2),
                ArrivalAirportCity = reader.GetString(3),
                DurationMinutes = reader.GetInt32(4)
            };
        }

        private async Task<IEnumerable<Flight>> MapReaderToFlightsAsync(SqlDataReader reader)
        {
            var flights = new List<Flight>();
            while (await reader.ReadAsync())
            {
                flights.Add(new Flight
                {
                    FlightNumber = reader.GetString(0),
                    DepartureDateTime = reader.GetDateTime(1),
                    DepartureAirportCity = reader.GetString(2),
                    ArrivalAirportCity = reader.GetString(3),
                    DurationMinutes = reader.GetInt32(4)
                });
            }
            return flights;
        }

        public async Task<Flight> GetFlightByNumberAsync(string flightNumber)
        {
            try
            {
                using var connection = new SqlConnection(_connectionString);
                await connection.OpenAsync();

                using var command = new SqlCommand("[FlightsDb].[dbo].[GetFlightByNumber]", connection)
                {
                    CommandType = CommandType.StoredProcedure
                };

                command.Parameters.AddWithValue("@FlightNumber", flightNumber);

                using var reader = await command.ExecuteReaderAsync();
                return await MapReaderToFlightAsync(reader);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting flight by number: {FlightNumber}", flightNumber);
                throw;
            }
        }

        public async Task<IEnumerable<Flight>> GetFlightsByDateAsync(DateTime date)
        {
            try
            {
                using var connection = new SqlConnection(_connectionString);
                await connection.OpenAsync();

                using var command = new SqlCommand("[FlightsDb].[dbo].[GetFlightsByDate]", connection)
                {
                    CommandType = CommandType.StoredProcedure
                };
                command.Parameters.AddWithValue("@Date", date.Date);

                using var reader = await command.ExecuteReaderAsync();
                return await MapReaderToFlightsAsync(reader);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting flights by date: {Date}", date);
                throw;
            }
        }

        public async Task<IEnumerable<Flight>> GetFlightsByDepartureCityAndDateAsync(string city, DateTime date)
        {
            try
            {
                using var connection = new SqlConnection(_connectionString);
                await connection.OpenAsync();

                using var command = new SqlCommand("[FlightsDb].[dbo].[GetFlightsByDepartureCityAndDate]", connection)
                {
                    CommandType = CommandType.StoredProcedure
                };
                command.Parameters.AddWithValue("@City", city);
                command.Parameters.AddWithValue("@Date", date.Date);

                using var reader = await command.ExecuteReaderAsync();
                return await MapReaderToFlightsAsync(reader);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting flights by departure city and date: {City}, {Date}", city, date);
                throw;
            }
        }

        public async Task<IEnumerable<Flight>> GetFlightsByArrivalCityAndDateAsync(string city, DateTime date)
        {
            try
            {
                using var connection = new SqlConnection(_connectionString);
                await connection.OpenAsync();

                using var command = new SqlCommand("[FlightsDb].[dbo].[GetFlightsByArrivalCityAndDate]", connection)
                {
                    CommandType = CommandType.StoredProcedure
                };
                command.Parameters.AddWithValue("@City", city);
                command.Parameters.AddWithValue("@Date", date.Date);

                using var reader = await command.ExecuteReaderAsync();
                return await MapReaderToFlightsAsync(reader);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting flights by arrival city and date: {City}, {Date}", city, date);
                throw;
            }
        }

        public async Task AddFlightAsync(Flight flight)
        {
            try
            {
                using var connection = new SqlConnection(_connectionString);
                await connection.OpenAsync();

                using var command = new SqlCommand("[FlightsDb].[dbo].[AddFlight]", connection)
                {
                    CommandType = CommandType.StoredProcedure
                };
                command.Parameters.AddWithValue("@FlightNumber", flight.FlightNumber);
                command.Parameters.AddWithValue("@DepartureDateTime", flight.DepartureDateTime);
                command.Parameters.AddWithValue("@DepartureAirportCity", flight.DepartureAirportCity);
                command.Parameters.AddWithValue("@ArrivalAirportCity", flight.ArrivalAirportCity);
                command.Parameters.AddWithValue("@DurationMinutes", flight.DurationMinutes);

                await command.ExecuteNonQueryAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding flight: {FlightNumber}", flight.FlightNumber);
                throw;
            }
        }

        public async Task CleanupOldFlightsAsync()
        {
            try
            {
                using var connection = new SqlConnection(_connectionString);
                await connection.OpenAsync();

                using var command = new SqlCommand("[FlightsDb].[dbo].[CleanupOldFlights]", connection)
                {
                    CommandType = CommandType.StoredProcedure
                };

                await command.ExecuteNonQueryAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error cleaning up old flights");
                throw;
            }
        }
    }
}

