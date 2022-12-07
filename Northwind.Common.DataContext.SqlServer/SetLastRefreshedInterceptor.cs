using Microsoft.EntityFrameworkCore.Diagnostics;
using Northwind.Common.EntityModels.SqlServer;

namespace Northwind.Shared;
public class SetLastRefreshedInterceptor : IMaterializationInterceptor
{
    public object InitializedInstance(MaterializationInterceptionData materializationData, object entity)
    {
        if(entity is IHasLastRefreshed entityWithLastRefreshed)
        {
            entityWithLastRefreshed.LastRefreshed = DateTimeOffset.UtcNow;
        }
        return entity;
    }
}

