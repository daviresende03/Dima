namespace Dima.Core.Requests.Stripe
{
    public class CreateSessionRequest : BaseRequest
    {
        public string OrderNumber { get; set; } = string.Empty;
    }
}
