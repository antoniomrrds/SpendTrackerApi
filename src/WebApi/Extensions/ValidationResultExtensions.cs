using FluentValidation.Results;

using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace WebApi.Extensions;

internal static class ValidationResultExtensions
{
    public static void AddToModelState(this ValidationResult validationResult, ModelStateDictionary modelState)
    {
        foreach (ValidationFailure? error in validationResult.Errors)
        {
            modelState.AddModelError(error.PropertyName, error.ErrorMessage);
        }
    }
}