using System.Collections.Generic;
using System.Linq;
using MoverCandidateTest.Inventory.Model;

namespace MoverCandidateTest.Inventory.Extension;

public static class Extension
{
    public static InventoryItemResponseModel ToInventoryItemResponseModel(this InventoryItem item) =>
        new(item.Sku, item.Description, item.Quantity);

    public static InventoryItemListResponseModel ToInventoryItemListResponseModel(
        this IEnumerable<InventoryItem> itemList) =>
            new(itemList.Select(item => item.ToInventoryItemResponseModel()));
}