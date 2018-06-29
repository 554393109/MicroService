using System;

namespace Utility.Extension
{
    public static class IntExtension
    {
        public static string Join(this int[] array, string separator)
        {
            if (array == null)
                throw new ArgumentNullException("array");

            if (array.Length > 0)
            {
                string[] strArray = new string[array.Length];
                for (int i = 0; i < array.Length; i++)
                    strArray[i] = array[i].ToString();

                return string.Join(separator, strArray);
            }

            return string.Empty;
        }

    }
}
