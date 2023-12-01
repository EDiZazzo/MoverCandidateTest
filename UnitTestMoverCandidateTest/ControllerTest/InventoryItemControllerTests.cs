using System.Collections;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using MoverCandidateTest.Inventory.Controller;
using MoverCandidateTest.Inventory.Model;
using MoverCandidateTest.Inventory.Service;
using MoverCandidateTest.Inventory.Utility;

namespace UnitTestMoverCandidateTest.ControllerTest
{
    [TestFixture]
    public class InventoryItemControllerTests
    {
        private InventoryItemController _inventoryItemController;
        private Mock<ILogger<InventoryItemController>> _loggerMock;
        private Mock<IAddItemService> _addItemServiceMock;
        private Mock<IRemoveItemService> _removeItemServiceMock;
        private Mock<IGetAllItemsService> _getAllItemsServiceMock;
        private Mock<IValidator<AddInventoryItemRequestModel>> _addValidatorMock;
        private Mock<IValidator<ValidationRemoveItemQuantityModel>> _removeValidatorMock;

        [SetUp]
        public void Setup()
        {
            _loggerMock = new Mock<ILogger<InventoryItemController>>();
            _addItemServiceMock = new Mock<IAddItemService>();
            _removeItemServiceMock = new Mock<IRemoveItemService>();
            _getAllItemsServiceMock = new Mock<IGetAllItemsService>();
            _addValidatorMock = new Mock<IValidator<AddInventoryItemRequestModel>>();
            _removeValidatorMock = new Mock<IValidator<ValidationRemoveItemQuantityModel>>();

            _inventoryItemController = new InventoryItemController(
                _loggerMock.Object,
                _addItemServiceMock.Object,
                _removeItemServiceMock.Object,
                _getAllItemsServiceMock.Object,
                _addValidatorMock.Object,
                _removeValidatorMock.Object
            );
        }

        [Test]
        public async Task AddInventoryItem_WhenValidInput_ReturnsOkResult()
        {
            // Arrange
            var validModel = new AddInventoryItemRequestModel("SKU001", "Test Item", 5);
            _addValidatorMock.Setup(validator => validator.ValidateAsync(
                    validModel, new ()))
                .ReturnsAsync(new FluentValidation.Results.ValidationResult());

            _addItemServiceMock.Setup(service => service.AddItem(It.IsAny<InventoryItem>()))
                .ReturnsAsync(ResultUtility.OkAddedItemResult(
                    new (validModel.Sku, validModel.Description, validModel.Quantity)));

            // Act
            var result = await _inventoryItemController.AddInventoryItem(validModel) as ObjectResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.That(result.StatusCode, Is.EqualTo(StatusCodes.Status200OK));
            Assert.IsNotNull(result.Value);
            Assert.IsInstanceOf<InventoryItemResponseModel>(result.Value);
        } 
        
        [Test]
        public async Task AddInventoryItem_WhenInvalidInput_ReturnsBadRequest()
        {
            // Arrange
            var invalidModel = new AddInventoryItemRequestModel(null, "", 0);
            var validationErrors = new FluentValidation.Results.ValidationResult();
            validationErrors.Errors.Add(
                new FluentValidation.Results.ValidationFailure("SKU", "SKU cannot be null."));

            _addValidatorMock.Setup(validator => validator.ValidateAsync(
                        invalidModel, new ()))
                    .ReturnsAsync(validationErrors);

            // Act
            var result = await _inventoryItemController.AddInventoryItem(invalidModel) as BadRequestObjectResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.That(result.StatusCode, Is.EqualTo(StatusCodes.Status400BadRequest));
            Assert.IsNotNull(result.Value);
        }

        [Test]
        public async Task AddInventoryItem_WhenAddItemFails_ReturnsStatusCode()
        {
            // Arrange
            var validModel = new AddInventoryItemRequestModel("SKU001", "Test Item", 5);
            var errorResult = ResultUtility.ConflictOnAddedItemResult();

            _addValidatorMock.Setup(validator => validator.ValidateAsync(validModel, new()))
                .ReturnsAsync(new FluentValidation.Results.ValidationResult());

            _addItemServiceMock.Setup(service => service.AddItem(It.IsAny<InventoryItem>()))
                .ReturnsAsync(errorResult);

            // Act
            var result = await _inventoryItemController.AddInventoryItem(validModel) as ObjectResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.That(result.StatusCode, Is.EqualTo(StatusCodes.Status409Conflict));
            Assert.That(((AddItemServiceResult)result.Value).ErrorMessage, Is.EqualTo(errorResult.ErrorMessage));
        }
        
        [Test]
        public async Task RemoveInventoryItem_WhenValidInput_ReturnsOkResult()
        {
            // Arrange
            var validSku = "InvalidSKU";
            uint validQuantity = 10;
            
            _removeValidatorMock.Setup(validator => validator.ValidateAsync(
                    new ValidationRemoveItemQuantityModel(validSku, validQuantity), new ()))
                .ReturnsAsync(new FluentValidation.Results.ValidationResult());
            
            _removeItemServiceMock.Setup(service => service.RemoveItem(validSku, validQuantity))
                .ReturnsAsync(ResultUtility.OkRemovedItemResult(new InventoryItem(validSku, "This is a fun description", 0)));

            // Act
            var result = await _inventoryItemController.RemoveItemQuantity(
                validSku, new RemoveItemQuantityRequestModel(validQuantity)) as ObjectResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.That(result.StatusCode, Is.EqualTo(StatusCodes.Status200OK));
            Assert.IsNotNull(result.Value);
            Assert.IsInstanceOf<InventoryItemResponseModel>(result.Value);
        } 

   
        [Test]
        public async Task RemoveInventoryItem_WhenInvalidInput_ReturnsBadRequest()
        {
            // Arrange
            var invalidSku = "";
            uint invalidQuantity = 0;
            var validationErrors = new FluentValidation.Results.ValidationResult();
            validationErrors.Errors.Add(
                new FluentValidation.Results.ValidationFailure("SKU", "SKU cannot be null."));

            _removeValidatorMock.Setup(validator => validator.ValidateAsync(
                    new ValidationRemoveItemQuantityModel(invalidSku, invalidQuantity), new ()))
                .ReturnsAsync(validationErrors);

            // Act
            var result = await _inventoryItemController.RemoveItemQuantity(
                invalidSku,
                new RemoveItemQuantityRequestModel(invalidQuantity)) as ObjectResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.That(result.StatusCode, Is.EqualTo(StatusCodes.Status400BadRequest));
            Assert.IsNotNull(result.Value);
        }

        [Test]
        public async Task RemoveItemQuantity_WhenItemNotFound_ReturnsNotFound()
        {
            // Arrange
            var invalidSku = "InvalidSKU";
            uint invalidQuantity = 10;
            var validationErrors = new FluentValidation.Results.ValidationResult();

            _removeValidatorMock.Setup(validator => validator.ValidateAsync(
                    new ValidationRemoveItemQuantityModel(invalidSku, invalidQuantity), new ()))
                .ReturnsAsync(validationErrors);

            _removeItemServiceMock.Setup(service => service.RemoveItem(invalidSku, invalidQuantity))
                .ReturnsAsync(ResultUtility.NotFoundItemToRemoveResult);

            // Act
            var result = await _inventoryItemController.RemoveItemQuantity(
                invalidSku,
                new RemoveItemQuantityRequestModel(invalidQuantity)) as ObjectResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.That(result.StatusCode, Is.EqualTo(StatusCodes.Status404NotFound));
            Assert.IsNotNull(result.Value);
        }

        [Test]
        public async Task GetAllInventoryItems_WhenInventoryEmpty_ReturnsEmptyList()
        {
            // Arrange
            _getAllItemsServiceMock.Setup(service => service.GetAllInventoryItems())
                .ReturnsAsync(ResultUtility.OkGetAllInventoryResult(new List<InventoryItem>()));

            // Act
            var result = await _inventoryItemController.GetAllInventoryItems() as ObjectResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.That(result.StatusCode, Is.EqualTo(StatusCodes.Status200OK));
            Assert.IsNotNull(result.Value);
            Assert.IsNull(result.Value as IEnumerable<InventoryItem>);
        }

        [Test]
        public async Task GetAllInventoryItems_WhenServiceFails_ReturnsStatusCode()
        {
            // Arrange
            var errorResult = ResultUtility.InternalErrorOnGetAllInventoryResult();

            _getAllItemsServiceMock.Setup(service => service.GetAllInventoryItems())
                .ReturnsAsync(errorResult);

            // Act
            var result = await _inventoryItemController.GetAllInventoryItems() as ObjectResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.That(result.StatusCode, Is.EqualTo(StatusCodes.Status500InternalServerError));
        }
    }
}
