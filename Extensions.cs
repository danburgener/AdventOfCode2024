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
    }
}
