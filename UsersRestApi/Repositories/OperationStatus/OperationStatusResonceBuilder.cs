using System.Text.Json;

namespace UsersRestApi.Repositories.OperationStatus
{
    public class OperationStatusResonceBuilder
    {
        public static OperationStatusResponse CreateSuccessfulStatusAdding()
        {
            var _operationStatus = new OperationStatusResponse();
            _operationStatus.OperationId = new Random().Next(0, 1000);
            _operationStatus.Status = StatusName.Created;
            _operationStatus.Title = "Entity created and added";
            return _operationStatus;
        }
        public static OperationStatusResponse CreateSuccessfulStatusAdding(Object obj)
        {
            var middleResult = CreateSuccessfulStatusAdding();
            middleResult.JsonBody = JsonSerializer.SerializeToNode(obj);
            return middleResult;
        }
        public static OperationStatusResponse CreateSuccessfulStatusRemoving()
        {
            var _operationStatus = new OperationStatusResponse();
            _operationStatus.OperationId = new Random().Next(0, 1000);
            _operationStatus.Status = StatusName.Deleted;
            _operationStatus.Title = "Entity has been deleted";
            return _operationStatus;
        }
        public static OperationStatusResponse CreateSuccessfulStatusUpdating()
        {
            var _operationStatus = new OperationStatusResponse();
            _operationStatus.OperationId = new Random().Next(0, 1000);
            _operationStatus.Status = StatusName.Updated;
            _operationStatus.Title = "Entity has been updating";
            return _operationStatus;
        }
        public static OperationStatusResponse CreateCustomStatus(string message, StatusName statusName)
        {
            var _operationStatus = new OperationStatusResponse();
            _operationStatus.OperationId = new Random().Next(0, 1000);
            _operationStatus.Status = statusName;
            _operationStatus.Title = message;
            return _operationStatus;
        }
    }
}
