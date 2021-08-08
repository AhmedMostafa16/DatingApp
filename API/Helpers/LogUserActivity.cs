using System;
using System.Threading.Tasks;
using API.Extensions;
using API.Interfaces;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;

namespace API.Helpers
{
    public class LogUserActivity : IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var resultContext = await next().ConfigureAwait(false);

            if (!resultContext.HttpContext.User.Identity.IsAuthenticated) return;

            int userId = resultContext.HttpContext.User.GetUserId();
            var repo = resultContext.HttpContext.RequestServices.GetService<IUserRepository>();
            var user = await repo.GetUserByIdAsync(userId).ConfigureAwait(false);
            user.LastActive = DateTime.Now;
            await repo.SaveAllAsync().ConfigureAwait(false);
        }
    }
}