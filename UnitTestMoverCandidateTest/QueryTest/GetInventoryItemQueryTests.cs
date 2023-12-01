using Microsoft.EntityFrameworkCore;
using MoverCandidateTest.Inventory.EntityFramework;
using MoverCandidateTest.Inventory.Model;
using MoverCandidateTest.Inventory.Query;

namespace UnitTestMoverCandidateTest.QueryTest
{
    [TestFixture]
    public class GetInventoryItemQueryTests
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
        public async Task GetItem_ReturnsItem_WhenItemExistsInDatabase()
        {
            // Arrange
            var sku = "SKU001";
            var item = new InventoryItem { Sku = sku, Description = "Item Description", Quantity = 10 };
            var dbContext = CreateMockDbContext();
            dbContext.Inventory.Add(item);
            await dbContext.SaveChangesAsync();

            var query = new GetInventoryItemQuery(dbContext);

            // Act
            var result = await query.GetItem(sku);

            // Assert
            Assert.IsNotNull(result);
            Assert.That(result?.Sku, Is.EqualTo(sku));
            Assert.That(result?.Description, Is.EqualTo(item.Description));
            Assert.That(result?.Quantity, Is.EqualTo(item.Quantity));
        }

        [Test]
        public async Task GetItem_ReturnsNull_WhenItemDoesNotExistInDatabase()
        {
            // Arrange
            var sku = "NonExistingSKU";
            var dbContext = CreateMockDbContext();
            var query = new GetInventoryItemQuery(dbContext);

            // Act
            var result = await query.GetItem(sku);

            // Assert
            Assert.IsNull(result);
        }
    }
}
