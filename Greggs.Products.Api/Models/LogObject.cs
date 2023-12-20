namespace Greggs.Products.Api.Models
{
    public class LogObject
    {
        public string HttpUri { get; set; }
        public string Method { get; set; }
        public string Path { get; set; }
        public string RequestHeaders { get; set; }
        public string RequestParams { get; set; }
        public object RequestBody { get; set; }
        public string StatusCode { get; set; }
        public string ResponseHeaders { get; set; }
        public string ResponseBody { get; set; }
        public string Duration { get; set; }
    }
}
