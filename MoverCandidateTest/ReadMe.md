# Mover Candidate Test

## Introduktion

Dear candidate.

Welcome to the Mover Candidate Test.
Please use you preferred way of working, and show us what you can :)

Enjoy!

## General

1. Should be able to run with a push of f5 without any installations (NuGet packages are allowed)
2. Include tests
3. Implement in-memory storage of own choice

## Tasks

1. Implement API endpoint: Calculate least angle between watch hands.
   1. Return least angle as output

2. Inventory management
   1. Create APIs to
      1. Create inventory items
         1. With SKU, description and quantity
         2. Adding inventory with unknown SKU, it should create new.
         3. Adding inventory with known SKU, it should add the quantity to existing
      2. Remove a defined quantity for a specific SKU
      3. List of all inventory

## SOLUTION

### 1. Calculate Least Angle between Watch Hands

Implement an API endpoint to calculate the least angle between watch hands.

#### Controller: CalculateLeastAngleController

The `CalculateLeastAngleController` manages the calculation of the least angle between watch hands.

- **Endpoint:** GET `/calculateleastangle/least-angle`
- **Input:** Accepts a `CalculateLeastAngleRequestModel` from the query parameters.
- **Validation:** The controller validates using a provided validator for the request model. If the input is invalid, it returns a `BadRequest` response with validation errors.
- **Calculation:** Utilizes the `ICalculateLeastAngleService` to calculate the least angle based on the provided date and time.
- **Output:** Returns an `Ok` response with a `CalculateLeastAngleResponseModel` containing the calculated least angle.

Example:
```plaintext
GET /calculateleastangle/least-angle?DateTime=2023-12-01T05:40:45
```
### 2. Inventory Management

Implement APIs for managing inventory items.

#### Controller: InventoryItemController

The `InventoryItemController` handles various operations related to inventory management.

#### Endpoints:

##### Add Inventory Item

- **Endpoint:** POST `/api/inventory/addInventoryItem`
- **Description:** Creates a new inventory item or increases the quantity of an existing item.
- **Request Body:** Expects an `AddInventoryItemRequestModel` in JSON format.
- **Validation:** Validates the input model using `_addValidator`. If invalid, returns a `BadRequest` response with validation errors.
- **Output:** Returns an `InventoryItemResponseModel` upon successful addition or a relevant status code for an unsuccessful attempt.

##### Remove Inventory Item Quantity

- **Endpoint:** DELETE `/api/inventory/removeInventoryItemQuantity/{sku}`
- **Description:** Removes a defined quantity for a specific SKU.
- **Input:** Accepts a SKU as a route parameter and a `RemoveItemQuantityRequestModel` in JSON format.
- **Validation:** Validates the input model using `_removeValidator`. If invalid, returns a `BadRequest` response with validation errors.
- **Output:** Returns an `InventoryItemResponseModel` upon successful removal or a relevant status code for an unsuccessful attempt.

##### Get All Inventory Items

- **Endpoint:** GET `/api/inventory/getInventory`
- **Description:** Retrieves a list of all inventory items.
- **Output:** Returns an `InventoryItemListResponseModel` containing all inventory items or a relevant status code if unsuccessful.

The `InventoryItemController` provides methods for adding new inventory items, removing quantities, and retrieving a list of all available inventory items. It performs validation on the input models and responds with appropriate success or failure messages based on the operations performed.
