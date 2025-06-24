using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    private static UIManager _instance;

    public static UIManager Instance
    {
        get
        {
            if (_instance == null)
            {
                //_instance 가 널이면 라이어라키 게임오브젝트 중에 Ui매니저를 찾아 할당.
                _instance = FindObjectOfType<UIManager>();
            }

            //없다면 새로 만듬.
            if (_instance == null)
            {
                GameObject go = new GameObject();
                go.name = "UIManager";
                _instance = go.AddComponent<UIManager>();
            }
            return _instance;
        }
    }

    Dictionary<string, UIBase> _uiList = new();




    public void Open(string uiName)
    {
        GameObject prefab = Resources.Load<GameObject>($"UI/{uiName}");

        if (prefab == null)// 방어코드
        {
            Debug.LogError($"{uiName} not found.");// 빨간줄로 뜨게 하는법
            return;
        }

        GameObject uiobject = Instantiate(prefab);

        UIBase ui = uiobject.GetComponent<UIBase>();

        if (ui == null)
        {
            Debug.LogError($"{uiName}Ui 프리팹에 Ui 컴포넌트가 없습니다");
            return;
        }

        _uiList.Add(uiName, ui);

    }

    public void Close(string uiName)
    {
        if (_uiList.ContainsKey(uiName) == false)
        {
            Debug.Log($"{uiName}이 없습니다");
            return;
        }

        UIBase ui = _uiList[uiName]; //  리스트에서 불러옴

        Destroy(ui.gameObject);
        _uiList.Remove(uiName);
    }

}