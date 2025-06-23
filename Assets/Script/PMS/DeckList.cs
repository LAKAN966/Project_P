using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class DeckList 
{
    public int myUnitID;
}
public class DeckData
{
    public List<DeckList> deckList = new(); // �� ����Ʈ �����. �Ϲ� ����.
    public DeckList leaderUnit; // ���� ����.

    public bool Contains(int myUnitID) // �� ����Ʈ���� ���̵� ������ Ȯ�� �Լ�.
    {
        return deckList.Any(unit => unit.myUnitID == myUnitID) || (leaderUnit != null && leaderUnit.myUnitID == myUnitID);
    }

    public bool AddNormalUnit(int myUnitID) // �Ϲ� �� ���� �Լ�.
    {
        if(deckList.Count >= 6 || Contains(myUnitID)) // �Ϲ� �� �����ο� �ִ� 6��.
        {
            return false;
        }

        deckList.Add(new DeckList { myUnitID = myUnitID });
        return true;
    }

    public bool SetLeaderUnit(int myUnitID) // ���� ���� ���� �Լ�.
    {
        if (Contains(myUnitID)) return false;

        UnitStats stat = DataManager.Instance.GetStats(myUnitID); // ���ݿ��� IsHero bool ������ Ȯ��.
        if (stat == null || !stat.IsHero)
        {
            Debug.LogWarning($"���� {myUnitID}�� ������ ������ �� �����ϴ� (IsHero = false)");
            return false;
        }

        leaderUnit = new DeckList { myUnitID = myUnitID };
        return true;
    }

    public void RemovUnit(int myUnitID) // �� ����Ʈ���� ���� ���� �Լ�.
    {
        deckList.RemoveAll(unit => unit.myUnitID == myUnitID);

        if(leaderUnit != null && leaderUnit.myUnitID == myUnitID)
        {
            leaderUnit = null;
        }
    }
}
