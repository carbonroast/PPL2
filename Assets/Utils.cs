using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public static class RNG
{
    private static System.Random rand = new System.Random();

    public static Element RandomElement()
    {
        int elements_count = Enum.GetNames(typeof(Element)).Length-1;
        int element_num = rand.Next(elements_count);
        return (Element)element_num;
    }
}

public static class ElementalColorUtils
{
    public static Color SetColor(Element element)
    {
        Color color = element switch
        {
            Element.Red => Color.red,
            Element.Green => Color.green,
            Element.Blue => Color.blue,
            Element.White => Color.white,
            _ => Color.cyan
        };
        return color;
    }
}