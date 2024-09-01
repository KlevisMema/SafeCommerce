using Stripe.Checkout;
using SafeCommerce.DataTransormObject.CheckOut;
using SafeCommerce.ProxyApi.Container.Interfaces;
using Stripe;

namespace SafeCommerce.ProxyApi.Container.Services;

public class CheckOutProxyService : ICheckOutProxyService
{
    private readonly ILogger<CheckOutProxyService> logger;

    public CheckOutProxyService(ILogger<CheckOutProxyService> logger)
    {
        this.logger = logger;
        StripeConfiguration.ApiKey = Environment.GetEnvironmentVariable("STRIPE_API_KEY");
    }


    private const string Domain = "https://localhost:7127";
    public Session? CreateCheckOutSesssion
    (
        List<DTO_CartItem> cartItems
    )
    {
        try
        {
            List<SessionLineItemOptions> sessionLineItemOptions = [];

            foreach (DTO_CartItem item in cartItems)
            {
                sessionLineItemOptions.Add(new SessionLineItemOptions
                {
                    PriceData = new SessionLineItemPriceDataOptions
                    {
                        Currency = "EUR",
                        UnitAmount = (long?)(item.Price * 100),
                        ProductData = new SessionLineItemPriceDataProductDataOptions
                        {
                            Name = item.Name,
                        }
                    },
                    Quantity = item.Quantity,
                });
            }

            var options = new SessionCreateOptions
            {
                PaymentMethodTypes =
                [
                    "card"
                ],
                LineItems = sessionLineItemOptions,
                Mode = "payment",
                SuccessUrl = Domain + "/Success-CheckOut",
                CancelUrl = Domain + "/Cancel-CheckOut",
            };

            var service = new SessionService();
            Session session = service.Create(options);

            return session;
        }
        catch (Exception ex)
        {
            logger.LogCritical(ex, "Exception in CreateCheckOutSesssion.");

            return null;
        }
    }
}
