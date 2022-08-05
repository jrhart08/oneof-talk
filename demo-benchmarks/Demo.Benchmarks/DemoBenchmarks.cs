﻿using BenchmarkDotNet.Attributes;
using FluentValidation;
using OneOf;
using System.Threading;
using System.Threading.Tasks;

using OneOfHandler = Demo.OneOf.MediatorHandlers.Features.GetWeatherForecast.GetWeatherForecastHandler;
using OneOfRequest = Demo.OneOf.MediatorHandlers.Features.GetWeatherForecast.GetWeatherForecastRequest;
using OneOfResponse = Demo.OneOf.MediatorHandlers.Features.GetWeatherForecast.GetWeatherForecastResponse;

using ExceptionBasedHandler = Demo.ExceptionBased.MediatorHandlers.Features.GetWeatherForecast.GetWeatherForecastHandler;
using ExceptionBasedRequest = Demo.ExceptionBased.MediatorHandlers.Features.GetWeatherForecast.GetWeatherForecastRequest;
using ExceptionBasedResponse = Demo.ExceptionBased.MediatorHandlers.Features.GetWeatherForecast.GetWeatherForecastResponse;

namespace Demo.Benchmarks;

[MemoryDiagnoser]
[ThreadingDiagnoser]
public class DemoBenchmarks
{
    [Benchmark]
    public async Task<ExceptionBasedResponse> ExceptionBenchmark()
    {
        var request = new ExceptionBasedRequest { Days = 100 };

        try
        {
            return await new ExceptionBasedHandler().Handle(request, CancellationToken.None);
        }
        catch (ValidationException)
        {
            return new ExceptionBasedResponse();
        }
    }

    [Benchmark]
    public async Task<OneOf<OneOfResponse, ValidationException>> OneOfBenchmark()
    {
        var request = new OneOfRequest { Days = 100 };

        var result = await new OneOfHandler().Handle(request, CancellationToken.None);

        return result.Match(
            forecast => forecast,
            err => new OneOfResponse()
        );
    }
}