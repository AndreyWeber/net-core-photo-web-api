using System;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;

namespace PhotoWebApi.Helpers
{
    public class ValidateMimeMultipartContentFilter : ActionFilterAttribute
    {
        private readonly ILogger logger;

        public ValidateMimeMultipartContentFilter(ILoggerFactory loggerFactory)
        {
            logger = loggerFactory.CreateLogger("ctor ValidateMimeMultipartContentFilter");
        }

        private static Boolean IsMultipartContenType(String contentType) =>
            !String.IsNullOrWhiteSpace(contentType) &&
            contentType.IndexOf("multipart/", StringComparison.OrdinalIgnoreCase) >= 0;

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (!IsMultipartContenType(context.HttpContext.Request.ContentType))
            {
                context.Result = new StatusCodeResult(415);
                return;
            }

            base.OnActionExecuting(context);
        }
    }
}
