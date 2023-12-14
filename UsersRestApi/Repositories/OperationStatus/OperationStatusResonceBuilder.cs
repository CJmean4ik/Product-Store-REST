using System.Text.Json;

namespace UsersRestApi.Repositories.OperationStatus
{
    public class OperationStatusResonceBuilder
    {
        public static OperationStatusResponse<T> CreateStatusAdding<T>(T body)
        {

            var _operationStatus = new OperationStatusResponse<T>();
            _operationStatus.OperationId = new Random().Next(0, 1000);
            _operationStatus.Status = StatusName.Created;
            _operationStatus.Title = "Entity created and added";
            _operationStatus.Body = body;
            return _operationStatus;
        }
        public static OperationStatusResponse<T> CreateStatusRemoving<T>(T body)
        {
            var _operationStatus = new OperationStatusResponse<T>();
            _operationStatus.OperationId = new Random().Next(0, 1000);
            _operationStatus.Status = StatusName.Deleted;
            _operationStatus.Title = "Entity has been deleted";
            _operationStatus.Body = body;
            return _operationStatus;
        }
        public static OperationStatusResponse<T> CreateStatusUpdating<T>(T body)
        {
            var _operationStatus = new OperationStatusResponse<T>();
            _operationStatus.OperationId = new Random().Next(0, 1000);
            _operationStatus.Status = StatusName.Updated;
            _operationStatus.Title = "Entity has been updating";
            _operationStatus.Body = body;
            return _operationStatus;
        }

        public static OperationStatusResponse<string> CreateStatusSuccessfully(string? message = default)
        {
            var _operationStatus = new OperationStatusResponse<string>();
            _operationStatus.OperationId = new Random().Next(0, 1000);
            _operationStatus.Status = StatusName.Successfully;
            _operationStatus.Title = "The operation was successful";
            _operationStatus.Message = message;
            return _operationStatus;
        }
        public static OperationStatusResponse<string> CreateStatusSendedMailCode(string? message = default)
        {
            var _operationStatus = new OperationStatusResponse<string>();
            _operationStatus.OperationId = new Random().Next(0, 1000);
            _operationStatus.Status = StatusName.SendedMailCode;
            _operationStatus.Title = "The code was sent to the mail";
            _operationStatus.Message = message;
            return _operationStatus;
        }
        public static OperationStatusResponse<string> CreateStatusCodeVerified(string? message = default)
        {
            var _operationStatus = new OperationStatusResponse<string>();
            _operationStatus.OperationId = new Random().Next(0, 1000);
            _operationStatus.Status = StatusName.CodeVerified;
            _operationStatus.Title = "The code has been successfully verified";
            _operationStatus.Message = message;
            return _operationStatus;
        }
        public static OperationStatusResponse<string> CreateStatusWrongCode(string? message = default)
        {
            var _operationStatus = new OperationStatusResponse<string>();
            _operationStatus.OperationId = new Random().Next(0, 1000);
            _operationStatus.Status = StatusName.WrongСode;
            _operationStatus.Title = "Incorrect code";
            _operationStatus.Message = message;
            return _operationStatus;
        }
        public static OperationStatusResponse<string> CreateStatusAuthorized(string? message = default)
        {
            var _operationStatus = new OperationStatusResponse<string>();
            _operationStatus.OperationId = new Random().Next(0, 1000);
            _operationStatus.Status = StatusName.Authorized;
            _operationStatus.Title = "Authorization was successful!";
            _operationStatus.Message = message;
            return _operationStatus;
        }
        public static OperationStatusResponse<string> CreateStatusWrongUsername(string? message = default)
        {
            var _operationStatus = new OperationStatusResponse<string>();
            _operationStatus.OperationId = new Random().Next(0, 1000);
            _operationStatus.Status = StatusName.WrongUsername;
            _operationStatus.Title = "The user under this name was not found";
            _operationStatus.Message = message;
            return _operationStatus;
        }
        public static OperationStatusResponse<string> CreateStatusWrongPassword(string? message = default)
        {
            var _operationStatus = new OperationStatusResponse<string>();
            _operationStatus.OperationId = new Random().Next(0, 1000);
            _operationStatus.Status = StatusName.WrongPassword;
            _operationStatus.Title = "The password for this user was entered incorrectly";
            _operationStatus.Message = message;
            return _operationStatus;
        }
        public static OperationStatusResponse<string> CreateStatusRegistered(string? message = default)
        {
            var _operationStatus = new OperationStatusResponse<string>();
            _operationStatus.OperationId = new Random().Next(0, 1000);
            _operationStatus.Status = StatusName.Registered;
            _operationStatus.Title = "The user has been registered";
            _operationStatus.Message = message;
            return _operationStatus;
        }
        public static OperationStatusResponse<string> CreateStatusUserExist(string? message = default)
        {
            var _operationStatus = new OperationStatusResponse<string>();
            _operationStatus.OperationId = new Random().Next(0, 1000);
            _operationStatus.Status = StatusName.UserExist;
            _operationStatus.Title = "A user with this name already exists";
            _operationStatus.Message = message;
            return _operationStatus;
        }
        public static OperationStatusResponse<string> CreateStatusError(Exception ex = null, string message = "")
        {
            var _operationStatus = new OperationStatusResponse<string>();
            _operationStatus.OperationId = new Random().Next(0, 1000);
            _operationStatus.Status = StatusName.Error;
            _operationStatus.Title = "An error occurred on the server when processing the request";
            _operationStatus.Message = message == "" ? ex.Message : message;
            return _operationStatus;
        }
        public static OperationStatusResponse<string> CreateStatusWarning(string message)
        {
            var _operationStatus = new OperationStatusResponse<string>();
            _operationStatus.OperationId = new Random().Next(0, 1000);
            _operationStatus.Status = StatusName.Warning;
            _operationStatus.Title = "Warning when executing";
            _operationStatus.Message = message;
            return _operationStatus;
        }

        public static OperationStatusResponse<T> CreateCustomStatus<T>(string title, StatusName statusName, T? message)
        {
            var _operationStatus = new OperationStatusResponse<T>();
            _operationStatus.OperationId = new Random().Next(0, 1000);
            _operationStatus.Status = statusName;
            _operationStatus.Title = title;
            _operationStatus.Body = message;
            return _operationStatus;
        }

    }
}
