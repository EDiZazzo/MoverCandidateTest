using Microsoft.EntityFrameworkCore;
using MoverCandidateTest.Inventory.Command;
using MoverCandidateTest.Inventory.EntityFramework;
using MoverCandidateTest.Inventory.Model;

namespace UnitTestMoverCandidateTest.CommandTest
{
    [TestFixture]
    public class DeleteItemCommandTests
    {
        private EfInventoryItemContext CreateMockDbContext()
        {
            var options = new DbContextOptionsBuilder<EfInventoryItemContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()) 
                .Options;

            var dbContext = new EfInventoryItemContext(options);
            return dbContext;
        }

        [Test]
        public async Task RemoveQuantity_ReturnsUpdatedItem_WhenSuccessful()
        {
            // Arrange
            var existingItem = new InventoryItem { Sku = "SKU001", Description = "Existing Item", Quantity = 10 };
            var dbContext = CreateMockDbContext();
            dbContext.Inventory.Add(existingItem);
            await dbContext.SaveChangesAsync();

            var command = new DeleteItemCommand(dbContext);

            // Act
            var result = await command.RemoveQuantity(existingItem.Sku, 5);

            // Assert
            Assert.IsNotNull(result);
            Assert.That(result?.Sku, Is.EqualTo(existingItem.Sku));
            Assert.That(result?.Description, Is.EqualTo(existingItem.Description));
            Assert.That(result?.Quantity, Is.EqualTo(5));
        }

        [Test]
        public async Task RemoveQuantity_ReturnsNull_WhenItemNotFound()
        {
            // Arrange
            var nonExistingItem = new InventoryItem { Sku = "SKU002", Description = "Non-existing Item", Quantity = 5 };
            var dbContext = CreateMockDbContext();
            var command = new DeleteItemCommand(dbContext);

            // Act
            var result = await command.RemoveQuantity(nonExistingItem.Sku, 3);

            // Assert
            Assert.IsNull(result);
        }
    }
}
