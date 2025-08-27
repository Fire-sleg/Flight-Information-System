using FlightStorageService.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;

namespace FlightStorageService.Controllers
{
    [Route("api/flights")]
    [ApiController]
    public class FlightsController : ControllerBase
    {
        private readonly IFlightService _flightService;
        private readonly ILogger<FlightsController> _logger;

        public FlightsController(IFlightService flightService, ILogger<FlightsController> logger)
        {
            _flightService = flightService;
            _logger = logger;
        }

        [HttpGet("{flightNumber}")]
        public async Task<IActionResult> GetFlightByNumber(string flightNumber)
        {
            _logger.LogInformation("Request to get flight by number: {FlightNumber}", flightNumber);

            try
            {
                var flight = await _flightService.GetFlightByNumberAsync(flightNumber);
                if (flight == null)
                {
                    return NotFound($"Flight with number {flightNumber} not found.");
                }
                return Ok(flight);
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning(ex, "Invalid request for flight by number: {FlightNumber}", flightNumber);
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving flight by number: {FlightNumber}", flightNumber);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetFlightsByDate([FromQuery] string date)
        {
            _logger.LogInformation("Request to get flights by date: {Date}", date);

            if (!DateTime.TryParseExact(date, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out var parsedDate))
            {
                return BadRequest("Invalid date format. Use yyyy-MM-dd.");
            }

            try
            {
                var flights = await _flightService.GetFlightsByDateAsync(parsedDate);
                return Ok(flights);
            }
            catch (ArgumentOutOfRangeException ex)
            {
                _logger.LogWarning(ex, "Invalid date range for flights by date: {Date}", date);
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving flights by date: {Date}", date);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("departure")]
        public async Task<IActionResult> GetFlightsByDepartureCityAndDate([FromQuery] string city, [FromQuery] string date)
        {
            _logger.LogInformation("Request to get flights by departure city and date: {City}, {Date}", city, date);

            if (string.IsNullOrWhiteSpace(city))
            {
                return BadRequest("City cannot be empty.");
            }

            if (!DateTime.TryParseExact(date, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out var parsedDate))
            {
                return BadRequest("Invalid date format. Use yyyy-MM-dd.");
            }

            try
            {
                var flights = await _flightService.GetFlightsByDepartureCityAndDateAsync(city, parsedDate);
                return Ok(flights);
            }
            catch (ArgumentOutOfRangeException ex)
            {
                _logger.LogWarning(ex, "Invalid date range for flights by departure: {City}, {Date}", city, date);
                return BadRequest(ex.Message);
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning(ex, "Invalid request for flights by departure: {City}, {Date}", city, date);
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving flights by departure: {City}, {Date}", city, date);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("arrival")]
        public async Task<IActionResult> GetFlightsByArrivalCityAndDate([FromQuery] string city, [FromQuery] string date)
        {
            _logger.LogInformation("Request to get flights by arrival city and date: {City}, {Date}", city, date);

            if (string.IsNullOrWhiteSpace(city))
            {
                return BadRequest("City cannot be empty.");
            }

            if (!DateTime.TryParseExact(date, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out var parsedDate))
            {
                return BadRequest("Invalid date format. Use yyyy-MM-dd.");
            }

            try
            {
                var flights = await _flightService.GetFlightsByArrivalCityAndDateAsync(city, parsedDate);
                return Ok(flights);
            }
            catch (ArgumentOutOfRangeException ex)
            {
                _logger.LogWarning(ex, "Invalid date range for flights by arrival: {City}, {Date}", city, date);
                return BadRequest(ex.Message);
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning(ex, "Invalid request for flights by arrival: {City}, {Date}", city, date);
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving flights by arrival: {City}, {Date}", city, date);
                return StatusCode(500, "Internal server error");
            }
        }

    }
}
