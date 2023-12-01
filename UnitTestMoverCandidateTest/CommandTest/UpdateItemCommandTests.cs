using Microsoft.EntityFrameworkCore;
using MoverCandidateTest.Inventory.Command;
using MoverCandidateTest.Inventory.EntityFramework;
using MoverCandidateTest.Inventory.Model;

namespace UnitTestMoverCandidateTest.CommandTest
{
    [TestFixture]
    public class UpdateItemCommandTests
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
        public async Task AddQuantity_ReturnsUpdatedItem_WhenSuccessful()
        {
            // Arrange
            var existingItem = new InventoryItem { Sku = "SKU001", Description = "Existing Item", Quantity = 10 };
            var dbContext = CreateMockDbContext();
            dbContext.Inventory.Add(existingItem);
            await dbContext.SaveChangesAsync();

            var command = new UpdateItemCommand(dbContext);

            // Act
            var newItem = new InventoryItem { Sku = "SKU001", Quantity = 5 };
            var result = await command.AddQuantity(newItem);

            // Assert
            Assert.IsNotNull(result);
            Assert.That(result?.Sku, Is.EqualTo(existingItem.Sku));
            Assert.That(result?.Description, Is.EqualTo(existingItem.Description));
            Assert.That(result?.Quantity, Is.EqualTo(15));
        }

        [Test]
        public async Task AddQuantity_ReturnsNull_WhenItemNotFound()
        {
            // Arrange
            var nonExistingItem = new InventoryItem { Sku = "SKU002", Description = "Non-existing Item", Quantity = 5 };
            var dbContext = CreateMockDbContext();
            var command = new UpdateItemCommand(dbContext);

            // Act
            var result = await command.AddQuantity(nonExistingItem);

            // Assert
            Assert.IsNull(result);
        }

        [Test]
        public async Task AddQuantity_ReturnsNull_WhenDbUpdateExceptionOccurs()
        {
            // Arrange
            var existingItem = new InventoryItem { Sku = "SKU003", Description = "Existing Item", Quantity = 5 };
            var dbContext = CreateMockDbContext();
            dbContext.Inventory.Add(existingItem);
            await dbContext.SaveChangesAsync();

            // Creating a command with the existing DbContext
            var command = new UpdateItemCommand(dbContext);

            // Act
            var invalidItem = new InventoryItem { Sku = "SKU0035", Quantity = 10 }; // Using the existing SKU to trigger DbUpdateException
            var result = await command.AddQuantity(invalidItem);

            // Assert
            Assert.IsNull(result);
        }
    }
}
