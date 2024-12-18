# Basket API Documentation

## Overview

The Basket API is responsible for managing customer baskets in the eShop application. It provides endpoints for adding, updating, and retrieving basket items.

## Endpoints

### Get Basket

**GET** `/api/basket/{buyerId}`

Retrieves the basket for the specified buyer.

#### Parameters

- `buyerId` (string): The ID of the buyer.

#### Responses

- `200 OK`: Returns the basket for the specified buyer.
- `404 Not Found`: If the basket does not exist.

### Update Basket

**PUT** `/api/basket`

Updates the basket with the provided basket data.

#### Request Body

- `CustomerBasket`: The basket data to update.

#### Responses

- `200 OK`: If the basket was successfully updated.
- `400 Bad Request`: If the request data is invalid.

### Add Item to Basket

**POST** `/api/basket/items`

Adds an item to the basket.

#### Request Body

- `BasketItem`: The item to add to the basket.

#### Responses

- `200 OK`: If the item was successfully added.
- `400 Bad Request`: If the request data is invalid.

## Models

### CustomerBasket

Represents a customer's basket.

#### Properties

- `BuyerId` (string): The ID of the buyer.
- `Items` (List<BasketItem>): The list of items in the basket.

### BasketItem

Represents an item in the basket.

#### Properties

- `Id` (string): The ID of the item.
- `ProductId` (int): The ID of the product.
- `ProductName` (string): The name of the product.
- `UnitPrice` (decimal): The unit price of the product.
- `OldUnitPrice` (decimal): The old unit price of the product.
- `Quantity` (int): The quantity of the product.
- `PictureUrl` (string): The URL of the product picture.

## Validation

### BasketItem Validation

- `Quantity` must be greater than or equal to 1. If not, a validation error "Invalid number of units" will be returned.
