using System;
using System.Collections.Generic;
using MediatR;

namespace Demo.ExceptionBased.MediatorHandlers.Features.GetWeatherForecast;

public class WeatherProviderDownException : Exception
{

}

public class GetWeatherForecastRequest : IRequest<GetWeatherForecastResponse>
{
    public int? Days { get; set; } = 7;
}

public class GetWeatherForecastResponse
{
    public List<Forecast> DailyForecasts { get; init; }

    public class Forecast
    {
        public DateTime Date { get; init; }

        public int TemperatureC { get; init; }

        public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);

        public string? Summary { get; init; }
    }
}