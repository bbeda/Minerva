namespace Minerva.Application.Common;
public static class EnumExtensions
{
    public static IEnumerable<T> Flags<T>(this T value) where T : struct, Enum => Enum.GetValues<T>().Where(v => value.HasFlag(v));
}
