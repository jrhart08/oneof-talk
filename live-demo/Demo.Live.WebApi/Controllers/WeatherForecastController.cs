using System.Linq;
using System.Threading.Tasks;
using Demo.Live.MediatorHandlers.Features.GetWeatherForecast;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Demo.ExceptionBased.WebApi.Controllers;

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
        try
        {
            return Ok(await _mediator.Send(request));
        }
        catch (ValidationException ex)
        {
            return BadRequest(ex.Errors.Select(err => new
            {
                err.PropertyName,
                err.ErrorMessage,
            }));
        }
    }
}