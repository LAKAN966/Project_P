using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Button = UnityEngine.UI.Button;


public class ShopUI : UIBase
{

    private UILoader uILoader;
    private UIManager uiManager;

    public Button ExitBtn;

    public void Start()
    {
        //프리펩으로 만들어서  직접 찾아와야함.  무조건 코드로 가져와야함.
        uiManager = FindObjectOfType<UIManager>();
        ExitBtn.onClick.AddListener(() => UIManager.Instance.Close<ShopUI>());
        // 나머지 창은 비활성화 메인제외,  가독성 측면에서 안좋을때 나누기.
        ExitBtn.onClick.AddListener(() => UIManager.Instance.Close<HQUI>());
        ExitBtn.onClick.AddListener(() => UIManager.Instance.Close<StageUI>());
        ExitBtn.onClick.AddListener(() => UIManager.Instance.Close<GottaUI>());
    }
}
