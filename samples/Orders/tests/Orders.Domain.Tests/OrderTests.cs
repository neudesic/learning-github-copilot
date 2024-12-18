
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace Orders.Domain.Tests
{
    [TestClass]
    public class OrderTest
    {
        [TestMethod]
        public void AddOrderItem_ShouldAddItemToOrder()
        {
            // Arrange
            var order = new Order();
            var orderItem = new OrderItem
            {
                Id = 1,
                ProductName = "Test Product",
                Price = 10.0m,
                Quantity = 2
            };

            // Act
            order.AddOrderItem(orderItem);

            // Assert
            Assert.AreEqual(1, order.Items.Count);
            Assert.AreEqual(orderItem, order.Items[0]);
        }

        [TestMethod]
        public void AddOrderItem_ShouldIncreaseOrderTotal()
        {
            // Arrange
            var order = new Order();
            var orderItem = new OrderItem
            {
                Id = 1,
                ProductName = "Test Product",
                Price = 10.0m,
                Quantity = 2
            };

            // Act
            order.AddOrderItem(orderItem);
            var total = order.GetTotal();

            // Assert
            Assert.AreEqual(20.0m, total);
        }

        [TestMethod]
        public void GetTotal_ShouldReturnZero_WhenNoItems()
        {
            // Arrange
            var order = new Order();

            // Act
            var total = order.GetTotal();

            // Assert
            Assert.AreEqual(0.0m, total);
        }

        [TestMethod]
        public void GetTotal_ShouldReturnCorrectTotal_WithSingleItem()
        {
            // Arrange
            var order = new Order();
            var orderItem = new OrderItem
            {
                Id = 1,
                ProductName = "Test Product",
                Price = 10.0m,
                Quantity = 2
            };
            order.AddOrderItem(orderItem);

            // Act
            var total = order.GetTotal();

            // Assert
            Assert.AreEqual(20.0m, total);
        }

        [TestMethod]
        public void GetTotal_ShouldReturnCorrectTotal_WithMultipleItems()
        {
            // Arrange
            var order = new Order();
            var orderItem1 = new OrderItem
            {
                Id = 1,
                ProductName = "Test Product 1",
                Price = 10.0m,
                Quantity = 2
            };
            var orderItem2 = new OrderItem
            {
                Id = 2,
                ProductName = "Test Product 2",
                Price = 15.0m,
                Quantity = 1
            };
            order.AddOrderItem(orderItem1);
            order.AddOrderItem(orderItem2);

            // Act
            var total = order.GetTotal();

            // Assert
            Assert.AreEqual(35.0m, total);
        }

        [TestMethod]
        public void GetTotal_ShouldReturnZero_WithZeroQuantity()
        {
            // Arrange
            var order = new Order();
            var orderItem = new OrderItem
            {
                Id = 1,
                ProductName = "Test Product",
                Price = 10.0m,
                Quantity = 0
            };
            order.AddOrderItem(orderItem);

            // Act
            var total = order.GetTotal();

            // Assert
            Assert.AreEqual(0.0m, total);
        }

        [TestMethod]
        public void GetTotal_ShouldReturnNegativeTotal_WithNegativePrice()
        {
            // Arrange
            var order = new Order();
            var orderItem = new OrderItem
            {
                Id = 1,
                ProductName = "Test Product",
                Price = -10.0m,
                Quantity = 2
            };
            order.AddOrderItem(orderItem);

            // Act
            var total = order.GetTotal();

            // Assert
            Assert.AreEqual(-20.0m, total);
        }

        [TestMethod]
        public void RemoveOrderTest_ShouldRemoveItemFromOrder()
        {
            // Arrange
            var order = new Order();
            var orderItem = new OrderItem
            {
                Id = 1,
                ProductName = "Test Product",
                Price = 10.0m,
                Quantity = 2
            };
            order.AddOrderItem(orderItem);

            // Act
            order.RemoveOrderItem(orderItem);

            // Assert
            Assert.AreEqual(0, order.Items.Count);
        }
    }
}