using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

using Button = UnityEngine.UI.Button;
public class MainUI : UIBase
{
    private UILoader uILoader;
    private UIManager uiManager;

    public Button UnitGotchaBotton;      //가챠    
    public Button UnitManagementButton;  // 유닛 관리 (덱빌딩)
    public Button SacredPlaceButton;     //내실
    public Button StoreButton;           //상점


    public Button SelectStageButton;     // 스테이지

    public void Start()
    {
        //프리펩으로 만들어서  직접 찾아와야함.  무조건 코드로 가져와야함.
        uiManager = FindObjectOfType<UIManager>();
        ButtonPressed();
    }


    public void ButtonPressed()
    {
        UnitGotchaBotton.onClick.AddListener(() => uiManager.Open<GottaUI>());
        UnitManagementButton.onClick.AddListener(() => UIManager.Instance.Open<DeckBuildUI>());
        SacredPlaceButton.onClick.AddListener(() => uiManager.Open<HQUI>());
        StoreButton.onClick.AddListener(() => uiManager.Open<ShopUI>());
        SelectStageButton.onClick.AddListener(() => uiManager.Open<StageUI>());
    }
}
