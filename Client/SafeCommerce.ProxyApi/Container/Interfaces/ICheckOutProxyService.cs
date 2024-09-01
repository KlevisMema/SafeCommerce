using Stripe.Checkout;
using SafeCommerce.DataTransormObject.CheckOut;

namespace SafeCommerce.ProxyApi.Container.Interfaces;

public interface ICheckOutProxyService
{
    Session? CreateCheckOutSesssion
    (
        List<DTO_CartItem> cartItems
    );
}