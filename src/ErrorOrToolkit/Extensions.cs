using System.Threading.Tasks;

namespace ErrorOr;

/// <summary>
/// The extensions class
/// </summary>
public static class Extensions
{
    /// <summary>
    /// Ises the success using the specified error or
    /// </summary>
    /// <typeparam name="T">The </typeparam>
    /// <param name="errorOr">The error or</param>
    /// <returns>The bool</returns>
    public static bool IsSuccess<T>(this ErrorOr<T> errorOr) => !errorOr.IsError;

    /// <summary>
    /// Values the or null using the specified error or
    /// </summary>
    /// <typeparam name="T">The </typeparam>
    /// <param name="errorOr">The error or</param>
    /// <returns>The</returns>
    public static T? ValueOrNull<T>(this ErrorOr<T> errorOr) where T : struct => errorOr.IsError ? null : errorOr.Value;

    /// <summary>
    /// Values the or default using the specified error or
    /// </summary>
    /// <typeparam name="T">The </typeparam>
    /// <param name="errorOr">The error or</param>
    /// <param name="defaultValue">The default value</param>
    /// <returns>The</returns>
    public static T? ValueOrDefault<T>(this ErrorOr<T> errorOr, T? defaultValue = default) => errorOr.IsError ? defaultValue : errorOr.Value;

    /// <summary>
    /// Values the or null using the specified error or
    /// </summary>
    /// <typeparam name="T">The </typeparam>
    /// <param name="errorOr">The error or</param>
    /// <returns>A task containing the</returns>
    public static async Task<T?> ValueOrNull<T>(this Task<ErrorOr<T>> errorOr) where T : struct => (await errorOr).ValueOrNull();

    /// <summary>
    /// Values the or default using the specified error or
    /// </summary>
    /// <typeparam name="T">The </typeparam>
    /// <param name="errorOr">The error or</param>
    /// <param name="defaultValue">The default value</param>
    /// <returns>A task containing the</returns>
    public static async Task<T?> ValueOrDefault<T>(this Task<ErrorOr<T>> errorOr, T? defaultValue = default) => (await errorOr).ValueOrDefault();
}
