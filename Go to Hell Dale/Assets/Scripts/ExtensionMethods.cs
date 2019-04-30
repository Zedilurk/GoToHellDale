using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ExtensionMethods
{
    public static T Pop<T>(this List<T> list, int index)
    {
        T itemToPop = list[index];
        list.Remove(itemToPop);
        return itemToPop;
    }
}
