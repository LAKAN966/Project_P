using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
public class PickUpCalculator
{
    public static bool IsLeader()
    {
        float randomValue = UnityEngine.Random.value;     //리더유닛10, 일반 90 //0~1.
        return randomValue <= 0.1f;
    }
    //리더인지 아닌지
    //리더일 경우 리스트에서 하나 뽑는다.
    //리더가 아닐경우 일반에서 모집.
}