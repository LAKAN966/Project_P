using UnityEngine;
using UnityEditor;

public class DebugWindow : EditorWindow
{
    [MenuItem("Window/DebugWindow")]
    public static void ShowWindow()
    {
        // 창을 띄운다
        GetWindow<DebugWindow>("MyDebugWindow");
    }
    static int StageValue = 0;
    private void OnGUI()
    {
        GUILayout.Label("이건 커스텀 에디터 창입니다!", EditorStyles.boldLabel);

        GUILayout.Label("정수 값 입력", EditorStyles.boldLabel);


        if (GUILayout.Button("테스트 버튼"))
        {
            Test.Instance.EditFunctionSetUnit();
        }

        if (GUILayout.Button("덱 추가 버튼"))
        {
            Test.Instance.EditFunctionSetDeck();
        }
        
        if (GUILayout.Button("덱 데이터 출력 버튼"))
        {
            Test.Instance.EditFuctionDeckData();
        }

    }
}
