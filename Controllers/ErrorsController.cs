using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Diagnostics;

namespace PhotoWebApi.Controllers
{
    [Route("api/[controller]")]
    public class ErrorsController : ControllerBase
    {
        [HttpGet]
        [HttpPost]
        public IActionResult PrepareStatus500ErrorResponse()
        {
            // TODO: Add logging

            var exceptionFeature = HttpContext.Features.Get<IExceptionHandlerPathFeature>();
            if (exceptionFeature != null)
            {
                var errorMessage = String.IsNullOrWhiteSpace(exceptionFeature.Error.Message)
                    ? String.Empty
                    : exceptionFeature.Error.Message;

                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    route = exceptionFeature.Path,
                    errorMessage = errorMessage
                });
            }

            return StatusCode(StatusCodes.Status500InternalServerError, new
            {
                route = String.Empty,
                errorMessage = "Unexpected error"
            });
        }
    }
}
