using System.Collections.Generic;
using System.Threading.Tasks;
using MoverCandidateTest.Inventory.Model;

namespace MoverCandidateTest.Inventory.Query;

public interface IGetAllInventoryItemsQuery
{
    Task<IEnumerable<InventoryItem>> GetAllItems();
}