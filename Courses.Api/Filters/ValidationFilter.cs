using Microsoft.AspNetCore.Mvc.Filters;
using System.Text.Json;

namespace Courses.Api.Filters
{
    public class ValidationFilter : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (!context.ModelState.IsValid)
            {
                var errors = context.ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage)
                    .ToList();

                var response = ApiResponse<object>.ValidationError(errors);
                
                var jsonResponse = JsonSerializer.Serialize(response, new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                });

                context.Result = new BadRequestObjectResult(response);
                return;
            }

            base.OnActionExecuting(context);
        }
    }
} 