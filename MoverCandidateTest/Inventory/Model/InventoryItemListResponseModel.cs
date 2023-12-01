using System.Collections.Generic;

namespace MoverCandidateTest.Inventory.Model;

public record InventoryItemListResponseModel(IEnumerable<InventoryItemResponseModel> InventoryItemList);
