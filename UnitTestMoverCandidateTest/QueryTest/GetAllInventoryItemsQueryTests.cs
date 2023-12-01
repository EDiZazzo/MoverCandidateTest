using Microsoft.EntityFrameworkCore;
using MoverCandidateTest.Inventory.EntityFramework;
using MoverCandidateTest.Inventory.Model;
using MoverCandidateTest.Inventory.Query;

namespace UnitTestMoverCandidateTest.QueryTest
{
    [TestFixture]
    public class GetAllInventoryItemsQueryTests
    {
        [Test]
        public async Task GetAllItems_ReturnsFilteredItems()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<EfInventoryItemContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            var dbContext = new EfInventoryItemContext(options);
            await dbContext.Database.EnsureDeletedAsync(); // Ensure any previous data is deleted
            dbContext.Inventory.AddRange(new List<InventoryItem>
            {
                new InventoryItem { Sku = "SKU001", Description = "Description 1", Quantity = 10 },
                new InventoryItem { Sku = "SKU002", Description = "Description 2", Quantity = 0 },
                new InventoryItem { Sku = "SKU003", Description = "Description 3", Quantity = 5 }
            });
            await dbContext.SaveChangesAsync();

            var query = new GetAllInventoryItemsQuery(dbContext);

            // Act
            var result = await query.GetAllItems();

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<IEnumerable<InventoryItem>>(result);

            var resultList = result.ToList();
            Assert.That(resultList.Count, Is.EqualTo(2));

            Assert.IsTrue(resultList.Any(item => item.Sku == "SKU001"));
            Assert.IsTrue(resultList.Any(item => item.Sku == "SKU003"));
        }

        [Test]
        public async Task GetAllItems_ReturnsEmpty_WhenNoItems()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<EfInventoryItemContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            var dbContext = new EfInventoryItemContext(options);
            await dbContext.Database.EnsureDeletedAsync(); // Ensure any previous data is deleted

            var query = new GetAllInventoryItemsQuery(dbContext);

            // Act
            var result = await query.GetAllItems();

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<IEnumerable<InventoryItem>>(result);
            Assert.IsEmpty(result);
        }
        
        [Test]
        public async Task GetAllItems_ExcludesItemsWithZeroQuantity()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<EfInventoryItemContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            var dbContext = new EfInventoryItemContext(options);
            await dbContext.Database.EnsureDeletedAsync(); // Ensure any previous data is deleted
            dbContext.Inventory.AddRange(new List<InventoryItem>
            {
                new InventoryItem { Sku = "SKU001", Description = "Description 1", Quantity = 10 },
                new InventoryItem { Sku = "SKU002", Description = "Description 2", Quantity = 0 },
                new InventoryItem { Sku = "SKU003", Description = "Description 3", Quantity = 5 }
            });
            await dbContext.SaveChangesAsync();

            var query = new GetAllInventoryItemsQuery(dbContext);

            // Act
            var result = await query.GetAllItems();

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<IEnumerable<InventoryItem>>(result);

            var resultList = result.ToList();
            Assert.That(resultList.Count, Is.EqualTo(2));

            Assert.IsTrue(resultList.Any(item => item.Sku == "SKU001"));
            Assert.IsTrue(resultList.Any(item => item.Sku == "SKU003"));
            Assert.IsFalse(resultList.Any(item => item.Sku == "SKU002"));
        }

    }
}
