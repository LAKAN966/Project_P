using TMPro;
using UnityEngine;

public class GospelConfirmUI : MonoBehaviour
{
    public TMP_Text nameText;
    public TMP_Text costText;
    public TMP_Text descText;

    public void Show(GospelData data)
    {
        nameText.text = data.name;
        costText.text = $"999/{data.cost}";
        descText.text = data.description;
    }
}
