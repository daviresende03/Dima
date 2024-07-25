using Dima.Core.Enums;
using System.ComponentModel.DataAnnotations;

namespace Dima.Core.Requests.Transactions
{
    public class CreateTransactionRequest : BaseRequest
    {
        [Required(ErrorMessage = "Invalid title")]
        public string Title { get; set; } = string.Empty;

        [Required(ErrorMessage = "Invalid type")]
        public ETransactionType Type { get; set; } = ETransactionType.Withdrawal;

        [Required(ErrorMessage = "Invalid amount")]
        public decimal Amount { get; set; }

        [Required(ErrorMessage = "Invalid id of category")]
        public long CategoryId { get; set; }

        [Required(ErrorMessage = "Invalid date")]
        public DateTime? PaidOrReceivedAt { get; set; }
    }
}
