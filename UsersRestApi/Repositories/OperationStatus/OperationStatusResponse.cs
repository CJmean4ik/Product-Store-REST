using System.Text.Json.Nodes;

namespace UsersRestApi.Repositories.OperationStatus
{
    public  class OperationStatusResponse : OperationStatusResponseBase
    {
        public JsonNode? JsonBody { get; set; }
    }
}
