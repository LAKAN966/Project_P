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



    public List<Card> deck = new List<Card>();
    public int Total = 0;
    public List<Card> CardResult = new List<Card>();

    public void ResultSelect()
    {
        CardResult.Add(RandomCard(Total));
    }
    public Card RandomCard(int weight)
    {
        int SelectNum = 0;
        SelectNum = Mathf.RoundToInt(Total * Random.Range(0.0f, 1.0f));

        for (int i = 0; i < deck.Count; i++)
        {
            weight += deck[i].Weight;
            if (SelectNum <= weight)
            {
                Card temp = new Card(deck[i]);
                return temp;
            }
        }
        return null;
    }

    void start()
    {
        for (int i = 0; i < deck.Count; i++)
        {
            Total += deck[i].Weight;
        }

    }
}

public enum CardGrade { Leader, }
[System.Serializable]
public class Card
{
    public string CaedName;
    public string CardImage;
    public CardGrade cardGrade;
    public int Weight;

    public Card(Card card)
    {
        this.CaedName = card.CaedName;
        this.CardImage = card.CardImage;
        this.cardGrade = card.cardGrade;
        this.Weight = card.Weight;
    }
}