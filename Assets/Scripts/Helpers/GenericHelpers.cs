using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GenericHelpers
{
    public static bool FloatEqualsPrecision(float float1, float float2, float precision)
    {
        return float1 > float2 - precision && float1 < float2 + precision;
                
    }
}
