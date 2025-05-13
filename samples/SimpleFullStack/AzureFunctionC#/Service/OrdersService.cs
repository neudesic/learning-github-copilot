using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using Microsoft.Extensions.Logging;
using api.Model;


namespace api.Services
{


    public interface IOrdersService
    {
        List<Order> FetchOrders();
        List<Order> FilterByCompany(string companyId, List<Order> orders);
        double CalculateTotalAmount(List<Order> orders);
    }

    public class OrdersService : IOrdersService
    {
        private readonly string _ordersFilePath;

        public OrdersService()
        {
            // Use AppContext.BaseDirectory to point to the output folder.
            _ordersFilePath = Path.Combine(AppContext.BaseDirectory, "orders.json");
        }


        public List<Order> FetchOrders()
        {
            if (!File.Exists(_ordersFilePath))
            {
                return new List<Order>();
            }

            string jsonFromFile = File.ReadAllText(_ordersFilePath);
            var orders = JsonSerializer.Deserialize<List<Order>>(jsonFromFile);

            if (orders == null)
            {
                return new List<Order>();
            }
            return orders;
        }

        // Function to filter by company ID.
        public List<Order> FilterByCompany(string companyId, List<Order> orders)
        {
            return orders.FindAll(o => o.CompanyId == companyId);
        }

        // Calculate the total amount of orders.

        public double CalculateTotalAmount(List<Order> orders)
        {
            if (orders == null || orders.Count == 0)
            {
                return 0;
            }

            double totalAmount = 0;
            foreach (var order in orders)
            {
                totalAmount += order.TotalAmount;
            }

            return totalAmount;
        }
    }
}
