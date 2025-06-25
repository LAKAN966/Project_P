using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using System;

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

    private string GetUIName<T>() where T : UIBase
    {
        return typeof(T).Name;
    }

    public T Open<T>() where T : UIBase//  타입 제한자
    {
        string uiName = GetUIName<T>();

        if (_uiList.ContainsKey(uiName) == false)
        {
            T prefab = Resources.Load<T>($"UI/{uiName}"); // resources / Ui / uiName을 불러옴

            if (prefab == null)// 방어코드
            {
                throw new System.Exception($"{uiName} not found.");
                //Debug.LogError($"{uiName} not found.");// 빨간줄로 뜨게 하는법
                //return;
            }

            T uiobject = Instantiate(prefab); //하이어라키에 복사해서 추가

            _uiList.Add(uiName, uiobject);// 딕셔너리에 생성한 ui 추가

            UIBase ui = uiobject.GetComponent<UIBase>();

            if (ui == null)
            {
                throw new System.Exception($"{uiName}Ui 프리팹에 Ui 컴포넌트가 없습니다");
                //Debug.LogError($"{uiName}Ui 프리팹에 Ui 컴포넌트가 없습니다");
                //return;
            }
        }

        UIBase spawnedUI = _uiList[uiName]; //키값은 변수로 할당.
        spawnedUI.Open();
        return spawnedUI as T;
    }

    public void Close<T>(bool kill = false) where T : UIBase
    {
        string uiName = GetUIName<T>();

        UIBase ui /*= _uiList[uiName]*/  ; //  (리스트에서 불러옴)

        if (_uiList.TryGetValue(uiName, out UIBase SavedUI))//  존재여부 확인을 위해 TryGetValue 사용, Out에 값을 넣어줌.
            ui = SavedUI;
        else return;

        if (_uiList.ContainsKey(uiName) == false)
        {
            Debug.Log($"{uiName}이 없습니다");
            return;
        }

        SavedUI.Close(); // 찾은 UI를 꺼준다.
        Destroy(ui.gameObject);
        _uiList.Remove(uiName);
    }
}