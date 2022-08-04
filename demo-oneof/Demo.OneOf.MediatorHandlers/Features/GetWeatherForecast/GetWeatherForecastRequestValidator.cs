using FluentValidation;

namespace Demo.OneOf.MediatorHandlers.Features.GetWeatherForecast;
public class GetWeatherForecastRequestValidator : AbstractValidator<GetWeatherForecastRequest>
{
    public GetWeatherForecastRequestValidator()
    {
        RuleFor(req => req.Days)
            .GreaterThan(0)
            .WithMessage("Please enter a number of days to forecast greater than 0");

        RuleFor(req => req.Days)
            .LessThan(30)
            .WithMessage("Can't forecast weather beyond 30 days");
    }
}