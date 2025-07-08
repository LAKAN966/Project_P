using UnityEngine;

[System.Serializable]
public class PickInfo
{
    public int    ID;                   //유닛 ID
    public string Name;                 //유닛 이름
    public string Description;          //유닛 설명
    public bool   IsHero;               //유닛 타입
    public Sprite Uniticon;             //유닛 아이콘
}
