using System.Threading.Tasks;
using MoverCandidateTest.Inventory.Model;

namespace MoverCandidateTest.Inventory.Command;

public interface IDeleteItemCommand
{
    Task<InventoryItem?> RemoveQuantity(string sku, uint quantity);
}