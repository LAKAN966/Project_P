using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using Button = UnityEngine.UI.Button;

public class GottchaUI : MonoBehaviour
{
    [SerializeField]

    public Button SpritPickBtn;
    public Button UndeadPickBtn;
    public Button NormalPickBtn;

    public void Start()
    {
        SpritPickBtn.onClick.AddListener(Sprit);
        UndeadPickBtn.onClick.AddListener(Undead);
        NormalPickBtn.onClick.AddListener(Normal);
    }

    public void Sprit()
    {
        Button.ButtonClickedEvent buttonClicked = new Button.ButtonClickedEvent();
        



        SpritPickBtn.onClick.RemoveAllListeners();
    }

    public void Undead()
    {
        UndeadPickBtn.onClick.RemoveAllListeners();
    }
    public void Normal()
    {
        NormalPickBtn.onClick.RemoveAllListeners();
    }
}
