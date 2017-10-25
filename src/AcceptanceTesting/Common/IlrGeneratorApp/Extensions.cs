namespace IlrGeneratorApp
{
    public static class Extensions
    {
        public static bool IsNumeric(this string value)
        {
            long x;
            return long.TryParse(value, out x);
        }
    }
}
