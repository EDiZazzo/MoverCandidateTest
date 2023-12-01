using System.Threading.Tasks;
using MoverCandidateTest.Inventory.Model;

namespace MoverCandidateTest.Inventory.Service;

public interface IGetAllItemsService
{
    Task<GetAllItemsServiceResult> GetAllInventoryItems();
}