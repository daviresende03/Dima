namespace Dima.Core.Requests
{
    public abstract class PagedRequest : BaseRequest
    {
        public int PageNumber { get; set; } = 2;
        public int PageSize { get; set; }
    }
}
