using Newtonsoft.Json;

namespace Trustesse_Assessment.ResponseHandler
{
    public class ResponseStatus
    {
        public int StatusCode { get; set; }
        public string? Status { get; set; }
        public string? Message { get; set; }
        public override string ToString() => JsonConvert.SerializeObject(this);
    }
}
