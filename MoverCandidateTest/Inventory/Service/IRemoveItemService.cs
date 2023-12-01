using System.Threading.Tasks;
using MoverCandidateTest.Inventory.Model;

namespace MoverCandidateTest.Inventory.Service;

public interface IRemoveItemService
{
    Task<DeleteItemServiceResult> RemoveItem(string sku, uint quantity);
}