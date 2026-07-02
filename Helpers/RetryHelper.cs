namespace QueenswoodSoftware.Utils.Helpers;

/// <summary>
/// Provides asynchronous retry execution with exponential backoff and optional jitter.
/// </summary>
public static class RetryHelper
{
    /// <summary>
    /// Executes an asynchronous operation and retries on failure until success or retries are exhausted.
    /// </summary>
    /// <typeparam name="T">The result type returned by the operation.</typeparam>
    /// <param name="operation">The asynchronous operation to execute.</param>
    /// <param name="maxRetries">Maximum number of retry attempts after the initial call.</param>
    /// <param name="baseDelayMs">Base delay in milliseconds used for exponential backoff between retries.</param>
    /// <param name="shouldRetry">Optional predicate to determine whether a caught exception is retryable; defaults to retrying all exceptions.</param>
    /// <param name="cancellationToken">Token used to cancel delays and the operation.</param>
    /// <returns>The result of the successful operation invocation.</returns>
    public static async Task<T> ExecuteAsync<T>(
        Func<Task<T>> operation,
        int maxRetries = 3,
        int baseDelayMs = 200,
        Func<Exception, bool>? shouldRetry = null,
        CancellationToken cancellationToken = default)
    {
        if (operation == null) throw new ArgumentNullException(nameof(operation));
        if (maxRetries < 0) throw new ArgumentOutOfRangeException(nameof(maxRetries));
        if (baseDelayMs < 0) throw new ArgumentOutOfRangeException(nameof(baseDelayMs));
        Exception? lastException = null;
        for (var attempt = 0; attempt <= maxRetries; attempt++)
        {
            cancellationToken.ThrowIfCancellationRequested();
            try
            {
                return await operation();
            }
            catch (Exception ex)
            {
                lastException = ex;
                var retryable = shouldRetry?.Invoke(ex) ?? true;
                if (!retryable || attempt == maxRetries)
                    break;
                var delay = GetDelayWithJitter(baseDelayMs, attempt);
                await Task.Delay(delay, cancellationToken);
            }
        }
        throw lastException ?? new InvalidOperationException("Retry failed without captured exception.");
    }

    /// <summary>
    /// Executes an asynchronous operation without a return value and retries on failure.
    /// </summary>
    /// <param name="operation">The asynchronous operation to execute.</param>
    /// <param name="maxRetries">Maximum number of retry attempts after the initial call.</param>
    /// <param name="baseDelayMs">Base delay in milliseconds used for exponential backoff between retries.</param>
    /// <param name="shouldRetry">Optional predicate to determine whether a caught exception is retryable; defaults to retrying all exceptions.</param>
    /// <param name="cancellationToken">Token used to cancel delays and the operation.</param>
    /// <returns>A task that completes when the operation succeeds or retries are exhausted.</returns>
    public static async Task ExecuteAsync(
        Func<Task> operation,
        int maxRetries = 3,
        int baseDelayMs = 200,
        Func<Exception, bool>? shouldRetry = null,
        CancellationToken cancellationToken = default)
    {
        if (operation == null) throw new ArgumentNullException(nameof(operation));
        await ExecuteAsync(async () =>
        {
            await operation();
            return true;
        }, maxRetries, baseDelayMs, shouldRetry, cancellationToken);
    }

    /// <summary>
    /// Calculates the delay before the next retry using exponential backoff plus random jitter.
    /// </summary>
    /// <param name="baseDelayMs">Base delay in milliseconds.</param>
    /// <param name="attempt">Zero-based attempt index used as the exponent for backoff.</param>
    /// <returns>Delay in milliseconds before the next retry.</returns>
    private static int GetDelayWithJitter(int baseDelayMs, int attempt)
    {
        var backoff = baseDelayMs * (int)Math.Pow(2, attempt);
        var jitter = Random.Shared.Next(0, 100);
        return backoff + jitter;
    }
}