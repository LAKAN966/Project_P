[System.Serializable]
public class UnitStats
{
    public int ID;
    public string Name;
    public bool IsHero;
    public bool IsAOE;//범위공격
    public float AttackRange;//사거리
    public float Damage;//데미지
    public float MaxHP;//최대체력
    public float MoveSpeed;//이동속도
    public float SpawnInterval;//스폰 쿨타임
    public int Cost;//소환 코스트/보상 코스트
    public int Hitback;//넉백 체력의 n-1/n ~ 1/n일때 넉백
    public string species;//시너지용 종족명
}
