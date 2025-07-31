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

    private void OnEnable()
    {
        //SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        //SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    public void StartTuto()
    {
        SceneManager.LoadScene("BattleScene");
        StartCoroutine(InitTutorial());
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

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        AssignCamera();

        if (scene.name == "BattleScene")
        {
            StartCoroutine(HandleBattleSceneLoaded());
        }
    }

    private IEnumerator HandleBattleSceneLoaded()
    {
        yield return new WaitForSeconds(2f);
        Time.timeScale = 0f;
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

        if (!string.IsNullOrEmpty(step.triggerEventName))
        {
            tutorialCanvas.gameObject.SetActive(false);
            // 트리거가 필요한 경우: 대기만 하고 대화창 생성 안함
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
        {
            MoveToTopCenter(panel,200);
        }

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
                yield return dialog.PlayNpcIntroAnimationWithYield(); // 애니메이션 끝날 때까지 대기
            }
            else
            {
                dialog.ShowDialogInstant(); // 즉시 보여줌
            }
        }

        yield return HandleStepEffects(step.effectID,step.highlightTarget);

        if (string.IsNullOrEmpty(step.triggerEventName))
        {
            nextButton.interactable = true;
        }
    }

    public void MoveToTopCenter(GameObject uiObject, float offsetY = 0f)
    {
        RectTransform targetRect = uiObject.GetComponent<RectTransform>();
        if (targetRect == null)
        {
            Debug.LogWarning("해당 오브젝트에 RectTransform이 없습니다.");
            return;
        }

        // 1. 피벗과 앵커를 화면 위 중앙으로 설정
        targetRect.pivot = new Vector2(0.5f, 1f);
        targetRect.anchorMin = new Vector2(0.5f, 1f);
        targetRect.anchorMax = new Vector2(0.5f, 1f);

        // 2. 위치 설정 (offsetY 만큼 아래로 내리기 가능)
        targetRect.anchoredPosition = new Vector2(0f, -offsetY);
    }


    private IEnumerator HandleStepEffects(int effectID, string target)
    {
        maskPanel.SetActive(false);
        blackImage.SetActive(false);
        blockImage.SetActive(false);
        GameObject button;
        switch (effectID)
        {
            case 0:
                // 아무 연출 없음
                if (target == "EnemyHealth")
                {
                    BattleCameraController camController = FindObjectOfType<BattleCameraController>();
                    if (camController != null)
                    {
                        camController.FocusRightMax();
                    }
                }
                if (target == "AllyHealth")
                {
                    BattleCameraController camController = FindObjectOfType<BattleCameraController>();
                    if (camController != null)
                    {
                        camController.FocusLeftMax();
                    }
                }
                break;
            case 1:
                maskPanel.SetActive(true);
                button = GameObject.Find(target);
                if (button != null)
                {
                    HighlightUI(button);
                    Debug.Log("하이라이트 연출");
                }
                
                break;
            case 2:
                // 씬 전환
                SceneManager.LoadScene("BattleScene");
                break;

            case 3:
                blackImage.SetActive(true);
                break;
            case 4:
                blockImage.SetActive(true);
                break;
            case 5:
                blockImage.SetActive(true);
                FindObjectOfType<BattleCameraController>().RestoreCameraState();
                break;

        }

        yield return null;
    }


    public void NextStep()
    {
        Debug.Log("실행!");
        var dialogPanel = dialogueBoxInstance?.GetComponent<DialogPanel>();
        if (dialogPanel != null)
        {
            if (dialogPanel.IsNpcAnimating())
            {
                dialogPanel.ForceFinishAnimation();
                return;
            }
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
        if (step.triggerEventName == eventName)
            NextStep();
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

        if (targetRect == null || maskRect == null || targetCanvas == null || maskCanvas == null)
        {
            Debug.LogWarning("RectTransform 또는 Canvas 누락");
            return;
        }

        Camera targetCamera = targetCanvas.renderMode == RenderMode.ScreenSpaceOverlay ? null : targetCanvas.worldCamera;
        Camera maskCamera = maskCanvas.renderMode == RenderMode.ScreenSpaceOverlay ? null : maskCanvas.worldCamera;

        // 📍 1. 타겟의 월드 코너 → 스크린 좌표로 변환
        Vector3[] corners = new Vector3[4];
        targetRect.GetWorldCorners(corners); // 0: 좌하, 2: 우상

        Vector2 screenMin = RectTransformUtility.WorldToScreenPoint(targetCamera, corners[0]);
        Vector2 screenMax = RectTransformUtility.WorldToScreenPoint(targetCamera, corners[2]);

        Vector2 screenCenter = (screenMin + screenMax) * 0.5f;
        Vector2 screenSize = screenMax - screenMin;

        // 📍 2. 마스크 캔버스 기준 로컬 좌표로 변환 (화면 기준으로 위치 계산)
        Vector2 localPos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            maskCanvas.GetComponent<RectTransform>(),
            screenCenter,
            maskCamera,
            out localPos
        );

        // 📍 3. 적용
        float margin = 5f;
        maskRect.anchoredPosition = localPos;
        maskRect.sizeDelta = screenSize + new Vector2(margin * 2f, margin * 2f);
    }






}
