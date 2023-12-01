using System.Threading.Tasks;
using MoverCandidateTest.Inventory.Model;

namespace MoverCandidateTest.Inventory.Service;

public interface IAddItemService
{
    Task<AddItemServiceResult> AddItem(InventoryItem item);
}