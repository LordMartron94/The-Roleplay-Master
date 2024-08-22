namespace MD.Common;

/// <summary>
/// Defines a converter that converts objects of type T to objects of type T1.
/// </summary>
/// <typeparam name="T">The type of objects to convert from.</typeparam>
/// <typeparam name="T1">The type of objects to convert to.</typeparam>
public interface IConverter<in T, out T1>
{
    /// <summary>
    /// Converts an object of type T to an object of type T1.
    /// </summary>
    /// <param name="toConvert">The object to convert.</param>
    /// <returns>A new object of type T1 that is the result of the conversion.</returns>
    T1 Convert(T toConvert);
}