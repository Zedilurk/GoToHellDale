using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ExtensionMethods
{
    private static object _PopLock = new object();
    public static T Pop<T>(this List<T> list, int index)
    {
        lock (_PopLock)
        {
            T itemToPop = list[index];
            list.Remove(itemToPop);
            return itemToPop;
        }        
    }
}
