using System.Text.Json.Nodes;

namespace UsersRestApi.Repositories.OperationStatus
{
    public  class OperationStatusResponse<T> : OperationStatusResponseBase
    {
        public T? Body { get; set; }
    }


}
