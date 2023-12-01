using System.Threading.Tasks;
using MoverCandidateTest.Inventory.Model;

namespace MoverCandidateTest.Inventory.Command;

public interface IUpdateItemCommand
{
    Task<InventoryItem?> AddQuantity(InventoryItem item);
}