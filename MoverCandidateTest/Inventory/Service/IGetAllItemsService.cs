using System.Collections.Generic;
using System.Threading.Tasks;
using MoverCandidateTest.Inventory.Model;

namespace MoverCandidateTest.Controllers.Inventory.Service;

public interface IGetAllItemsService
{
    Task<GetAllItemsServiceResult> GetAllInventoryItems();
}