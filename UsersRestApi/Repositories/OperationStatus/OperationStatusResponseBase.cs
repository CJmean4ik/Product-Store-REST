namespace UsersRestApi.Repositories.OperationStatus
{
    public abstract class OperationStatusResponseBase
    {
        public int OperationId { get; set; }
        public StatusName Status { get; set; }
        public string? Title { get; set; }
        public string? Message { get; set; }
    }

    public enum StatusName
    {
        Created = 1000,
        Deleted = 2000,
        Updated = 3000,
        Successfully = 4000,
        SendedMailCode = 5000,
        CodeVerified = 5001,
        WrongСode = 5002,
        Authorized = 6000,
        WrongUsername = 6001,
        WrongPassword = 6002,
        Registered = 7000,
        UserExist = 7001,
        Error = 4400,
        Warning = 4550,

    }
}
