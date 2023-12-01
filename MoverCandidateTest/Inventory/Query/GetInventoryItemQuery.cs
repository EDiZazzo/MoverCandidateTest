using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MoverCandidateTest.Inventory.EntityFramework;
using MoverCandidateTest.Inventory.Model;

namespace MoverCandidateTest.Inventory.Query
{
    public class GetInventoryItemQuery : IGetInventoryItemQuery
    {
        private readonly EfInventoryItemContext _dbContext;

        public GetInventoryItemQuery(EfInventoryItemContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<InventoryItem?> GetItem(string sku)
        {
            var item = await _dbContext.Inventory.FirstOrDefaultAsync(i => 
                i.Sku.Equals(sku));

            return item;
        }
    }
}