using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MoverCandidateTest.Inventory.EntityFramework;
using MoverCandidateTest.Inventory.Model;

namespace MoverCandidateTest.Inventory.Command
{
    public class UpdateItemCommand : IUpdateItemCommand
    {
        private readonly EfInventoryItemContext _dbContext;

        public UpdateItemCommand(EfInventoryItemContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<InventoryItem?> AddQuantity(InventoryItem item)
        {
            try
            {
                var itemToUpdate = await _dbContext.Inventory.FindAsync(item.Sku);

                if (itemToUpdate == null) return null;

                itemToUpdate.Quantity += item.Quantity;

                await _dbContext.SaveChangesAsync();

                return itemToUpdate;
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}