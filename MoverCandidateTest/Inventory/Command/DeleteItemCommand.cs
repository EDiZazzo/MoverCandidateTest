using System;
using System.Threading.Tasks;
using MoverCandidateTest.Inventory.EntityFramework;
using MoverCandidateTest.Inventory.Model;

namespace MoverCandidateTest.Inventory.Command;

public class DeleteItemCommand: IDeleteItemCommand
{
    private readonly EfInventoryItemContext _dbContext;

    public DeleteItemCommand(EfInventoryItemContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public async Task<InventoryItem?> RemoveQuantity(InventoryItem item, uint quantity)
    {
        try
        {
            var itemToUpdate = await _dbContext.Inventory.FindAsync(item.Sku);
            
            if (itemToUpdate == null) return null;
            
            itemToUpdate.Quantity -= quantity;
            
            await _dbContext.SaveChangesAsync();
            
            return itemToUpdate;
            

        }
        catch (Exception)
        {
            return null; 
        }
    }
}