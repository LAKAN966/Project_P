using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Random = UnityEngine.Random;
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
    //PlayerDataManager.AddUnit에 데이터 패싱 필요.


    [Serializable]
    public class Merchandise<T>
    {
        public T item;
        public float value;
    }

    public class PickUpCalculator2
    {

        // 확률에 따라 랜덤으로 선택하는 함수
        public static T GetRandomEnum<T>(List<Merchandise<T>> probabilities)
        {
            float total = 0f;
            foreach (var entry in probabilities)
            {
                total += entry.value; // 전체 확률 합계
            }

            float randomValue = UnityEngine.Random.Range(0f, total); // 0과 total 사이의 랜덤 값 생성
            float cumulative = 0f;

            foreach (var entry in probabilities)
            {
                cumulative += entry.value; // 확률을 누적
                if (randomValue < cumulative)
                {
                    return entry.item; // 랜덤 값이 누적 확률을 넘지 않으면 해당 값을 선택
                }
            }

            return probabilities[0].item; // 기본값 (예외처리로 사용 가능)
        }
    }
}