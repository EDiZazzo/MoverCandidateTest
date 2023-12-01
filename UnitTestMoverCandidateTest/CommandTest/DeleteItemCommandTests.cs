using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Moq;
using NUnit.Framework;
using MoverCandidateTest.Inventory.Command;
using MoverCandidateTest.Inventory.EntityFramework;
using MoverCandidateTest.Inventory.Model;

namespace MoverCandidateTest.Inventory.Command.Tests
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
            var result = await command.RemoveQuantity(existingItem, 5);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(existingItem.Sku, result?.Sku);
            Assert.AreEqual(existingItem.Description, result?.Description);
            Assert.AreEqual(5, result?.Quantity);
        }

        [Test]
        public async Task RemoveQuantity_ReturnsNull_WhenItemNotFound()
        {
            // Arrange
            var nonExistingItem = new InventoryItem { Sku = "SKU002", Description = "Non-existing Item", Quantity = 5 };
            var dbContext = CreateMockDbContext();
            var command = new DeleteItemCommand(dbContext);

            // Act
            var result = await command.RemoveQuantity(nonExistingItem, 3);

            // Assert
            Assert.IsNull(result);
        }

        [Test]
        public async Task RemoveQuantity_ReturnsNull_WhenDbUpdateExceptionOccurs()
        {
            // Arrange
            var existingItem = new InventoryItem { Sku = "SKU003", Description = "Existing Item", Quantity = 5 };
            var dbContext = CreateMockDbContext();
            dbContext.Inventory.Add(existingItem);
            await dbContext.SaveChangesAsync();

            // Setting an invalid quantity to cause a DbUpdateException
            var command = new DeleteItemCommand(dbContext);

            // Act
            var result = await command.RemoveQuantity(existingItem, 10);

            // Assert
            Assert.IsNull(result);
        }
    }
}
