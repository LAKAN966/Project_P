using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test
{
    private static Test instance;

    public static Test Instance 
    {
        get
        {
            if (instance == null)
            {
                instance = new Test();
            }
            return instance;
        }
    }

    public void EditFunctionSetUnit()
    {

        MyUnitList.Instance.AddUnit(1001);
        MyUnitList.Instance.AddUnit(1002);
        MyUnitList.Instance.AddUnit(2001);
        MyUnitList.Instance.AddUnit(2002);
        MyUnitList.Instance.AddUnit(3001);

        var myList = MyUnitList.Instance.GetAllUnit();
        
        if(myList == null)
        {
            Debug.Log($"{myList} is null");
        }
        
        foreach (var unit in myList)
        {
            Debug.Log($"{unit.Name} {unit.ID} {unit.IsHero} {unit.Damage}");
        }
    }

    public void EditFunctionSetDeck()
    {
        DeckManager.Instance.TryAddUnitToDeck(1001);
        DeckManager.Instance.TryAddUnitToDeck(1002);
        DeckManager.Instance.TryAddUnitToDeck(2001);
        DeckManager.Instance.TryAddUnitToDeck(2002);
        DeckManager.Instance.TryAddUnitToDeck(3001);

    }

    public void EditFuctionDeckData()
    {
        var deckList = DeckManager.Instance.GetAllDataInDeck();
        var leader = DeckManager.Instance.GetLeaderDataInDeck();


        foreach(var deck in deckList)
        {
            if (deckList == null)
                return;

            Debug.Log($"{deck.Name} {deck.ID} {deck.IsHero}");
        }

        if(leader == null)
        {
            return;
        }
        Debug.Log($"{leader.Name} {leader.ID} {leader.IsHero}");
    }

    public void SetDeckBuild()
    {
        UIDeckBuildManager.instance.Init();
    }


}
