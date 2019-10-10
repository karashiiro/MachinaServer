namespace MachinaWrapper.Common
{
    static class Util
    {
        /// <summary>
        /// Camelcases a string.
        /// </summary>
        public static void JSify(ref string input)
        {
            input = input.Substring(0, 1).ToLower() + input.Substring(1);
        }
    }
}
