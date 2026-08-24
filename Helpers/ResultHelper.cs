using QueenswoodSoftware.Utils.Models;

namespace QueenswoodSoftware.Utils.Helpers;

/// <summary>
/// Provides helper methods for creating standardized API response messages.
/// </summary>
public static class ResultHelper
{
    public static ResponseMessage Ok(string? message = null, string? token = null) =>
        new() { Success = true, ShortDescription = message, Token = token };

    public static ResponseMessage Ok(object data, string? message = null, string? token = null) =>
        new() { Success = true, Object = data, ShortDescription = message, Token = token };

    public static ResponseMessage Fail(string shortDescription, string? fullDescription = null, object? data = null) =>
        new() { Success = false, ShortDescription = shortDescription, FullDescription = fullDescription, Object = data };

    public static ResponseMessage FromException(Exception ex, string shortDescription = "Unexpected error occurred.", object? data = null) =>
        new() { Success = false, ShortDescription = shortDescription, FullDescription = ex.Message, Object = data };
}
