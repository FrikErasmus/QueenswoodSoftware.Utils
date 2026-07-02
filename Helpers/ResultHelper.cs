using QueenswoodSoftware.Utils.Models;

namespace QueenswoodSoftware.Utils.Helpers;

/// <summary>
/// Provides helper methods for creating standardized API response messages.
/// </summary>
public static class ResultHelper
{
    // --- Non-generic (no payload) ---

    public static ResponseMessage Ok(string? message = null, string? token = null) =>
        new() { Success = true, ShortDescription = message, Token = token };

    public static ResponseMessage Fail(string shortDescription, string? fullDescription = null) =>
        new() { Success = false, ShortDescription = shortDescription, FullDescription = fullDescription };

    public static ResponseMessage FromException(Exception ex, string shortDescription = "Unexpected error occurred.") =>
        new() { Success = false, ShortDescription = shortDescription, FullDescription = ex.Message };

    // --- Generic (typed payload) ---

    public static ResponseMessage<T> Ok<T>(T data, string? message = null, string? token = null) =>
        new() { Success = true, Data = data, ShortDescription = message, Token = token };

    public static ResponseMessage<T> Fail<T>(string shortDescription, string? fullDescription = null, T? data = default) =>
        new() { Success = false, ShortDescription = shortDescription, FullDescription = fullDescription, Data = data };

    public static ResponseMessage<T> FromException<T>(Exception ex, string shortDescription = "Unexpected error occurred.", T? data = default) =>
        new() { Success = false, ShortDescription = shortDescription, FullDescription = ex.Message, Data = data };
}