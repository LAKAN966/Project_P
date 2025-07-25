using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static System.Net.Mime.MediaTypeNames;

public class GospelConfirmUI : MonoBehaviour
{
    public TMP_Text nameText;
    public TMP_Text costText;
    public TMP_Text descText;
    public Button confirmBtn;
    public Button cancleBtn;
    public void Show(GospelData data)
    {
        nameText.text = data.name;
        costText.text = $"{NumberFormatter.FormatNumber(PlayerDataManager.Instance.player.tribute)}/{NumberFormatter.FormatNumber(data.cost)}";
        descText.text = data.description;
    }
}
