namespace MD.Common;

/// <summary>
/// Static extension class for miscellaneous helper functions.
/// </summary>
public static class Extensions
{
    /// <summary>
    /// Splits a string formatted as 'key:value' into a KeyValuePair.
    /// </summary>
    /// <param name="input">The string to parse, which must contain exactly one colon character.</param>
    /// <returns>A KeyValuePair constructed from the input string, where the key is the part of the string before the colon, and the value is the part of the string after the colon.</returns>
    /// <exception cref="ArgumentException">Thrown when the input string does not contain exactly one colon character.</exception>
    public static KeyValuePair<string, string> ExtractKeyValueFromColonSeparatedString(this string input)
    {
        return Utilities.ExtractKeyValueFromColonSeparatedString(input);
    }

    /// <summary>
    /// Replaces the element at the specified index in the list with the specified item.
    /// </summary>
    /// <typeparam name="T">The type of elements in the list.</typeparam>
    /// <param name="list">The list in which to replace the element.</param>
    /// <param name="index">The zero-based index of the element to replace.</param>
    /// <param name="item">The new value for the element at the specified index.</param>
    /// <returns>The modified list. Note that this method does not create a new list; it modifies the existing one.</returns>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when the index is less than 0 or greater than or equal to the list's Count.</exception>
    public static IList<T> Replace<T>(this IList<T> list, int index, T item)
    {
        list[index] = item;
        return list;
    }

    /// <summary>
    /// Finds and replaces all occurrences of a specified item in the list with another specified item.
    /// </summary>
    /// <typeparam name="T">The type of elements in the list.</typeparam>
    /// <param name="list">The list in which to find and replace items.</param>
    /// <param name="itemToReplace">The item to find in the list.</param>
    /// <param name="itemToReplaceWith">The item to replace the found item with.</param>
    /// <returns>The modified list. Note that this method does not create a new list; it modifies the existing one.</returns>
    public static IList<T> FindAndReplace<T>(this IList<T> list, T itemToReplace, T itemToReplaceWith)
    {
        for (int i = 0; i < list.Count; i++)
        {
            if (list[i].Equals(itemToReplace))
            {
                list[i] = itemToReplaceWith;
            }
        }
        return list;
    }
}