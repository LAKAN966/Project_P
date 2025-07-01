using System.Security.Cryptography.X509Certificates;
using UnityEditor.SceneManagement;

public class StageData
{
    public int ID;                  
    public float BaseDistance; //기지간의 거리
    public int EnemyBaseHP; //적 기지 체력
    public int EnemyUnit1; 
    public int EnemyUnit2;
    public int EnemyUnit3; 
    public int DropGold; //클리어 보상 골드
    public int DropUnit; //클리어 보상 유닛
    public string StageName; //스테이지 이름
    public float TeaTime; //웨이브 시작 전 유예시간
    public float ResetTime; //웨이브 길이
    public int EnemyHeroID; //적 영웅 ID
    public string StageBG; //스테이지 배경 이름
    public int ActionPoint; //행동력
}