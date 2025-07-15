using OrderApi.Application.DTOs;
using OrderApi.Application.DTOs.Conversions;
using OrderApi.Application.Interfaces;
using Polly;
using Polly.Registry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace OrderApi.Application.Services
{
    public class OrderService(IOrder orderInterface, HttpClient httpClient, 
        ResiliencePipelineProvider<string> resiliencePipeline) : IOrderService
    {
        //get rpoduct
        public async Task<ProductDTO> GetProduct(int productId)
        {
            //call product api using httpclient
            //Redirect this call to API gateway since product API is not response to outsiders.
            var getProduct = await httpClient.GetAsync($"/api/Product/{productId}");
            if (!getProduct.IsSuccessStatusCode)
                return null!;

            var product = await getProduct.Content.ReadFromJsonAsync<ProductDTO>();
            return product!;
        }

        //get user
        public async Task<AppUserDTO> GetUser(int userId)
        {
            //call product api using httpclient
            //Redirect this call to API gateway since product API is not response to outsiders.
            var getUser = await httpClient.GetAsync($"api/Authentication/{userId}");
            //var getUser = await httpClient.GetAsync($"http://localhost:5000/api/Authentication/{userId}");
            if (!getUser.IsSuccessStatusCode)
                return null!;

            var product = await getUser.Content.ReadFromJsonAsync<AppUserDTO>();
            return product!;
        }

        //get order detail by Id
        public async Task<OrderDetailsDTO> GetOrderDetails(int orderId)
        {
            //prepare order
            var order = await orderInterface.FIndByIdAsync(orderId);
            if(order is null || order!.Id <= 0){
                return null!;
            }

            //get retry piprline
            var retryPipeline = resiliencePipeline.GetPipeline("my-retry-pipeline");

            //prepare product
            var productDTO = await retryPipeline.ExecuteAsync(async token => await GetProduct(order.ProductId));

            //prepare client
            var appUserDTO = await retryPipeline.ExecuteAsync(async token => await GetUser(order.ClientId));

            //populate order details
            return new OrderDetailsDTO(
                order.Id,
                productDTO.Id,
                appUserDTO.Id,
                appUserDTO.Name,
                appUserDTO.Email,
                appUserDTO.Address,
                appUserDTO.TelephoneNumber,
                productDTO.Name,
                order.PurchaseQuantity,
                productDTO.Price,
                productDTO.Price * order.PurchaseQuantity,
                order.OrderedDate
            );
        }


        //get orders by client Id
        public async Task<IEnumerable<OrderDTO>> GetOrdersByClientId(int clientId)
        {
            //Get all client orders
            var orders = await orderInterface.GetOrdersAsync(o => o.ClientId == clientId);
            if(!orders.Any()) return null!;

            //convert from entity to DTO
            var (_,_orders) = OrderConversion.FromEntity(null, orders);
            return _orders!;

        }
    }
}
