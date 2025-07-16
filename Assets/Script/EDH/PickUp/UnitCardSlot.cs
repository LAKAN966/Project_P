using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UnitCardSlot : MonoBehaviour
{
    [SerializeField] private TMP_Text UnitICardNametext;        // 유닛 이름

    [SerializeField] public Image UnitIcon;                     // 유닛 아이콘

    public void init(PickInfo Alliance) //4
    {
        UnitICardNametext.text = Alliance.Name;

        var stats = UnitDataManager.Instance.GetStats(Alliance.ID);

        Debug.Log(stats.ModelName);

        UnitIcon.sprite = Resources.Load<Sprite>($"SPUMImg/{stats.ModelName}"); //5
    }
}