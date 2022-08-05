using System.Linq;
using System.Threading.Tasks;
using Demo.OneOf.MediatorHandlers.Features.GetWeatherForecast;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Demo.OneOf.WebApi.Controllers;

[ApiController]
[Route("[controller]")]
public class WeatherForecastController : ControllerBase
{
    readonly IMediator _mediator;

    public WeatherForecastController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("GetWeatherForecast")]
    [ProducesResponseType(200, Type = typeof(GetWeatherForecastResponse))]
    [ProducesResponseType(400)]
    public async Task<IActionResult> Get([FromQuery] GetWeatherForecastRequest request)
    {
        var response = await _mediator.Send(request);

        return response.Match<IActionResult>(
            forecast => Ok(forecast),
            exception => BadRequest(exception.Errors.Select(err => new
            {
                err.PropertyName,
                err.ErrorMessage,
            })),
            weatherException => StatusCode(503, new { message = "Weather provider is currently down. Please try again" })
        );
    }
}