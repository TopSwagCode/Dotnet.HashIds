using HashidsNet;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Dotnet.HashIds.API;

public class ProductIdBinder : IModelBinder
{
    private readonly IHashids _hashids;

    public ProductIdBinder(IHashids hashids)
    {
        _hashids = hashids;
    }


    public Task BindModelAsync(ModelBindingContext bindingContext)
    {
        if (bindingContext == null)
        {
            throw new ArgumentNullException(nameof(bindingContext));
        }

        var modelName = bindingContext.ModelName;

        // Try to fetch the value of the argument by name
        var valueProviderResult = bindingContext.ValueProvider.GetValue(modelName);

        if (valueProviderResult == ValueProviderResult.None)
        {
            return Task.CompletedTask;
        }

        bindingContext.ModelState.SetModelValue(modelName, valueProviderResult);

        var value = valueProviderResult.FirstValue;

        // Check if the argument value is null or empty
        if (string.IsNullOrEmpty(value))
        {
            return Task.CompletedTask;
        }

        /*
        if (!int.TryParse(value, out var id))
        {
            // Non-integer arguments result in model state errors
            bindingContext.ModelState.TryAddModelError(
                modelName, "Author Id must be an integer.");

            return Task.CompletedTask;
        }
        */
        
        // Model will be null if not found, including for
        // out of range id values (0, -3, etc.)

        bindingContext.Result = ModelBindingResult.Success(new ProductId(1));
        return Task.CompletedTask;
    }
}