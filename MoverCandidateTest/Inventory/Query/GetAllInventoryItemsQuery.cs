using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MoverCandidateTest.Inventory.EntityFramework;
using MoverCandidateTest.Inventory.Model;

namespace MoverCandidateTest.Inventory.Query
{
    public class GetAllInventoryItemsQuery: IGetAllInventoryItemsQuery
    {
        private readonly EfInventoryItemContext _dbContext;

        public GetAllInventoryItemsQuery(EfInventoryItemContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<InventoryItem>> GetAllItems()
        {
            var items = await _dbContext.Inventory.ToListAsync();

            if (items is null) return Enumerable.Empty<InventoryItem>();

            var filteredItems = items.Where(item => item.Quantity != 0);

            return filteredItems;
        }
    }
}