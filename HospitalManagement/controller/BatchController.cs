using HospitalManagement.dto.request.Batch;
using HospitalManagement.dto.response;
using HospitalManagement.dto.response.Product;
using HospitalManagement.service;
using HospitalManagement.service.impl;
using HospitalManagement.Service.Impl;

namespace HospitalManagement.controller;

public class BatchController
{
    private readonly IBatchService _batchService;
    private readonly IProductService _productService;

    public BatchController(string connectionString)
    {
        _batchService = new BatchServiceImpl(connectionString);
        _productService = new ProductServiceImpl(connectionString);
    }
    public BatchController(
        IBatchService batchService,
        IProductService productService)
    {
        _batchService = batchService;
        _productService = productService;
    }

    // ===== Batch APIs =====

    /// <summary>
    /// Lấy toàn bộ lô hàng
    /// </summary>
    public List<BatchResponse> GetAll()
    {
        return _batchService.GetAll();
    }

    /// <summary>
    /// Lấy danh sách lô theo sản phẩm
    /// </summary>
    public List<BatchResponse> GetByProduct(long productId)
    {
        return _batchService.GetByProduct(productId);
    }

    /// <summary>
    /// Tạo mới lô hàng
    /// </summary>
    public long Create(CreateBatchRequest request)
    {
        return _batchService.Create(request);
    }

    /// <summary>
    /// Cập nhật lô hàng
    /// </summary>
    public void Update(long batchId, UpdateBatchRequest request)
    {
        _batchService.Update(batchId, request);
    }

    /// <summary>
    /// Ngưng sử dụng lô hàng (BLOCKED)
    /// </summary>
    public void Disable(long batchId)
    {
        _batchService.Disable(batchId);
    }

    // ===== Product APIs (for ComboBox) =====

    public List<ProductResponse> GetAllProducts()
    {
        return _productService.GetAll();
    }
    
    public List<BatchResponse> FindByBatchCode(string keyword)
    {
        return _batchService.FindByBatchCode(keyword);
    }

}