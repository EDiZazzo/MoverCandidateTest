using Microsoft.AspNetCore.Http;
using Moq;
using MoverCandidateTest.Inventory.Command;
using MoverCandidateTest.Inventory.Model;
using MoverCandidateTest.Inventory.Query;
using MoverCandidateTest.Inventory.Service;

namespace UnitTestMoverCandidateTest.ServiceTest
{
    [TestFixture]
    public class AddItemServiceTests
    {
        private AddItemService _addItemService;
        private Mock<IGetInventoryItemQuery> _mockGetQuery;
        private Mock<ICreateItemCommand> _mockCreateCommand;
        private Mock<IUpdateItemCommand> _mockUpdateCommand;

        [SetUp]
        public void SetUp()
        {
            _mockGetQuery = new Mock<IGetInventoryItemQuery>();
            _mockCreateCommand = new Mock<ICreateItemCommand>();
            _mockUpdateCommand = new Mock<IUpdateItemCommand>();

            _addItemService = new AddItemService(
                _mockGetQuery.Object,
                _mockCreateCommand.Object,
                _mockUpdateCommand.Object
            );
        }

        [Test]
        public async Task AddItem_WithNewItem_ReturnsSuccess()
        {
            // Arrange
            var newItem = new InventoryItem("SKU123", "New Item", 10);
            _mockGetQuery.Setup(q => q.GetItem("SKU123")).ReturnsAsync((InventoryItem)null);
            _mockCreateCommand.Setup(c => c.CreateItem(newItem)).ReturnsAsync(newItem);

            // Act
            var result = await _addItemService.AddItem(newItem);

            // Assert
            Assert.IsTrue(result.IsSuccessfully);
            Assert.That(result.StatusCode, Is.EqualTo(StatusCodes.Status200OK));
            Assert.That(result.Item, Is.EqualTo(newItem));
            Assert.IsEmpty(result.ErrorMessage);
        }

        [Test]
        public async Task AddItem_WithExistingItemAndMatchingDescription_ReturnsSuccess()
        {
            // Arrange
            var existingItem = new InventoryItem("SKU123", "Existing Item", 5);
            var updatedItem = new InventoryItem("SKU123", "Existing Item", 10);
            _mockGetQuery.Setup(q => q.GetItem("SKU123")).ReturnsAsync(existingItem);
            _mockUpdateCommand.Setup(u => u.AddQuantity(updatedItem)).ReturnsAsync(updatedItem);

            // Act
            var result = await _addItemService.AddItem(updatedItem);

            // Assert
            Assert.IsTrue(result.IsSuccessfully);
            Assert.That(result.StatusCode, Is.EqualTo(StatusCodes.Status200OK));
            Assert.That(result.Item, Is.EqualTo(updatedItem));
            Assert.IsEmpty(result.ErrorMessage);
        }

        [Test]
        public async Task AddItem_WithExistingItemAndDifferentDescription_ReturnsConflict()
        {
            // Arrange
            var existingItem = new InventoryItem("SKU123", "Existing Item", 5);
            var newItem = new InventoryItem("SKU123", "New Description", 10);
            _mockGetQuery.Setup(q => q.GetItem("SKU123")).ReturnsAsync(existingItem);

            // Act
            var result = await _addItemService.AddItem(newItem);

            // Assert
            Assert.IsFalse(result.IsSuccessfully);
            Assert.That(result.StatusCode, Is.EqualTo(StatusCodes.Status409Conflict));
            Assert.IsNull(result.Item);
        }
    }
}