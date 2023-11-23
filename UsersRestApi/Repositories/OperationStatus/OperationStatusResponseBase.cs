namespace UsersRestApi.Repositories.OperationStatus
{
    public abstract class OperationStatusResponseBase
    {
        public int OperationId { get; set; }
        public StatusName Status { get; set; }
        public string? Title { get; set; }
    }
    public enum StatusName
    {
        Created = 100,
        Deleted = 200,
        Updated = 300,
        Successfully = 400,
        SendedMailCode = 500,
        Error = 001,
        Warning = 002
    }
}
