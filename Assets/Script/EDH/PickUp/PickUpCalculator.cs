using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class PickUpCalculator
{
    public static bool IsLeader()
    {
        float randomValue = UnityEngine.Random.value;     //리더유닛10, 일반 90 //0~1.
        return randomValue <= 0.1f;
    }
}