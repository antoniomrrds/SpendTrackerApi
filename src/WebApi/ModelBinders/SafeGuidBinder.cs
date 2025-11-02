using Microsoft.AspNetCore.Mvc.ModelBinding;
using WebApi.Controllers.Common;

namespace WebApi.ModelBinders;

public class SafeGuidBinder : IModelBinder
{
    public Task BindModelAsync(ModelBindingContext bindingContext)
    {
        string? value =
            bindingContext.ValueProvider.GetValue(bindingContext.FieldName).FirstValue
            ?? bindingContext.ValueProvider.GetValue(bindingContext.OriginalModelName).FirstValue;

        if (!Guid.TryParse(value, out Guid guid) || guid == Guid.Empty)
        {
            bindingContext.ModelState.AddModelError(
                bindingContext.ModelName,
                $"O valor '{value}' não é um GUID válido."
            );

            return Task.CompletedTask;
        }

        bindingContext.Result = ModelBindingResult.Success(new SafeGuid { Value = guid });
        return Task.CompletedTask;
    }
}
