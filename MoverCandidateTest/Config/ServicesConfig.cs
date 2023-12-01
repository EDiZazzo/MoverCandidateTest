using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using MoverCandidateTest.Controllers.Inventory.Service;
using MoverCandidateTest.Inventory.Command;
using MoverCandidateTest.Inventory.Model;
using MoverCandidateTest.Inventory.Query;
using MoverCandidateTest.Inventory.Service;
using MoverCandidateTest.Inventory.Validator;
using MoverCandidateTest.WatchHands.Model;
using MoverCandidateTest.WatchHands.Service;
using MoverCandidateTest.WatchHands.Validator;

namespace MoverCandidateTest.Config;

public static class ServicesConfig
{
    public static void DependencyInjectionConfig(this IServiceCollection services)
    {
        services.AddScoped<ICalculateLeastAngleService, CalculateLeastAngleService>();
        
        services.AddScoped<ICreateItemCommand, CreateItemCommand>();
        services.AddScoped<IUpdateItemCommand, UpdateItemCommand>();
        services.AddScoped<IDeleteItemCommand, DeleteItemCommand>();

        services.AddScoped<IGetInventoryItemQuery, GetInventoryItemQuery>();
        services.AddScoped<IGetAllInventoryItemsQuery, GetAllInventoryItemsQuery>();
        
        services.AddScoped<IAddItemService, AddItemService>();
        services.AddScoped<IRemoveItemService, RemoveItemService>();
        services.AddScoped<IGetAllItemsService, GetAllItemsService>();
        
        services.AddScoped<IValidator<CalculateLeastAngleRequestModel>, CalculateLeastAngleRequestModelValidator>();
        services.AddScoped<IValidator<AddInventoryItemRequestModel>, AddInventoryItemRequestModelValidator>();
        services.AddScoped<IValidator<ValidationRemoveItemQuantityModel>, RemoveInventoryItemQuantityRequestModelValidator>();
    }
}