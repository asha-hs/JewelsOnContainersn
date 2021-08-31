using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebMvc.Infrastructure;
using WebMvc.Models;
using WebMvc.Models.CartModels;

namespace WebMvc.Services
{
    public class CartService : ICartService
    {
        private readonly IConfiguration _config;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IHttpClient _apiClient;
        private readonly string _remoteServiceBaseUrl;
        private readonly ILogger _logger;
        public CartService(IConfiguration config, IHttpContextAccessor httpContextAccessor,IHttpClient httpClient,ILoggerFactory logger)
        {
            _config = config;
            _remoteServiceBaseUrl = $"{_config["CartUrl"]}/api/cart";
            _httpContextAccessor = httpContextAccessor;
            _apiClient = httpClient;
            _logger = logger.CreateLogger<CartService>();
        }
        public async Task AddItemToCart(ApplicationUser user, CartItem product)
        {
            var cart = await GetCart(user);
            _logger.LogDebug("User Name: " + user.Email);

            var basketItem = cart.Items
                .Where(p => p.ProductId == product.ProductId)
                .FirstOrDefault();

            if(basketItem == null)
            {
                cart.Items.Add(product);
            }
            else
            {
                basketItem.Quantity += 1;
            }

            await UpdateCart(cart);
        }

        public async Task ClearCart(ApplicationUser user)
        {
            var token = await GetUserTokenAsync();
            var cleanBasketUri = ApiPaths.Basket.CleanBasket(_remoteServiceBaseUrl, user.Email);
            _logger.LogDebug("Clean basket Uri: " + cleanBasketUri);
            var response = await _apiClient.DeleteAsync(cleanBasketUri);
            _logger.LogDebug("Basket Cleaned");

        }

        public async Task<Cart> GetCart(ApplicationUser user)
        {
            var token = await GetUserTokenAsync();
            _logger.LogInformation(" We are in get basket and user id " + user.Email);
            _logger.LogInformation(_remoteServiceBaseUrl);

            var getBasketUri = ApiPaths.Basket.GetBasket(_remoteServiceBaseUrl, user.Email);
            _logger.LogInformation(getBasketUri);
            var datastring = await _apiClient.GetStringAsync(getBasketUri, token);
            _logger.LogInformation(datastring);

            var response = JsonConvert.DeserializeObject<Cart>(datastring.ToString()) ??
                new Cart()
                {
                    BuyerId = user.Email
                };

            return response;
        }

        public async Task<Cart> SetQuantities(ApplicationUser user, Dictionary<string, int> quantities)
        {
            var basket = await GetCart(user);
            basket.Items.ForEach(x =>
            {
                if (quantities.TryGetValue(x.Id, out var quantity))
                {
                    x.Quantity = quantity;
                }
            });

            return basket;
        }

        public async Task<Cart> UpdateCart(Cart cart)
        {
            var token = await GetUserTokenAsync();
            _logger.LogDebug(_remoteServiceBaseUrl);
            var updateBasketUri = ApiPaths.Basket.UpdateBasket(_remoteServiceBaseUrl);
            _logger.LogDebug("Update Basket Url: " + updateBasketUri);
            var response = await _apiClient.PostAsync(updateBasketUri, cart, token);
            response.EnsureSuccessStatusCode();
            return cart;
        }

        async Task<string> GetUserTokenAsync()
        {
            var context = _httpContextAccessor.HttpContext;
            return await context.GetTokenAsync("access_token");
        }
    }
}
