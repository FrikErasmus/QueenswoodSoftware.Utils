namespace Library.Helpers;

/// <summary>
///
/// </summary>
public static class ResultHelper
{
    public static ResponseMessage Ok(object? payload = null, string? message = null, string? token = null)
    {
        return new ResponseMessage
        {
            Success = true,
            Object = payload,
            ShortDescription = message,
            Token = token
        };
    }

    public static ResponseMessage Fail(string shortDescription, string? fullDescription = null, object? payload = null)
    {
        return new ResponseMessage
        {
            Success = false,
            ShortDescription = shortDescription,
            FullDescription = fullDescription,
            Object = payload
        };
    }

    public static ResponseMessage FromException(Exception ex, string shortDescription = "Unexpected error occurred.", object? payload = null)
    {
        return new ResponseMessage
        {
            Success = false,
            ShortDescription = shortDescription,
            FullDescription = ex.Message,
            Object = payload
        };
    }
}