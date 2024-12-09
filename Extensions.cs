using System.Text;

namespace AdventOfCode2024
{
    public static class Extensions
    {
        public static string Replace(this string originalString, int index, char character)
        {
            StringBuilder stringBuilder = new(originalString);
            stringBuilder = stringBuilder.Remove(index, 1);
            stringBuilder = stringBuilder.Insert(index, character);
            return stringBuilder.ToString();
        }

        public static IList<T> Swap<T>(this IList<T> list, int indexA, int indexB)
        {
            T tmp = list[indexA];
            list[indexA] = list[indexB];
            list[indexB] = tmp;
            return list;
        }
    }
}
