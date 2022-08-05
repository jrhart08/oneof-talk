using System.Threading.Tasks;
using Demo.OneOf.MediatorHandlers.Features.GetWeatherForecast;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Demo.OneOf.WebApi.Controllers;

[ApiController]
[Route("[controller]")]
public class WeatherForecastController : MyBaseController
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

        return ToResult(response);
    }
}