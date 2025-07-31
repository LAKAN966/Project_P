using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using TMPro;
using UnityEngine.SceneManagement;

public class TutorialManager : MonoBehaviour
{
    public static TutorialManager Instance { get; private set; }

    [Header("설정")]
    public TutorialData tutorialData;
    public Canvas tutorialCanvas;
    public AssetReferenceGameObject dialogueBoxReference;

    [Header("마스크")]
    public GameObject maskPanel;
    public GameObject blackImage;
    public GameObject blockImage;

    private GameObject dialogueBoxPrefab;
    private DialogPanel dialogPanel;
    private GameObject dialogueBoxInstance;

    private GameObject panel;
    private TMP_Text npcNameText;
    private TMP_Text dialogueText;
    private Button nextButton;

    private CameraController cameraController;
    private int currentStepIndex;

    public bool isPlaying = false;
    private bool hasPlayedNpcIntro = false;

    private void Awake()
    {
        if (Instance != null) { Destroy(gameObject); return; }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void StartTuto()
    {
        var tutorialDeck = new DeckData();
        tutorialDeck.AddNormalUnit(1001);
        tutorialDeck.AddNormalUnit(1002);
        var clonedDeck = DeckManager.Instance.CloneDeck(tutorialDeck);

        PlayerDataManager.Instance.player.currentDeck = clonedDeck;
        SceneManager.LoadScene("MainScene");
        SceneManager.sceneLoaded += OnBattleSceneLoaded;
        SceneManager.LoadScene("BattleScene");
        Debug.Log($"튜토리얼 입장");
        StartCoroutine(InitTutorial());
    }

    private void OnBattleSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "BattleScene")
        {
            var normalDeck = PlayerDataManager.Instance.player.currentDeck.GetAllNormalUnit();
            var leaderDeck = PlayerDataManager.Instance.player.currentDeck.GetLeaderUnitInDeck();
            SceneManager.sceneLoaded -= OnBattleSceneLoaded;
            BattleManager.Instance.StartBattle(110001, normalDeck, leaderDeck);
        }
    }

    private IEnumerator InitTutorial()
    {
        var handle = dialogueBoxReference.LoadAssetAsync();
        yield return handle;
        dialogueBoxPrefab = handle.Result;
        dialogPanel = dialogueBoxPrefab.GetComponent<DialogPanel>();
        AssignCamera();
        StartTutorial();
    }

    private void AssignCamera()
    {
        cameraController = FindObjectOfType<CameraController>();
        Debug.Log(cameraController);
    }

    public void StartTutorial()
    {
        currentStepIndex = 0;
        blackImage.SetActive(true);
        ShowCurrentStep();
    }

    private void ShowCurrentStep()
    {
        var step = tutorialData.steps[currentStepIndex];

        // 🎯 triggerEventName은 effectID가 6이 아닐 때만 적용
        if (!string.IsNullOrEmpty(step.triggerEventName) && step.effectID != 6)
        {
            tutorialCanvas.gameObject.SetActive(false);
            Debug.Log($"[튜토리얼] 트리거 '{step.triggerEventName}' 대기 중...");
            return;
        }

        tutorialCanvas.gameObject.SetActive(true);
        StartCoroutine(PlayStep(step));
    }

    private IEnumerator PlayStep(TutorialStep step)
    {
        if (dialogueBoxInstance != null)
            Destroy(dialogueBoxInstance);

        dialogueBoxInstance = Instantiate(dialogueBoxPrefab, tutorialCanvas.transform);
        panel = dialogueBoxInstance.transform.Find("Panel").gameObject;
        npcNameText = dialogueBoxInstance.transform.Find("Panel/Name/NameText")?.GetComponent<TextMeshProUGUI>();
        dialogueText = dialogueBoxInstance.transform.Find("Panel/Dialog/DialogText")?.GetComponent<TextMeshProUGUI>();
        nextButton = dialogueBoxInstance.transform.Find("NextButton")?.GetComponent<Button>();

        if (step.dialogUp)
            MoveToTopCenter(panel, 200);

        npcNameText.text = step.npcName;
        dialogueText.text = step.dialogue;

        nextButton.onClick.RemoveAllListeners();
        nextButton.onClick.AddListener(NextStep);
        nextButton.interactable = false;

        var dialog = dialogueBoxInstance.GetComponent<DialogPanel>();
        if (dialog != null)
        {
            if (!hasPlayedNpcIntro)
            {
                hasPlayedNpcIntro = true;
                yield return dialog.PlayNpcIntroAnimationWithYield();
            }
            else
            {
                dialog.ShowDialogInstant();
            }
        }

        yield return HandleStepEffects(step.effectID, step.highlightTarget);

        if (step.effectID != 6 && string.IsNullOrEmpty(step.triggerEventName))
        {
            nextButton.interactable = true;
        }
    }

    public void MoveToTopCenter(GameObject uiObject, float offsetY = 0f)
    {
        RectTransform targetRect = uiObject.GetComponent<RectTransform>();
        if (targetRect == null) return;

        targetRect.pivot = new Vector2(0.5f, 1f);
        targetRect.anchorMin = new Vector2(0.5f, 1f);
        targetRect.anchorMax = new Vector2(0.5f, 1f);
        targetRect.anchoredPosition = new Vector2(0f, -offsetY);
    }

    private IEnumerator HandleStepEffects(int effectID, string target)
    {
        maskPanel.SetActive(false);
        blackImage.SetActive(false);
        blockImage.SetActive(false);

        switch (effectID)
        {
            case 0://연출 없음
                blockImage.SetActive(true);
                var cam = FindObjectOfType<BattleCameraController>();
                if (target == "EnemyHealth") cam?.FocusRightMax();
                else if (target == "AllyHealth") cam?.FocusLeftMax();
                break;

            case 1://target 하이라이트
                maskPanel.SetActive(true);
                GameObject button = GameObject.Find(target);
                if (button != null) HighlightUI(button);
                break;

            case 2://배틀신 이동
                SceneManager.LoadScene("BattleScene");
                break;

            case 3://검은 화면
                blackImage.SetActive(true);
                break;

            case 4://배틀 카메라 되돌리기(1번만씀)
                blockImage.SetActive(true);
                FindObjectOfType<BattleCameraController>()?.RestoreCameraState();
                break;

            case 5://메인신 이동
                SceneManager.LoadScene("MainScene");
                break;

            case 6:// 대화창은 그대로, 타겟 이벤트 실행되면 다음으로 넘어감
                //TutorialManager.Instance.OnEventTriggered(이벤트이름);이 실행되면 다음으로 넘어감
                Debug.Log("[튜토리얼] effectID 6: 이벤트 '" + target + "' 대기 중");
                break;
        }

        yield return null;
    }

    public void NextStep()
    {
        var dialogPanel = dialogueBoxInstance?.GetComponent<DialogPanel>();
        if (dialogPanel != null && dialogPanel.IsNpcAnimating())
        {
            dialogPanel.ForceFinishAnimation();
            return;
        }

        currentStepIndex++;
        if (currentStepIndex < tutorialData.steps.Count)
            ShowCurrentStep();
        else
            EndTutorial();
    }

    public void OnEventTriggered(string eventName)
    {
        var step = tutorialData.steps[currentStepIndex];
        if (step.effectID == 6 && step.highlightTarget == eventName)
        {
            Debug.Log($"[튜토리얼] 이벤트 '{eventName}' 수신됨, 다음 단계로");
            NextStep();
        }
        else if (step.triggerEventName == eventName)
        {
            Debug.Log($"[튜토리얼] 트리거 '{eventName}' 수신됨, 다음 단계로");
            NextStep();
        }
    }

    private void EndTutorial()
    {
        if (dialogueBoxInstance != null)
            Destroy(dialogueBoxInstance);
        Debug.Log("튜토리얼 완료");
    }

    public void HighlightUI(GameObject targetUI)
    {
        RectTransform targetRect = targetUI.GetComponent<RectTransform>();
        RectTransform maskRect = maskPanel.GetComponent<RectTransform>();
        Canvas maskCanvas = maskPanel.GetComponentInParent<Canvas>();
        Canvas targetCanvas = targetUI.GetComponentInParent<Canvas>();

        if (targetRect == null || maskRect == null || targetCanvas == null || maskCanvas == null) return;

        Camera targetCamera = targetCanvas.renderMode == RenderMode.ScreenSpaceOverlay ? null : targetCanvas.worldCamera;
        Camera maskCamera = maskCanvas.renderMode == RenderMode.ScreenSpaceOverlay ? null : maskCanvas.worldCamera;

        Vector3[] corners = new Vector3[4];
        targetRect.GetWorldCorners(corners);

        Vector2 screenMin = RectTransformUtility.WorldToScreenPoint(targetCamera, corners[0]);
        Vector2 screenMax = RectTransformUtility.WorldToScreenPoint(targetCamera, corners[2]);
        Vector2 screenCenter = (screenMin + screenMax) * 0.5f;
        Vector2 screenSize = screenMax - screenMin;

        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            maskCanvas.GetComponent<RectTransform>(),
            screenCenter,
            maskCamera,
            out Vector2 localPos
        );

        float margin = 5f;
        maskRect.anchoredPosition = localPos;
        maskRect.sizeDelta = screenSize + new Vector2(margin * 2f, margin * 2f);
    }
}
