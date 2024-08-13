using UnityEngine;

namespace IT.CoreLib.Extensions
{
    public static class ArrayExtensions 
    {
            public static T GetRandomItem<T>(this T[] array)
            {
                if ((array == null) || (array.Length < 1)) return default(T);
                return array[Random.Range(0, array.Length)];
            }
    }
}