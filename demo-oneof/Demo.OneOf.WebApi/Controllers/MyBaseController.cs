using System.Linq;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using OneOf;

namespace Demo.OneOf.WebApi.Controllers;

public abstract class MyBaseController : ControllerBase
{
    protected IActionResult ToResult<T>(OneOf<T, ValidationException> union)
    {
        return union.Match<IActionResult>(
            success => Ok(success),
            exception => BadRequest(exception.Errors.Select(err => new
            {
                err.PropertyName,
                err.ErrorMessage,
            })));
    }
}