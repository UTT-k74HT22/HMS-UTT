using HospitalManagement.dto.request.Batch;
using HospitalManagement.dto.response;
using HospitalManagement.repository;
using HospitalManagement.repository.impl;

namespace HospitalManagement.service.impl;

public class BatchServiceImpl : IBatchService
{   
       private readonly IBatchRepository _batchRepository;

        public BatchServiceImpl(IBatchRepository batchRepository)
        {
            _batchRepository = batchRepository;
        }

        public BatchServiceImpl()
        {
            _batchRepository = new BatchRepositoryImpl();
        }

        /// <summary>
        /// Lấy danh sách tất cả lô hàng
        /// </summary>
        public List<BatchResponse> GetAll()
        {
            return _batchRepository.FindAll();
        }

        /// <summary>
        /// Lấy danh sách lô hàng theo sản phẩm
        /// </summary>
        public List<BatchResponse> GetByProduct(long productId)
        {
            return _batchRepository.FindByProduct(productId);
        }

        /// <summary>
        /// Lấy chi tiết lô hàng
        /// </summary>
        public BatchResponse GetDetail(long batchId)
        {
            var batch = _batchRepository.FindDetail(batchId);
            if (batch == null)
            {
                throw new Exception($"Batch not found with id: {batchId}");
            }

            return batch;
        }

        /// <summary>
        /// Thêm mới lô hàng
        /// </summary>
        public long Create(CreateBatchRequest request)
        {
            return _batchRepository.Insert(request);
        }

        /// <summary>
        /// Cập nhật lô hàng
        /// </summary>
        public long Update(long batchId, UpdateBatchRequest updateBatchRequest)
        {
            _batchRepository.Update(batchId, updateBatchRequest);
            return batchId;
        }

        /// <summary>
        /// Ngưng sử dụng lô hàng
        /// </summary>
        public void Disable(long batchId)
        {
            _batchRepository.Disable(batchId);
        }
        
        public List<BatchResponse> FindByBatchCode(string keyword)
        {
            return _batchRepository.FindByBatchCode(keyword);
        }

    }
