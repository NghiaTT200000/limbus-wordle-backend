using Limbus_wordle_backend.Services;
using Limbus_wordle_backend.Util.Environment;
using Microsoft.AspNetCore.SignalR;

namespace Limbus_wordle_backend.Hubs.Filter
{
    public class GameAuthHubFilter : IHubFilter
    {
        private readonly AuthService _authService;
        public GameAuthHubFilter(AuthService authService)
        {
            _authService = authService;
        }

        public async ValueTask<object?> InvokeMethodAsync(HubInvocationContext invocationContext, Func<HubInvocationContext, ValueTask<object?>> next)
        {
            var player = _authService.GetTokenFromCookie(EnvironmentVariables.playerDataAuthCookie,
                EnvironmentVariables.playerDataClaimName);
            invocationContext.Context.Items[EnvironmentVariables.playerDataClaimName] = player;
            return await next(invocationContext);
        }
    }
}