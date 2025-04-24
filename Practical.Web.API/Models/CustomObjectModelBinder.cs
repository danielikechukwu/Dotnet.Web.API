using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Practical.Web.API.Models
{
    // Define a custom model binder by implementing the IModelBinder interface
    public class CustomObjectModelBinder : IModelBinder
    {
        // The BindModelAsync method is invoked by the ASP.NET Core model binding system 
        // to bind the incoming request data to the target model
        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            // Retrieve the value of the model binding field from the request (e.g., from query string)
            var value = bindingContext.ValueProvider.GetValue(bindingContext.FieldName).FirstValue;

            // Check if the value is null or empty, indicating a failed binding attempt
            if(string.IsNullOrEmpty(value))
            {
                // Mark the binding attempt as failed
                bindingContext.Result = ModelBindingResult.Failed();

                // Return a completed task as no further processing is needed
                return Task.CompletedTask;

            }

            // Split the incoming string by colons to extract the individual parts (Name, Age, Location)
            var parts = value.Split(':');

            if(parts.Length == 3)
            {
                var customObject = new CustomObject
                {
                    Name = parts[0],
                    Age = int.Parse(parts[1]),
                    Location = parts[2]
                };

                bindingContext.Result = ModelBindingResult.Success(customObject);
            }
            else
            {
                bindingContext.Result = ModelBindingResult.Failed();
            }

            return Task.CompletedTask;

        }
    }
}
