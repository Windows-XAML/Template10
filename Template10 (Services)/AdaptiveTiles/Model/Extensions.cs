namespace Template10.Services.AdaptiveTiles.Model
{
    public static class Extensions
    {
        public static string FirstLetterToLower(this string str)
        {
            if (str == null)
                return null;

            if (str.Length > 1)
                return char.ToLower(str[0]) + str.Substring(1);

            return str.ToLower();
        }
    }
}