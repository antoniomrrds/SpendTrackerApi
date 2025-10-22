namespace WebApi.Responses.Base;

public abstract class ApiResponseBase
{
    public bool IsSuccess { get; set; }
    public int StatusCode { get; set; }
}