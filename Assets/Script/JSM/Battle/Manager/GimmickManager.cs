using System.Collections.Generic;
using UnityEngine;
public class GimmickData
{
    public int ID;
    public string Name;
    public float EffectValue;

    public GimmickData(int id, string name, float value)
    {
        ID = id;
        Name = name;
        EffectValue = value;
    }
}

public class GimmickManager : MonoBehaviour
{
    public static GimmickManager Instance;

    // 기믹 데이터 저장용
    private Dictionary<int, GimmickData> gimmickDict = new();

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
        LoadGimmickData();
    }

    private void LoadGimmickData()
    {
        TextAsset csvFile = Resources.Load<TextAsset>("Data/GimmickData");
        if (csvFile == null)
        {
            Debug.LogError("GimmickData.csv 파일이 Resources/Data에 없습니다.");
            return;
        }

        string[] lines = csvFile.text.Split(new[] { '\n', '\r' }, System.StringSplitOptions.RemoveEmptyEntries);

        for (int i = 1; i < lines.Length; i++) // 첫 줄은 헤더
        {
            string line = lines[i].Trim();
            if (string.IsNullOrWhiteSpace(line)) continue;

            string[] tokens = line.Split(',');

            int id = int.Parse(tokens[0]);
            string name = tokens[1];
            float effectValue = 0;

            if (tokens.Length > 2 && !string.IsNullOrWhiteSpace(tokens[2]))
                float.TryParse(tokens[2], out effectValue);

            GimmickData data = new(id, name, effectValue);
            gimmickDict[id] = data;
        }

        Debug.Log($"[GimmickManager] 기믹 데이터 {gimmickDict.Count}개 로드 완료");
    }
    public void ApplyGimmick(int id)
    {
        if (!gimmickDict.TryGetValue(id, out var data))
        {
            Debug.LogWarning($"[GimmickManager] ID {id} 기믹을 찾을 수 없습니다.");
            return;
        }
        switch (data.Name)
        {
            case "TimeLimit": SetTimeLimit(data.EffectValue); break;
            case "UnitResummonCooldownUp": SetUnitResummonCooldownUp(data.EffectValue); break;
            case "UnitSummonCostUp": SetUnitSummonCostUp(data.EffectValue); break;
            case "LeaderUnitSummonBan": SetLeaderUnitSummonBan(); break;
        }
    }
    public void SetTimeLimit(float value)
    {
        WaveManager.Instance.Timer.SetActive(true);
        GameManager.Instance.timer.timeLimit=value;
    }
    public void SetUnitResummonCooldownUp(float value)
    {
        //SpawnButton에서 관리
    }
    public void SetUnitSummonCostUp(float value)
    {
        //SpawnButton에서 관리
    }
    public void SetLeaderUnitSummonBan()
    {

    }
}
