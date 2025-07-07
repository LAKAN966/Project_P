using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class WorldParallaxLayer
{
    public string spritePath;
    public float parallaxFactor = 1f;
    public float tileWidth = 10f;
}

public class WorldBG : MonoBehaviour
{
    public float startX = -20f;
    public float endX = 20f;
    public Transform cameraTarget;
    public Transform parentTransform;

    public int stageID = 0; // Stage ID 지정 가능하도록
    public TextAsset stageDataCSV; // Editor에서 연결할 수 있게

    public List<WorldParallaxLayer> layers = new();

    private List<Transform> generatedTiles = new();

    void Awake()
    {
        if (layers.Count == 0)
            LoadBGListFromStage();
    }

    void LoadBGListFromStage()
    {
        if (stageDataCSV == null)
        {
            Debug.LogError("Stage CSV가 할당되지 않았습니다.");
            return;
        }

        var stageData = StageDataLoader.LoadByID(stageDataCSV, stageID);
        if (stageData == null || stageData.BGList == null)
        {
            Debug.LogError("StageData 또는 BGList를 불러올 수 없습니다.");
            return;
        }

        foreach (string path in stageData.BGList)
        {
            layers.Add(new WorldParallaxLayer
            {
                spritePath = path,
                parallaxFactor = 1f, // 필요시 다르게 설정
                tileWidth = 10f
            });
        }
    }

    void Start()
    {
        foreach (var layer in layers)
        {
            Sprite sprite = Resources.Load<Sprite>(layer.spritePath);
            if (sprite == null)
            {
                Debug.LogError($"스프라이트 경로 잘못됨: {layer.spritePath}");
                continue;
            }

            float totalWidth = endX - startX + layer.tileWidth * 2;
            int tileCount = Mathf.CeilToInt(totalWidth / layer.tileWidth) + 2;

            for (int i = 0; i < tileCount; i++)
            {
                float xPos = startX + i * layer.tileWidth;

                GameObject go = new GameObject($"ParallaxTile_{layer.spritePath}_{i}", typeof(SpriteRenderer));
                go.transform.SetParent(parentTransform);
                go.transform.localScale = Vector3.one;
                go.transform.position = new Vector3(xPos, 0f, 0f);

                SpriteRenderer sr = go.GetComponent<SpriteRenderer>();
                sr.sprite = sprite;
                sr.sortingOrder = -Mathf.RoundToInt(layer.parallaxFactor * 100);

                generatedTiles.Add(go.transform);
            }
        }
    }

    void Update()
    {
        foreach (var layer in layers)
        {
            float camX = cameraTarget.position.x * layer.parallaxFactor;

            foreach (Transform tile in generatedTiles)
            {
                if (tile.name.StartsWith($"ParallaxTile_{layer.spritePath}"))
                {
                    float offset = tile.position.x - camX;

                    if (offset < startX - layer.tileWidth)
                        tile.position += Vector3.right * (endX - startX + layer.tileWidth);
                    else if (offset > endX + layer.tileWidth)
                        tile.position -= Vector3.right * (endX - startX + layer.tileWidth);
                }
            }
        }
    }
}
