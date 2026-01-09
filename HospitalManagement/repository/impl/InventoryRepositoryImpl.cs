using HospitalManagement.dto.request;
using HospitalManagement.dto.response;

namespace HospitalManagement.repository.impl;

public class InventoryRepositoryImpl : IInventoryRepository
{
    public List<InventoryResponse> GetAll()
    {
        throw new NotImplementedException();
    }

    public List<InventoryResponse> GetByWarehouse(long warehouseId)
    {
        throw new NotImplementedException();
    }

    public List<InventoryResponse> GetByProduct(long productId)
    {
        throw new NotImplementedException();
    }

    public InventoryResponse FindByProductAndWarehouse(long productId, long warehouseId, long? batchId)
    {
        throw new NotImplementedException();
    }

    public List<InventoryResponse> GetLowStockItems()
    {
        throw new NotImplementedException();
    }

    public List<InventoryResponse> GetNearExpiryItems()
    {
        throw new NotImplementedException();
    }

    public void UpdateThresholds(long inventoryItemId, UpdateInventoryThresholdRequest request)
    {
        throw new NotImplementedException();
    }

    public int GetTotalQuantityByProduct(long productId)
    {
        throw new NotImplementedException();
    }

    public bool HasStock(long productId, long warehouseId, int requiredQuantity)
    {
        throw new NotImplementedException();
    }

    public InventoryItemInfo GetOrCreateInventoryItem(long productId, long batchId, long warehouseId)
    {
        throw new NotImplementedException();
    }

    public void UpdateQuantity(long inventoryItemId, int newQuantity)
    {
        throw new NotImplementedException();
    }

    public int GetCurrentQuantity(long productId, long batchId, long warehouseId)
    {
        throw new NotImplementedException();
    }

    public void UpdateStock(long productId, long batchId, long warehouseId, int newQuantity)
    {
        throw new NotImplementedException();
    }

    public void InsertStockMovement(long productId, long batchId, long warehouseId, int quantity, int before, int after,
        long userId, string note, string movementType)
    {
        throw new NotImplementedException();
    }
}