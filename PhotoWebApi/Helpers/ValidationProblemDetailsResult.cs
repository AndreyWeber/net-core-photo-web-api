using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc;

namespace PhotoWebApi.Helpers
{
    public class ValidationProblemDetailsResult : IActionResult
    {
        // TODO: Get errors in case of wrong inferred type of action argument: public async Task<IActionResult> Post(IList<IFormFile> bananas)
        public Task ExecuteResultAsync(ActionContext context)
        {
            var modelStateEntries = context.ModelState.Where(e => e.Value.Errors.Count > 0).ToArray();
            var errors = new List<Object>();

            var details = "See ValidationErrors for details";

            if (modelStateEntries.Any())
            {
                if (modelStateEntries.Length == 1 &&
                    modelStateEntries[0].Value.Errors.Count == 1 &&
                    modelStateEntries[0].Key == string.Empty &&
                    modelStateEntries[0].Value.Errors[0].ErrorMessage != String.Empty)
                {
                    details = modelStateEntries[0].Value.Errors[0].ErrorMessage;
                }
                else
                {
                    foreach (var modelStateEntry in modelStateEntries)
                    {
                        foreach (var modelStateError in modelStateEntry.Value.Errors)
                        {
                            var error = new
                            {
                                name = modelStateEntry.Key,
                                description = modelStateError.ErrorMessage
                            };

                            errors.Add(error);
                        }
                    }
                }
            }

            // var problemDetails = new ValidationProblemDetails
            // {
            //     Status = 400,
            //     Title = "Request Validation Error",
            //     Instance = $"urn:myorganization:badrequest:{Guid.NewGuid()}",
            //     Detail = details,
            //     ValidationErrors = errors
            // };

            //context.HttpContext.Response.WriteJson(problemDetails);

            return Task.CompletedTask;
        }
    }
}
