using System.Threading.Tasks;
using MoverCandidateTest.Inventory.Model;

namespace MoverCandidateTest.Inventory.Query;

public interface IGetInventoryItemQuery
{
    Task<InventoryItem?> GetItem(string sku);
}