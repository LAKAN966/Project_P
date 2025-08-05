using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class GotchaPanel
{
    public GameObject backGround;
    public Button selectBtn;
}
public class GotchaInit : MonoBehaviour
{
    public int state = -1;
    public List<GotchaPanel> BGList;
    public Toggle percent;
    public Image ticketImg;
    public TextMeshProUGUI ticketCount;
    public void Awake()
    {
        for (int i = 0; i < BGList.Count; i++)
        {
            int capturedIndex = i;
            BGList[i].selectBtn.onClick.AddListener(() => OnSelectBtn(capturedIndex));
        }
    }
    public void OnEnable()
    {
        selectBtn(0);
    }
    public void selectBtn(int select)
    {
        percent.isOn = false;
        state = select-1;
        for (int i = 0; i < BGList.Count; i++)
        {
            if (i == select)
            {
                BGList[i].backGround.SetActive(true);
                ColorBlock cb = BGList[i].selectBtn.colors;
                cb.normalColor = Color.white;
                BGList[i].selectBtn.colors = cb;
            }
            else
            {
                BGList[i].backGround.SetActive(false);
                ColorBlock cb = BGList[i].selectBtn.colors;
                cb.normalColor = new Color { r = 255f, g = 255f, b = 255f, a = 0.5f };
                BGList[i].selectBtn.colors = cb;
            }
        }
    }
    private void OnSelectBtn(int id)
    {
        selectBtn(id);
    }
}
