using System.Collections.Generic;
using UnityEngine;

public static class CustomUtility
{
    public static float RoundToTwoDecimalPlaces( float value )
    {
        return Mathf.RoundToInt(value * 100) / 100f;
    }
    
    public static float DecimalToPercentage(float decimalValue)
    { 
        return decimalValue * 100;
    }

    public static List<T> ShuffleList<T>(List<T> list)
    {
        int random1, random2;
        T temp;

        for (int i = 0; i < list.Count; ++i)
        {
            random1 = UnityEngine.Random.Range(0, list.Count);
            random2 = UnityEngine.Random.Range(0, list.Count);

            temp = list[random1];
            list[random1] = list[random2];
            list[random2] = temp;
        }

        return list;
    }

}
