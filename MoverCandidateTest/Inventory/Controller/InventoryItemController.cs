using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MoverCandidateTest.Controllers.Inventory.Service;
using MoverCandidateTest.Inventory.Extension;
using MoverCandidateTest.Inventory.Model;
using MoverCandidateTest.Inventory.Service;

namespace MoverCandidateTest.Inventory.Controller
{
    [Route("api/inventory")]
    public class InventoryItemController : Microsoft.AspNetCore.Mvc.Controller
    {
        private readonly ILogger<InventoryItemController> _logger;
        private readonly IAddItemService _addItemService;
        private readonly IRemoveItemService _removeItemService;
        private readonly IGetAllItemsService _getAllItemsService;
        private readonly IValidator<AddInventoryItemRequestModel> _addValidator;
        private readonly IValidator<ValidationRemoveItemQuantityModel> _removeValidator;

        public InventoryItemController(
            ILogger<InventoryItemController> logger,
            IAddItemService addItemService, 
            IRemoveItemService removeItemService, 
            IGetAllItemsService getAllItemsService, 
            IValidator<AddInventoryItemRequestModel> addValidator, 
            IValidator<ValidationRemoveItemQuantityModel> removeValidator)
        {
            _logger = logger ?? throw new ArgumentNullException();;
            _addItemService = addItemService ?? throw new ArgumentNullException();
            _removeItemService = removeItemService ?? throw new ArgumentNullException();
            _getAllItemsService = getAllItemsService ?? throw new ArgumentNullException();
            _addValidator = addValidator ?? throw new ArgumentNullException();
            _removeValidator = removeValidator ?? throw new ArgumentNullException();
        }

        [HttpPost("addInventoryItem")]
        [ProducesResponseType(typeof(InventoryItemResponseModel), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(409)]
        [Consumes("application/json")]
        public async Task<IActionResult> AddInventoryItem([FromBody] AddInventoryItemRequestModel addInventoryItem)
        {
            var validationResult = await _addValidator.ValidateAsync(addInventoryItem);

            if (!validationResult.IsValid) return BadRequest(validationResult.Errors);

            var itemAddedResult = await _addItemService.AddItem(
                    new InventoryItem(addInventoryItem.Sku, addInventoryItem.Description, addInventoryItem.Quantity));

            return itemAddedResult.IsSuccessfully ? 
                Ok(itemAddedResult.Item.ToInventoryItemResponseModel()) : 
                StatusCode(itemAddedResult.StatusCode, itemAddedResult);
        }

        [HttpDelete("removeInventoryItemQuantity/{sku}")]
        [ProducesResponseType(typeof(InventoryItemResponseModel), 200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(409)]
        public async Task<IActionResult> RemoveItemQuantity(string sku, [FromBody] RemoveItemQuantityRequestModel removeItemQuantity)
        {
            var validationResult = await _removeValidator.ValidateAsync(
                new ValidationRemoveItemQuantityModel(sku, removeItemQuantity.Quantity));

            if (!validationResult.IsValid) return BadRequest(validationResult.Errors);

            var itemRemovedResult = await _removeItemService.RemoveItem(sku, removeItemQuantity.Quantity);

            return itemRemovedResult.IsSuccessfully ? 
                Ok(itemRemovedResult.Item.ToInventoryItemResponseModel()) : 
                StatusCode(itemRemovedResult.StatusCode, itemRemovedResult);
        }

        [HttpGet("getInventory")]
        [ProducesResponseType(typeof(InventoryItemListResponseModel), 200)]
        public async Task<IActionResult> GetAllInventoryItems()
        {
            var inventoryResult = await _getAllItemsService.GetAllInventoryItems();
            
            return inventoryResult.IsSuccessfully ?
                Ok(inventoryResult.Inventory.ToInventoryItemListResponseModel()) :
                StatusCode(inventoryResult.StatusCode, inventoryResult);
        }
    }
}