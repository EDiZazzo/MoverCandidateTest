using System.Threading.Tasks;
using MoverCandidateTest.Inventory.Model;

namespace MoverCandidateTest.Inventory.Command;

public interface ICreateItemCommand
{
    Task<InventoryItem?> CreateItem(InventoryItem item);
}