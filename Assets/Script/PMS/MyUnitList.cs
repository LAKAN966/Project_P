using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


public class MyUnitList 
{
    
    public List<UnitStats> myList = new();

    public bool AddUnit (int id) // ���� �߰� �޼���. ������ ������ ���̵� Ȯ���ؼ� bool �� ��ȯ.
    {
        if (myList.Exists(unit => unit.ID == id)) return false;

        myList.Add(new UnitStats { ID = id });
        return true;
    }

    public bool HasUnit(int id)
    {
        return myList.Exists(unit => unit.ID == id); // ���������� üũ

    }


    public void SaveList() // ����? �־�� �ұ�?
    {

    }

    public static MyUnitList LoadList() // ����Ʈ �ҷ�����? �־�� �ҵ�? ������ ��� �ϴ����� ���� �߰� �� ��������.
    {
        return null;
    }
}
