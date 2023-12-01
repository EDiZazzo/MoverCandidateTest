using System.Collections.Generic;

namespace MoverCandidateTest.Inventory.Model;

public record GetAllItemsServiceResult(bool IsSuccessfully, IEnumerable<InventoryItem> Inventory, int StatusCode, string? ErrorMessage);