using Stripe;

namespace DrivePal.Models.ServiceClasses
{
    /// <summary>
    /// Extension method for configuring and adding Stripe to the service collection.
    /// </summary>
    public static class StripeServiceExtension
    {
        /// <summary>
        /// Adds the Stripe service to the service collection with the specified API key.
        /// </summary>
        /// <param name="services">The service collection to add the Stripe service to.</param>
        /// <param name="apiKey">The Stripe API key.</param>
        public static void AddStripe(this IServiceCollection services, string apiKey)
        {
            services.AddSingleton(new StripeClient(apiKey));
        }
    }
}
