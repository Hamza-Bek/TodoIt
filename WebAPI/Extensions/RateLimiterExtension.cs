using System.Threading.RateLimiting;
using Microsoft.AspNetCore.RateLimiting;

namespace WebAPI.Extensions;

public static class RateLimiterExtension
{
    public static IServiceCollection AddCustomRateLimiter(this IServiceCollection services)
    {
       
        services.AddRateLimiter(options =>
        {
            options.RejectionStatusCode = 429;

            options.AddFixedWindowLimiter("authenticated", limiterOptions =>
            {
                limiterOptions.PermitLimit = 5; // THIS DEFINES THE NUMBER OF REQUESTS ALLOWED IN THE TIME WINDOW
                limiterOptions.Window = TimeSpan.FromSeconds(10); // THIS DEFINES THE TIME WINDOW : EVERY 10 SECONDS THE LIMIT WILL RESET , ALLOWING FOR A NEW REQUESTS
                limiterOptions.QueueLimit = 0; // THIS DEFINES HOW MANY EXTRA REQUESTS CAN BE QUEUED AFTER THE LIMIT HAS BEEN REACHED, THIS ALSO PREVENT ERROR 429
            });

            options.AddFixedWindowLimiter("anonymous", limiterOptions =>
            {
                limiterOptions.PermitLimit = 1;
                limiterOptions.Window = TimeSpan.FromSeconds(10);
                limiterOptions.QueueLimit = 0;
            });
        });

        return services;
    }
}