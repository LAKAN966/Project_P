using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.Progress;
public class CertiSlot : MonoBehaviour
{
    [SerializeField] public Image UnitIcon;                     // 유닛 아이콘
    [SerializeField] private Button certiSlot;                  // 증명서 유닛 슬롯
    [SerializeField] private TMP_Text CertiCost;
    [SerializeField] private CertiPurChaseSync certiPurChaseSync;

    private PickInfo _Info;

    public void init(PickInfo pickInfo)
    {
        var stats = UnitDataManager.Instance.GetStats(pickInfo.ID);
        //CertiCost.text = pickInfo.warrant.ToString();
        UnitIcon.sprite = Resources.Load<Sprite>($"SPUMImg/{stats.ModelName}");

        _Info = pickInfo;
        _Info.Name = pickInfo.Name;
        _Info.ID = pickInfo.ID;
        _Info.warrant = pickInfo.warrant;

        certiSlot.onClick.RemoveAllListeners();

        certiSlot.onClick.AddListener(() =>
        {
            Debug.Log("버튼 눌림");
            if(certiPurChaseSync == null)
            {
                certiPurChaseSync = FindObjectOfType<CertiPurChaseSync>();
            }
            CertiPurChaseSync.Instance.Init(pickInfo, this);

            UIController.Instance.PurchaseCertiUnitBox.SetActive(true);
            UIController.Instance.PurchaseCertiUnitBox.GetComponent<CertiPurchaseBoxSet>()._PickInfo = _Info;
        });
    }
}
