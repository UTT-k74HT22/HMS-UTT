using HospitalManagement.dto.request;
using HospitalManagement.dto.response;
using HospitalManagement.entity;

namespace HospitalManagement.repository.impl;

public class StockMovementRepositoryImpl : IStockMovementRepository
{
    public long Create(CreateStockMovementRequest request)
    {
        throw new NotImplementedException();
    }

    public List<StockMovementResponse> GetAll()
    {
        throw new NotImplementedException();
    }

    public List<StockMovementResponse> GetByWarehouse(long warehouseId)
    {
        throw new NotImplementedException();
    }

    public List<StockMovementResponse> GetByProduct(long productId)
    {
        throw new NotImplementedException();
    }

    public List<StockMovementResponse> GetByMovementType(StockMovementType movementType)
    {
        throw new NotImplementedException();
    }

    public List<StockMovementResponse> GetByDateRange(DateTime fromDate, DateTime toDate)
    {
        throw new NotImplementedException();
    }

    public StockMovementResponse FindById(long id)
    {
        throw new NotImplementedException();
    }

    public List<StockMovementResponse> GetHistoryByProductAndWarehouse(long productId, long warehouseId)
    {
        throw new NotImplementedException();
    }

    public long InsertWithQuantityTracking(CreateStockMovementRequest request)
    {
        throw new NotImplementedException();
    }
}