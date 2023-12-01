using Microsoft.EntityFrameworkCore;
using MoverCandidateTest.Inventory.Command;
using MoverCandidateTest.Inventory.EntityFramework;
using MoverCandidateTest.Inventory.Model;

namespace UnitTestMoverCandidateTest.CommandTest
{
    [TestFixture]
    public class CreateItemCommandTests
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
        public async Task CreateItem_ReturnsNewItem_WhenSuccessful()
        {
            // Arrange
            var newItem = new InventoryItem { Sku = "SKU001", Description = "New Item", Quantity = 5 };
            var dbContext = CreateMockDbContext();
            var command = new CreateItemCommand(dbContext);

            // Act
            var result = await command.CreateItem(newItem);

            // Assert
            Assert.IsNotNull(result);
            Assert.That(result?.Sku, Is.EqualTo(newItem.Sku));
            Assert.That(result?.Description, Is.EqualTo(newItem.Description));
            Assert.That(result?.Quantity, Is.EqualTo(newItem.Quantity));
        }

        [Test]
        public async Task CreateItem_ReturnsNull_WhenDbUpdateExceptionOccurs()
        {
            // Arrange
            var newItem = new InventoryItem { Sku = "SKU002", Description = "Another Item", Quantity = 3 };
            var dbContext = CreateMockDbContext();
            // Forcing DbUpdateException by adding an item with a duplicated SKU (Primary Key)
            dbContext.Inventory.Add(new InventoryItem { Sku = "SKU002", Description = "Duplicate Item", Quantity = 1 });
            await dbContext.SaveChangesAsync();

            var command = new CreateItemCommand(dbContext);

            // Act
            var result = await command.CreateItem(newItem);

            // Assert
            Assert.IsNull(result);
        }
    }
}
