using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Rhyze.API.Extensions;
using Rhyze.API.Models;
using System.Linq;
using System.Threading.Tasks;

namespace Rhyze.API.Filters
{
    public class BindRequestModelsToUserAttribute : TypeFilterAttribute
    {
        public BindRequestModelsToUserAttribute() : base(typeof(BindRequestModelsToUserAttributeImpl)) { }

        internal class BindRequestModelsToUserAttributeImpl : IAsyncActionFilter
        {
            public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
            {
                foreach(var arg in context.ActionArguments.Where(kvp => kvp.Value is RequireAnOwner))
                {
                    (arg.Value as RequireAnOwner).OwnerId = context.HttpContext.User.UserId();
                }

                await next();
            }
        }
    }
}