using System.Threading.Tasks;
using MoverCandidateTest.Controllers.Inventory.Service;
using MoverCandidateTest.Inventory.Model;
using MoverCandidateTest.Inventory.Query;
using static MoverCandidateTest.Inventory.Utility.ResultUtility;

namespace MoverCandidateTest.Inventory.Service;

public class GetAllItemsService: IGetAllItemsService
{
    private readonly IGetAllInventoryItemsQuery _query;

    public GetAllItemsService(IGetAllInventoryItemsQuery query)
    {
        _query = query;
    }

    public async Task<GetAllItemsServiceResult> GetAllInventoryItems()
    {
        try
        {
            return OkGetAllInventoryResult(await _query.GetAllItems());
        }
        catch
        {
            return InternalErrorOnGetAllInventoryResult();
        }
    }
}