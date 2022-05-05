using System.Net.Http.Headers;
using ShoppingCart.ShoppingCart;

namespace ShoppingCart
{
    public interface IProductCatalogClient
    {
        Task<IEnumerable<ShoppingCartItem>> GetShoppingCartItems(int[] productCatalogueIds);
    }

    public class ProductCatalogClient : IProductCatalogClient
    {
        private readonly HttpClient client;
        private static string productCatalogBaseUrl = @"https://git.io/JeHiE";
        private static string getProductPathTemplate = "?productIds=[{0}]";
        public ProductCatalogClient(HttpClient client)
        {
            client.BaseAddress =
            new Uri(productCatalogBaseUrl);
            client
            .DefaultRequestHeaders
            .Accept
            .Add(new MediaTypeWithQualityHeaderValue("application/json"));
            this.client = client;
        }

        private async Task<HttpResponseMessage>
        RequestProductFromProductCatalog(int[] productCatalogIds)
        {
            var productsResource =
            string.Format(getProductPathTemplate,
            string.Join(",", productCatalogIds));
            return await
            this.client.GetAsync(productsResource);
        }

        public Task<IEnumerable<ShoppingCartItem>> GetShoppingCartItems(int[] productCatalogueIds)
        {
            throw new NotImplementedException();
        }
    }
}