using UnityEditor;
using UnityEngine;
using System.IO;

public class SPUMExactImageExporter
{
    private const int ImageWidth = 402;
    private const int ImageHeight = 461;
    private const float OrthoSize = 0.58f;
    private const float CameraOffsetY = 0.35f; // ✅ 그림자 기준 위로 더 이동

    [MenuItem("Tools/SPUM/Export Exactly Like In-Game")]
    public static void Export()
    {
        string savePath = "Assets/Resources/SPUMImg";
        Directory.CreateDirectory(savePath);

        Object[] prefabs = Resources.LoadAll("SPUM", typeof(GameObject));

        GameObject camGO = new GameObject("TempRenderCamera");
        Camera cam = camGO.AddComponent<Camera>();
        cam.orthographic = true;
        cam.orthographicSize = OrthoSize;
        cam.clearFlags = CameraClearFlags.SolidColor;
        cam.backgroundColor = new Color(0, 0, 0, 0);
        cam.cullingMask = ~0;
        cam.allowHDR = false;
        cam.enabled = false;

        foreach (var prefab in prefabs)
        {
            GameObject inst = PrefabUtility.InstantiatePrefab(prefab) as GameObject;
            inst.transform.position = Vector3.zero;

            // ✅ Z축 평탄화
            foreach (var t in inst.GetComponentsInChildren<Transform>())
                t.localPosition = new Vector3(t.localPosition.x, t.localPosition.y, 0f);

            // ✅ Shadow 기준 카메라 중심 보정
            var shadow = FindShadowRenderer(inst);
            if (shadow == null)
            {
                Debug.LogWarning($"{prefab.name} → Shadow 못 찾음");
                Object.DestroyImmediate(inst);
                continue;
            }

            Vector3 center = shadow.bounds.center + new Vector3(0f, CameraOffsetY, 0f);
            cam.transform.position = new Vector3(center.x, center.y, -10);

            // ✅ 렌더링
            var rt = new RenderTexture(ImageWidth, ImageHeight, 24, RenderTextureFormat.ARGB32);
            RenderTexture.active = rt;
            cam.targetTexture = rt;
            cam.Render();

            var tex = new Texture2D(ImageWidth, ImageHeight, TextureFormat.RGBA32, false);
            tex.filterMode = FilterMode.Point;
            tex.ReadPixels(new Rect(0, 0, ImageWidth, ImageHeight), 0, 0);
            tex.Apply();

            string fullPath = Path.Combine(Application.dataPath, "Resources/SPUMImg", $"{prefab.name}.png");
            File.WriteAllBytes(fullPath, tex.EncodeToPNG());

            Debug.Log($"✅ 저장 완료: {fullPath}");

            // 정리
            cam.targetTexture = null;
            RenderTexture.active = null;
            Object.DestroyImmediate(rt);
            Object.DestroyImmediate(tex);
            Object.DestroyImmediate(inst);
        }

        Object.DestroyImmediate(camGO);
        AssetDatabase.Refresh();
        Debug.Log("🎉 SPUM 이미지 추출 완료!");
    }

    private static Renderer FindShadowRenderer(GameObject root)
    {
        foreach (var r in root.GetComponentsInChildren<Renderer>(true))
        {
            if (r.name.ToLower().Contains("shadow"))
                return r;
        }
        return null;
    }
}
