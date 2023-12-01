using System;
using System.Threading.Tasks;
using MoverCandidateTest.Inventory.EntityFramework;
using MoverCandidateTest.Inventory.Model;

namespace MoverCandidateTest.Inventory.Command;

public class CreateItemCommand : ICreateItemCommand
{
    private readonly EfInventoryItemContext _dbContext;

    public CreateItemCommand(EfInventoryItemContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<InventoryItem?> CreateItem(InventoryItem item)
    {
        try
        {
            var newItem = new InventoryItem(item.Sku, item.Description, item.Quantity);
            _dbContext.Inventory.Add(newItem);
            await _dbContext.SaveChangesAsync();

            return item;
        }
        catch (Exception ex)
        {
            return null;
        }
    }
}