using System.Runtime.CompilerServices;
using Domain;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Persistence;

namespace API.Controllers;

[ApiController]
[Route("[controller]")]

public class WeatherForecastController : ControllerBase
{

    private static readonly string[] Summaries = new[]
    {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

    private readonly ILogger<WeatherForecastController> _logger;

    private readonly DataContext _context;
    public WeatherForecastController (ILogger<WeatherForecastController> logger, DataContext context)
    {
        _logger = logger;
        _context = context;
    }

    [HttpGet (Name = "GetWeatherForecast")]

    public IEnumerable<WeatherForecast> Get()
    {
        return Enumerable.Range(1, 5).Select(Index => new WeatherForecast
        {
            Date = DateOnly.FromDateTime(DateTime.Now.AddDays(Index)),
            TemperatureC = Random.Shared.Next(-20, 55),
            Summary = Summaries[Random.Shared.Next(Summaries.Length)]
        })
        .ToArray();
    }

        [HttpPost]

    public ActionResult<WeatherForecast> Create()
    {
        Console.WriteLine($"Database path: {_context.DbPath}");
        Console.WriteLine("Insert a new WeatherForecast");

        var forecast = new WeatherForecast()
        {
            Date = new DateOnly(),
            TemperatureC = 75,
            Summary = "Warm"
        };

        _context.WeatherForecasts.Add(forecast);
        var success = _context.SaveChanges() > 0;

        if (success)
        {
            return forecast;
        }

        throw new Exception("Error creating WeatherForecast");
    }
}