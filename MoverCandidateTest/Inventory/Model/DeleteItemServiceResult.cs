namespace MoverCandidateTest.Inventory.Model;

public record DeleteItemServiceResult(bool IsSuccessfully, InventoryItem? Item, int StatusCode, string? ErrorMessage);
