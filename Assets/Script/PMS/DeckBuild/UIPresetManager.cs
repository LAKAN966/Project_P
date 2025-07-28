using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIPresetManager : MonoBehaviour
{
    [SerializeField] private GameObject panel;
    [SerializeField] private UIPresetDeck[] presetDecks; // 프리셋 3개

    public static UIPresetManager Instance;

    private void Awake()
    {
        Instance = this;
    }

    public void OpenPanel()
    {
        panel.SetActive(true);
        RefreshUI();
        UIDeckBuildManager.instance.SetMode(DeckMode.Preset);
        UIDeckBuildManager.instance.SetMyUnitIcons();
    }

    public void ClosePanel()
    {
        panel.SetActive(false);
        UIDeckBuildManager.instance.SetMode(DeckMode.DeckBuild);
        UIDeckBuildManager.instance.SetMyUnitIcons();
        foreach (var preset in presetDecks)
        {
            preset.ClearAll();
        }
    }

    public void SavePreset(int index)
    {
        if (DeckManager.Instance.SaveCurrentDeckToPreset(index))
        {
            Debug.Log($"프리셋 {index + 1} 저장 완료");
        }
        else
        {
            Debug.LogWarning($"프리셋 {index + 1} 저장 실패");
        }
    }

    public void RefreshUI()
    {
        var player = PlayerDataManager.Instance.player;

        for (int i = 0; i < presetDecks.Length; i++)
        {
            if (i < player.preset.Count)
                presetDecks[i].SetDeck(player.preset[i]);
            else
                presetDecks[i].ClearAll();
        }
    }

    public void OnClickSavePreset(int index)
    {
        if (index < 0 || index >= presetDecks.Length) return;

        // 임시 덱을 현재 덱에 반영
        PlayerDataManager.Instance.player.currentDeck = presetDecks[index].GetTempDeckData().Clone();

        // DeckManager 통해 프리셋 저장
        if (DeckManager.Instance.SaveCurrentDeckToPreset(index))
        {
            Debug.Log($"프리셋 {index + 1} 저장 완료");
        }
    }

    public void OnClickLoadPresetToCurrentDeck(int index)
    {
        bool success = DeckManager.Instance.SwitchPreset(index);

        if (success)
        {
            // 현재 덱 UI 갱신
            if (UIDeckBuildManager.instance != null)
            {
                UIDeckBuildManager.instance.SetDeckSlots();
            }
            Debug.Log($"프리셋 {index + 1}을 현재 덱으로 불러왔습니다.");
        }
        else
        {
            Debug.LogWarning($"프리셋 {index + 1} 불러오기 실패");
        }
    }
}
