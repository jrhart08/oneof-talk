using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using MediatR;
using OneOf;

namespace Demo.OneOf.MediatorHandlers.Features.GetWeatherForecast;

public class GetWeatherForecastHandler : IRequestHandler<GetWeatherForecastRequest, OneOf<GetWeatherForecastResponse, ValidationException, WeatherProviderDownException>>
{
    static readonly string[] Summaries =
    {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

    readonly GetWeatherForecastRequestValidator _validator = new();

    public async Task<OneOf<GetWeatherForecastResponse, ValidationException, WeatherProviderDownException>> Handle(GetWeatherForecastRequest request, CancellationToken cancellationToken)
    {
        var validationResult = await _validator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid)
        {
            return new ValidationException(validationResult.Errors);
        }

        if (IsWeatherProviderDown())
        {
            return new WeatherProviderDownException();
        }
        
        var dailyForecasts = Enumerable
            .Range(0, request.Days!.Value)
            .Select(index => new GetWeatherForecastResponse.Forecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = GetRandomTemp(),
                Summary = GetRandomSummary()
            })
            .ToList();

        return new GetWeatherForecastResponse
        {
            DailyForecasts = dailyForecasts,
        };
    }

    static bool IsWeatherProviderDown() => Random.Shared.Next(1, 10) == 1;

    static int GetRandomTemp() => Random.Shared.Next(-20, 55);

    static string GetRandomSummary() => Summaries[Random.Shared.Next(Summaries.Length)];
}