using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FormatString {

    public static string FormatNumber(int num)
    {
        int i = (int)Mathf.Pow(10, (int)Mathf.Max(0, Mathf.Log10(num) - 2));
        num = num / i * i;

        if (num >= 1000000000)
            return (num / 1000000000D).ToString("0.##") + "B";
        if (num >= 1000000)
            return (num / 1000000D).ToString("0.##") + "M";
        if (num >= 1000)
            return (num / 1000D).ToString("0.##") + "K";

        return num.ToString("#,0");
    }
}
