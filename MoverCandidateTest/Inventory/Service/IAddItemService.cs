using System.Threading.Tasks;
using MoverCandidateTest.Inventory.Model;

namespace MoverCandidateTest.Controllers.Inventory.Service;

public interface IAddItemService
{
    Task<AddItemServiceResult> AddItem(InventoryItem item);
}