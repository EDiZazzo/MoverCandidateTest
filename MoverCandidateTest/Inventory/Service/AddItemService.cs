using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using MoverCandidateTest.Inventory.Command;
using MoverCandidateTest.Inventory.Model;
using MoverCandidateTest.Inventory.Query;
using static MoverCandidateTest.Inventory.Utility.ResultUtility;

namespace MoverCandidateTest.Inventory.Service;

public class AddItemService : IAddItemService
{
    private readonly IGetInventoryItemQuery _query;
    private readonly ICreateItemCommand _createItemCommand;
    private readonly IUpdateItemCommand _updateItemCommand;

    public AddItemService(
        IGetInventoryItemQuery query,
        ICreateItemCommand createItemCommand,
        IUpdateItemCommand updateItemCommand)
    {
        _query = query ?? throw new NullReferenceException();
        _createItemCommand = createItemCommand ?? throw new NullReferenceException();
        _updateItemCommand = updateItemCommand ?? throw new NullReferenceException();
    }

    public async Task<AddItemServiceResult> AddItem(InventoryItem item)
    {
        InventoryItem? existingItem;
            
        try
        {
            existingItem = await _query.GetItem(item.Sku);
        }
        catch
        {
            return InternalErrorOnAddedItemResult();
        }
       

        if (existingItem != null && !existingItem.Description.Equals(item.Description))
        {
            return ConflictOnAddedItemResult();
        }

        var result = existingItem is null
            ? await _createItemCommand.CreateItem(item)
            : await _updateItemCommand.AddQuantity(item);

        return result is not null ? OkAddedItemResult(result) : InternalErrorOnAddedItemResult();
    }
}