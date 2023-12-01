namespace UsersRestApi.Repositories.OperationStatus
{
    public  class OperationStatusResponse<T> : OperationStatusResponseBase
    {
        public T? Body { get; set; }
    }


}
