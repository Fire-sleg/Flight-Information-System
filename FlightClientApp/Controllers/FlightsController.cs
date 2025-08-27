using FlightClientApp.Models;
using FlightClientApp.Services;
using Microsoft.AspNetCore.Mvc;

namespace FlightClientApp.Controllers
{
    [Route("flights")]
    [Route("")]
    public class FlightsController : Controller
    {
        private readonly IFlightApiClient _flightApiClient;

        public FlightsController(IFlightApiClient flightApiClient)
        {
            _flightApiClient = flightApiClient;
        }

        [HttpGet("")]
        [HttpGet("index")]
        public IActionResult Index() => View();

        [HttpGet("search-by-number")]
        public async Task<IActionResult> SearchByNumber(string flightNumber)
        {
            if (string.IsNullOrEmpty(flightNumber))
                return View();

            var flight = await _flightApiClient.GetFlightByNumberAsync(flightNumber);

            return View(flight);
        }

        [HttpGet("search-by-date")]
        public async Task<IActionResult> SearchByDate(DateTime? date)
        {
            if (!date.HasValue)
                return View(new List<Flight>());

            var flights = await _flightApiClient.GetFlightsByDateAsync(date.Value);
            return View(flights);
        }

        [HttpGet("search-by-departure")]
        public async Task<IActionResult> SearchByDeparture(string city, DateTime? date)
        {
            if (!date.HasValue || string.IsNullOrEmpty(city))
                return View(new List<Flight>());
            var flights = await _flightApiClient.GetFlightsByDepartureAsync(city, date.Value);

            return View(flights);
        }

        [HttpGet("search-by-arrival")]
        public async Task<IActionResult> SearchByArrival(string city, DateTime? date)
        {
            if (!date.HasValue || string.IsNullOrEmpty(city))
                return View(new List<Flight>());
            var flights = await _flightApiClient.GetFlightsByArrivalAsync(city, date.Value);

            return View(flights);
        }
    }
}
