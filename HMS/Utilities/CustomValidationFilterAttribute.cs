
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace HMS.Utilities
{
    
    public class CustomValidationFilterAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (!context.ModelState.IsValid)
            {
                // Retrieve model validation errors
                var errors = context.ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage)
                    .ToList();

                // Customize error message
                var errorMessage = "Validation failed**: " + string.Join(", ", errors);

                // Return bad request with custom error message
                context.Result = new BadRequestObjectResult(errorMessage);
            }
        }
    }

}
