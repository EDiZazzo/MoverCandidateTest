using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Http;
using MoverCandidateTest.Inventory.Model;

namespace MoverCandidateTest.Inventory.Utility;

public static class ResultUtility
{
    public static DeleteItemServiceResult OkRemovedItemResult(InventoryItem removedItem) =>
        new(
            true, 
            removedItem, 
            StatusCodes.Status200OK,
            string.Empty);
    
    public static DeleteItemServiceResult InternalErrorOnRemovedItemResult() =>
        new(
            false, 
            null, 
            StatusCodes.Status500InternalServerError, 
            "An error occurred while processing the internal database operation.");

    public static DeleteItemServiceResult ConflictOnRemovedItemResult(uint quantityInStock, uint quantityToRemove) =>
        new(
            false,
            null,
            StatusCodes.Status409Conflict,
            $"The quantity to be removed ({quantityToRemove}) exceeds the available stock ({quantityInStock}).");
    
    public static DeleteItemServiceResult NotFoundItemToRemoveResult() => 
        new(
            false, 
            null, 
            StatusCodes.Status404NotFound,
            "The item with the provided SKU was not found.");
    
    public static AddItemServiceResult OkAddedItemResult(InventoryItem addedItem) =>
        new(
            true, 
            addedItem, 
            StatusCodes.Status200OK,
            string.Empty);
    
    public static AddItemServiceResult InternalErrorOnAddedItemResult() =>
        new(
            false, 
            null, 
            StatusCodes.Status500InternalServerError, 
            "An error occurred while processing the internal database operation.");

    public static AddItemServiceResult ConflictOnAddedItemResult() =>
        new(
            false,
            null,
            StatusCodes.Status409Conflict,
            "Provided description does not match the existing item's description");
    
    public static GetAllItemsServiceResult OkGetAllInventoryResult(IEnumerable<InventoryItem> inventory) =>
        new(
            true, 
            inventory, 
            StatusCodes.Status200OK,
            string.Empty);
    
    public static GetAllItemsServiceResult InternalErrorOnGetAllInventoryResult() =>
        new(
            false, 
            Enumerable.Empty<InventoryItem>(), 
            StatusCodes.Status500InternalServerError, 
            "An error occurred while processing the internal database operation.");
    
}