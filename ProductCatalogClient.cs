using System.Net.Http.Headers;
using System.Text.Json;
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

        private async Task<IEnumerable<ShoppingCartItem>> ConvertToShoppingCartItems(HttpResponseMessage response)
        {
            response.EnsureSuccessStatusCode();
            var products = await
            JsonSerializer.DeserializeAsync<List<ProductCatalogProduct>>(
                await response.Content.ReadAsStreamAsync(),
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true }) ?? new();

            return products
                .Select(p =>
                    new ShoppingCartItem(
                    p.ProductId,
                    p.ProductName,
                    p.ProductDescription,
                    p.Price
                ));
        }
        public async Task<IEnumerable<ShoppingCartItem>> GetShoppingCartItems(int[] productCatalogIds)
        {
            using var response = await RequestProductFromProductCatalog(productCatalogIds);
            return await ConvertToShoppingCartItems(response);
        }
        
        private record ProductCatalogProduct(
            int ProductId,
            string ProductName,
            string ProductDescription,
            Money Price);    
    }
}

