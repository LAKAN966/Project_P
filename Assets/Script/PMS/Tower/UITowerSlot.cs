using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UITowerSlot : MonoBehaviour
{
    [SerializeField] private Image towerImg;
    [SerializeField] private TextMeshProUGUI raceNameText;
    [SerializeField] private TextMeshProUGUI floorText;
    [SerializeField] private Button btn;

    private int raceID;

    public void Setup(int raceID)
    {
        this.raceID = raceID;

        var stage = TowerManager.Instance.GetCurrentFloorStage(raceID);
        if (stage == null)
        {
            Debug.LogWarning($"해당 종족({raceID})의 스테이지 데이터를 찾을 수 없습니다.");
            return;
        }

        //towerImg.sprite = 
        raceNameText.text = TagManager.GetNameByID(raceID);
        floorText.text = $"{stage.Chapter}층";

        btn.onClick.RemoveAllListeners();
        btn.onClick.AddListener(OnClickTower);
    }

    private void OnClickTower()
    {
        var stage = TowerManager.Instance.GetCurrentFloorStage(raceID);
        if(stage == null)
        {
            return;
        }

        UITowerManager.instance.SelectTower(stage.ID);

    }
}
