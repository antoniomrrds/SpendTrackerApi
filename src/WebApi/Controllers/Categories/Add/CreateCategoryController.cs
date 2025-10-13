using Application.Categories.Add;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using SharedKernel;
using System.Globalization;
using WebApi.Extensions;

namespace WebApi.Controllers.Categories.Add;

[ApiController]
[Route("api/categories")]
public class CreateCategoryController : ControllerBase
{
    private readonly ICreateCategoryUseCase _useCase;
    private readonly IValidator<CreateCategoryCommand> _validator;

    public CreateCategoryController(ICreateCategoryUseCase useCase, IValidator<CreateCategoryCommand> validator)
    {
        _useCase = useCase;
        _validator = validator;
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateCategoryRequest request)
    {
        CreateCategoryCommand command = new(request.Name, request.Description);
        ValidationResult? validation = await _validator.ValidateAsync(command);
        if (!validation.IsValid)
        {
            validation.AddToModelState(ModelState);
            return ValidationProblem(ModelState);
        }
        
        Result<CreateCategoryResult> result = await _useCase.Perform(command);
        return result.IsFailure 
            ? Conflict(result.Error.Description) 
            : Ok(result.Value);
        // return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
    }
}