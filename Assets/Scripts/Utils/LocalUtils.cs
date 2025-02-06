using System.Collections.Generic;

public static class LocalUtils
{
    public static List<T> Shuffle<T>(List<T> list)
    {
        var rng = new System.Random(); 
        var n = list.Count;
        while (n > 1)
        {
            n--;
            var k = rng.Next(n + 1);
            (list[k], list[n]) = (list[n], list[k]);
        }
            
        return list;
    }
}