namespace Dima.Core.Requests.Orders
{
    public class ConfirmOrderRequest : BaseRequest
    {
        public string Number { get; set; } = string.Empty;
    }
}
