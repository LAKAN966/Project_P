using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Button = UnityEngine.UI.Button;


public class StageUI : UIBase
{
    private UILoader uILoader;
    private UIManager uiManager;

    public Button CloseButton;

    public void Start()
    {
        //프리펩으로 만들어서  직접 찾아와야함.  무조건 코드로 가져와야함.
        uiManager = FindObjectOfType<UIManager>();
        CloseButton.onClick.AddListener(() => UIManager.Instance.Close<StageUI>());
        // 나머지 창은 비활성화 메인제외
        CloseButton.onClick.AddListener(() => UIManager.Instance.Close<HQUI>());
        CloseButton.onClick.AddListener(() => UIManager.Instance.Close<GottaUI>());
        CloseButton.onClick.AddListener(() => UIManager.Instance.Close<ShopUI>());
    }
}
