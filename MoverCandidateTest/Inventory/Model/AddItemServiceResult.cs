namespace MoverCandidateTest.Inventory.Model;

public record AddItemServiceResult(bool IsSuccessfully, InventoryItem? Item, int StatusCode, string? ErrorMessage);