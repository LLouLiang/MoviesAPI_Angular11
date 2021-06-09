using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AngularMoviesAPI.Filters
{
    public class CustomActionFilter : IActionFilter
    {
        // trying to inject logging service
        private readonly ILogger<CustomActionFilter> logger;
        public CustomActionFilter(ILogger<CustomActionFilter> logger)
        {
            this.logger = logger;
        }


        public void OnActionExecuted(ActionExecutedContext context)
        {
            logger.LogWarning("******************** On action (Executed) ********************");
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            logger.LogWarning("******************** On action [Excecuting] ********************");
        }
    }
}
