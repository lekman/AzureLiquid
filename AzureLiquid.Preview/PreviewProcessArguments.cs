namespace AzureLiquid.Preview;

/// <summary>
/// Handles the arguments passed to the preview process.
/// </summary>
public class PreviewProcessArguments
{
    /// <summary>
    /// The current path of the process.
    /// </summary>
    private readonly string _path;

    /// <summary>
    /// Initializes a new instance of the <see cref="PreviewProcessArguments" /> class.
    /// </summary>
    public PreviewProcessArguments() => _path = Directory.GetCurrentDirectory();

    /// <summary>
    /// Gets the index of the argument, if it exists.
    /// </summary>
    /// <param name="args">The arguments.</param>
    /// <param name="key">The key.</param>
    /// <returns>The index of the argument.</returns>
    internal static int GetArgumentIndex(string[] args, string key)
    {
        for (int i = 0; i < args.Length; i++)
        {
            if (IsArgMatch(args[i], key))
            {
                return i;
            }
        }

        return -1;
    }

    /// <summary>
    /// Determines whether the argument matches the partial argument key name.
    /// </summary>
    /// <param name="arg">The argument.</param>
    /// <param name="key">The key.</param>
    /// <returns>
    /// <c>true</c> if argument found; otherwise, <c>false</c>.
    /// </returns>
    internal static bool IsArgMatch(string arg, string key)
    {
        return string.CompareOrdinal(arg, "--" + key) == 0;
    }

    /// <summary>
    /// Gets a value indicating whether the specific argument key was found in the arguments.
    /// </summary>
    /// <param name="args">The passed arguments.</param>
    /// <param name="key">The key.</param>
    /// <returns><c>true</c> if the argument was found, otherwise <c>false</c>,</returns>
    public static bool HasArgument(string[] args, string key) => args.Any(arg => IsArgMatch(arg, key));

    /// <summary>
    /// Parses the argument value.
    /// </summary>
    /// <param name="args">The arguments.</param>
    /// <param name="key">The key.</param>
    /// <returns>The argument value.</returns>
    public string ParsePath(string[] args, string key)
    {
        var index = GetArgumentIndex(args, key);
        return
            index == -1 || index - 1 >= args.Length
                ? string.Empty // No match, or no arguments passed
                : Path.GetFullPath(args[index + 1], _path); // Argument found, parsing path
    }
}