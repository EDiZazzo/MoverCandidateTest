using System;
using System.Threading.Tasks;
using MoverCandidateTest.Inventory.Command;
using MoverCandidateTest.Inventory.Model;
using MoverCandidateTest.Inventory.Query;
using static MoverCandidateTest.Inventory.Utility.ResultUtility;

namespace MoverCandidateTest.Inventory.Service
{
    public class RemoveItemService : IRemoveItemService
    {
        private readonly IGetInventoryItemQuery _query;
        private readonly IDeleteItemCommand _deleteItemCommand;

        public RemoveItemService(IGetInventoryItemQuery query, IDeleteItemCommand deleteItemCommand)
        {
            _query = query ?? throw new ArgumentNullException(nameof(query));
            _deleteItemCommand = deleteItemCommand ?? throw new ArgumentNullException(nameof(deleteItemCommand));
        }

        public async Task<DeleteItemServiceResult> RemoveItem(string sku, uint quantity)
        {

            InventoryItem? itemToReduce;
            
            try
            {
                itemToReduce = await _query.GetItem(sku);
            }
            catch
            {
                return InternalErrorOnRemovedItemResult();
            }

            if (itemToReduce is null)
            {
                return NotFoundItemToRemoveResult();
            }

            if (itemToReduce.Quantity < quantity)
            {
                return ConflictOnRemovedItemResult(itemToReduce.Quantity, quantity);
            }

            InventoryItem? deletedItem;

            try
            {
                deletedItem = await _deleteItemCommand.RemoveQuantity(itemToReduce.Sku, quantity);
            }
            catch
            {
                return InternalErrorOnRemovedItemResult();
            }

            return deletedItem is not null ? OkRemovedItemResult(deletedItem) : InternalErrorOnRemovedItemResult();
        }
    }
}