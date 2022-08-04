using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using MediatR;

namespace Demo.ExceptionBased.MediatorHandlers.Features;

public class GetWeatherForecastHandler : IRequestHandler<GetWeatherForecastRequest, GetWeatherForecastResponse>
{
    static readonly string[] Summaries =
    {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

    readonly IValidator<GetWeatherForecastRequest> _validator;

    public GetWeatherForecastHandler(IValidator<GetWeatherForecastRequest> validator)
    {
        _validator = validator;
    }
    
    public async Task<GetWeatherForecastResponse> Handle(GetWeatherForecastRequest request, CancellationToken cancellationToken)
    {
        await _validator.ValidateAndThrowAsync(request, cancellationToken);
        
        var dailyForecasts = Enumerable
            .Range(1, request.Days!.Value)
            .Select(index => new GetWeatherForecastResponse.Forecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToList();

        return new GetWeatherForecastResponse
        {
            DailyForecasts = dailyForecasts,
        };
    }
}