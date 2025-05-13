using System;
using System.Collections.Generic;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using api.Model;
using api.Services;
using System.Web;  // For HttpUtility

namespace api
{
    public class OrderFunctions
    {
        private readonly ILogger _logger;
        private readonly IOrdersService _ordersService;

        public OrderFunctions(ILoggerFactory loggerFactory, IOrdersService ordersService)
        {
            // Corrected logger type to OrderFunctions.
            _logger = loggerFactory.CreateLogger<OrderFunctions>();
            _ordersService = ordersService;
        }

        [Function("GetOrders")]
        public async Task<HttpResponseData> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = "orders")] HttpRequestData req)
        {
            _logger.LogInformation("Processing request to fetch orders.");

            try
            {
                // Fetch orders from the JSON file using the service.
                var orders = _ordersService.FetchOrders();

                var response = req.CreateResponse(HttpStatusCode.OK);
                await response.WriteAsJsonAsync(orders);
                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while processing the request.");
                var errorResponse = req.CreateResponse(HttpStatusCode.InternalServerError);
                await errorResponse.WriteStringAsync($"Error fetching products: {ex.Message}");
                return errorResponse;
            }
        }

        [Function("GetOrdersByCompany")]
        public async Task<HttpResponseData> GetOrdersByCompany(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = "orders/company")] HttpRequestData req)
        {
            _logger.LogInformation("Processing request to fetch orders by company.");

            try
            {
                // Parse the query parameter for companyId.
                var query = HttpUtility.ParseQueryString(req.Url.Query);
                var companyId = query["companyId"];

                if (string.IsNullOrEmpty(companyId))
                {
                    var badRequestResponse = req.CreateResponse(HttpStatusCode.BadRequest);
                    await badRequestResponse.WriteStringAsync("Missing or invalid 'companyId' query parameter.");
                    return badRequestResponse;
                }

                // Fetch orders from the service.
                var orders = _ordersService.FetchOrders();

                // Filter orders by the provided companyId.
                var filteredOrders = _ordersService.FilterByCompany(companyId, orders);

                //call the get toal function from the Order Service 
                double totalAmount = _ordersService.CalculateTotalAmount(filteredOrders);


                var orderResponse = new OrderResponse
                {
                    Orders = filteredOrders,
                    TotalAmount = totalAmount
                };

                var response = req.CreateResponse(HttpStatusCode.OK);
                await response.WriteAsJsonAsync(orderResponse);
                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while processing the request.");
                var errorResponse = req.CreateResponse(HttpStatusCode.InternalServerError);
                await errorResponse.WriteStringAsync($"Error fetching orders by company: {ex.Message}");
                return errorResponse;
            }
        }
    }
}
