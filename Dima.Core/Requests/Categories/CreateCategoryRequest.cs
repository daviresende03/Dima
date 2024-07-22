using System.ComponentModel.DataAnnotations;

namespace Dima.Core.Requests.Categories
{
    public class CreateCategoryRequest : BaseRequest
    {
        [Required(ErrorMessage = "Invalid title")]
        public string Title { get; set; } = string.Empty;

        [Required(ErrorMessage = "Invalid description")]
        public string Description { get; set; } = string.Empty;
    }
}
