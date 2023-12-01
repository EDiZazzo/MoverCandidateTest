using Microsoft.AspNetCore.Http;
using Moq;
using MoverCandidateTest.Inventory.Model;
using MoverCandidateTest.Inventory.Query;
using MoverCandidateTest.Inventory.Service;

namespace UnitTestMoverCandidateTest.ServiceTest
{
    [TestFixture]
    public class GetAllItemsServiceTests
    {
        [Test]
        public async Task GetAllInventoryItems_Returns_Ok()
        {
            // Arrange
            var queryMock = new Mock<IGetAllInventoryItemsQuery>();
            var inventoryItems = new List<InventoryItem>
            {
                new InventoryItem { Sku = "SKU001", Description = "Description 1", Quantity = 10 },
                new InventoryItem { Sku = "SKU002", Description = "Description 2", Quantity = 15 }
                // Add more sample items if needed
            };
            queryMock.Setup(q => q.GetAllItems()).ReturnsAsync(inventoryItems);
            var service = new GetAllItemsService(queryMock.Object);

            // Act
            var result = await service.GetAllInventoryItems();

            // Assert
            Assert.IsTrue(result.IsSuccessfully);
            Assert.AreEqual(StatusCodes.Status200OK, result.StatusCode);
            Assert.IsEmpty(result.ErrorMessage);
            Assert.AreEqual(inventoryItems, result.Inventory);
        }

        [Test]
        public async Task GetAllInventoryItems_Returns_InternalServerError_OnException()
        {
            // Arrange
            var queryMock = new Mock<IGetAllInventoryItemsQuery>();
            queryMock.Setup(q => q.GetAllItems()).ThrowsAsync(new Exception());
            var service = new GetAllItemsService(queryMock.Object);

            // Act
            var result = await service.GetAllInventoryItems();

            // Assert
            Assert.IsFalse(result.IsSuccessfully);
            Assert.AreEqual(StatusCodes.Status500InternalServerError, result.StatusCode);
            Assert.AreEqual("An error occurred while processing the internal database operation.", result.ErrorMessage);
            Assert.IsEmpty(result.Inventory);
        }
    }
}
