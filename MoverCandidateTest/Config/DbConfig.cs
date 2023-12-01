using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MoverCandidateTest.Inventory.EntityFramework;

namespace MoverCandidateTest.Config;

public static class DbConfig
{
    public static void DatabaseConfig(this IServiceCollection services)
    {
        services.AddDbContext<EfInventoryItemContext>(options =>
        {
            options.UseInMemoryDatabase("Data Source=InMemorySample;Mode=Memory;Cache=Shared");
        });
    }
}