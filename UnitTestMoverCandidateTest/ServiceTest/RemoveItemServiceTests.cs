using Microsoft.AspNetCore.Http;
using Moq;
using MoverCandidateTest.Inventory.Command;
using MoverCandidateTest.Inventory.Model;
using MoverCandidateTest.Inventory.Query;
using MoverCandidateTest.Inventory.Service;

namespace UnitTestMoverCandidateTest.ServiceTest
{
    [TestFixture]
    public class RemoveItemServiceTests
    {
        private Mock<IGetInventoryItemQuery> _mockGetQuery;
        private Mock<IDeleteItemCommand> _mockDeleteCommand;
        private IRemoveItemService _removeItemService;

        [SetUp]
        public void Setup()
        {
            _mockGetQuery = new Mock<IGetInventoryItemQuery>();
            _mockDeleteCommand = new Mock<IDeleteItemCommand>();
            _removeItemService = new RemoveItemService(_mockGetQuery.Object, _mockDeleteCommand.Object);
        }

        [Test]
        public async Task RemoveItem_ReturnsNotFound_WhenItemDoesNotExist()
        {
            // Arrange
            string sku = "NonExistingSKU";
            uint quantity = 5;
            InventoryItem nullItem = null;
            _mockGetQuery.Setup(q => q.GetItem(sku)).ReturnsAsync(nullItem);

            // Act
            var result = await _removeItemService.RemoveItem(sku, quantity);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsFalse(result.IsSuccessfully);
            Assert.That(result.StatusCode, Is.EqualTo(StatusCodes.Status404NotFound));
            Assert.That(result.ErrorMessage, Is.EqualTo("The item with the provided SKU was not found."));
        }

        [Test]
        public async Task RemoveItem_ReturnsInternalServerError_WhenDeleteCommandFails()
        {
            // Arrange
            string sku = "ExistingSKU";
            uint quantity = 5;
            var existingItem = new InventoryItem(sku, "Existing Item", 10);
            _mockGetQuery.Setup(q => q.GetItem(sku)).ReturnsAsync(existingItem);
            _mockDeleteCommand.Setup(d => d.RemoveQuantity(existingItem, quantity)).ReturnsAsync((InventoryItem)null);

            // Act
            var result = await _removeItemService.RemoveItem(sku, quantity);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsFalse(result.IsSuccessfully);
            Assert.That(result.StatusCode, Is.EqualTo(StatusCodes.Status500InternalServerError));
            Assert.That(result.ErrorMessage, Is.EqualTo("An error occurred while processing the internal database operation."));
            Assert.IsNull(result.Item);
        }
        
        [Test]
        public async Task RemoveItem_ReturnsConflict_WhenQuantityIsGreaterThanAvailable()
        {
            // Arrange
            string sku = "ExistingSKU";
            uint quantity = 15; // quantity > available quantity
            var existingItem = new InventoryItem(sku, "Existing Item", 10);
            _mockGetQuery.Setup(q => q.GetItem(sku)).ReturnsAsync(existingItem);

            // Act
            var result = await _removeItemService.RemoveItem(sku, quantity);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsFalse(result.IsSuccessfully);
            Assert.That(result.StatusCode, Is.EqualTo(StatusCodes.Status409Conflict));
            Assert.IsNull(result.Item);
        }

        [Test]
        public async Task RemoveItem_ReturnsSuccess_WhenItemIsDeletedSuccessfully()
        {
            // Arrange
            string sku = "ExistingSKU";
            uint quantity = 5;
            var existingItem = new InventoryItem(sku, "Existing Item", 10);
            var expectedDeletedItem = new InventoryItem(sku, "Existing Item", 5);
            _mockGetQuery.Setup(q => q.GetItem(sku)).ReturnsAsync(existingItem);
            _mockDeleteCommand.Setup(d => d.RemoveQuantity(existingItem, quantity)).ReturnsAsync(expectedDeletedItem);

            // Act
            var result = await _removeItemService.RemoveItem(sku, quantity);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsTrue(result.IsSuccessfully);
            Assert.That(result.StatusCode, Is.EqualTo(StatusCodes.Status200OK));
            Assert.That(result.ErrorMessage, Is.EqualTo(string.Empty));
            Assert.That(result.Item, Is.EqualTo(expectedDeletedItem));
        }

        [Test]
        public async Task RemoveItem_ReturnsInternalServerError_WhenExceptionOccurs()
        {
            // Arrange
            string sku = "ExistingSKU";
            uint quantity = 5;
            var existingItem = new InventoryItem(sku, "Existing Item", 10);
            _mockGetQuery.Setup(q => q.GetItem(sku)).ReturnsAsync(existingItem);
            _mockDeleteCommand.Setup(d => d.RemoveQuantity(existingItem, quantity)).ThrowsAsync(new Exception());

            // Act
            var result = await _removeItemService.RemoveItem(sku, quantity);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsFalse(result.IsSuccessfully);
            Assert.That(result.StatusCode, Is.EqualTo(StatusCodes.Status500InternalServerError));
            Assert.IsNull(result.Item);
        }
    }
}
