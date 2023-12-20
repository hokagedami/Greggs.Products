namespace Greggs.Products.Api.Models
{
    public class ApiResponse<T>
    {
        public string Description { get; set; } = string.Empty;
        public bool HasErrors { get; set; } = false;
        public int Code { get; set; }
        public T Data { get; set; }
    }
}
