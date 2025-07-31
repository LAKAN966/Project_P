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

    [Header("Settings")]
    public TutorialData tutorialData;
    public Canvas tutorialCanvas;
    public AssetReferenceGameObject dialogueBoxReference;

    private GameObject dialogueBoxPrefab;
    private DialogPanel dialogPanel;
    private GameObject dialogueBoxInstance;

    private TMP_Text npcNameText;
    private TMP_Text dialogueText;
    private Button nextButton;

    private CameraController cameraController;
    private int currentStepIndex;

    private bool hasPlayedNpcIntro = false;
    private void Awake()
    {
        if (Instance != null) { Destroy(gameObject); return; }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void Start()
    {
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
        ShowCurrentStep();
    }

    private void ShowCurrentStep()
    {
        StartCoroutine(PlayStep(tutorialData.steps[currentStepIndex]));
    }

    private IEnumerator PlayStep(TutorialStep step)
    {
        if (dialogueBoxInstance != null)
            Destroy(dialogueBoxInstance);

        dialogueBoxInstance = Instantiate(dialogueBoxPrefab, tutorialCanvas.transform);

        npcNameText = dialogueBoxInstance.transform.Find("Panel/Name/NameText")?.GetComponent<TextMeshProUGUI>();
        dialogueText = dialogueBoxInstance.transform.Find("Panel/Dialog/DialogText")?.GetComponent<TextMeshProUGUI>();
        nextButton = dialogueBoxInstance.transform.Find("NextButton")?.GetComponent<Button>();

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

        yield return HandleStepEffects(step.effectID);

        if (string.IsNullOrEmpty(step.triggerEventName))
        {
            nextButton.interactable = true;
        }
    }


    private IEnumerator HandleStepEffects(int effectID)
    {
        switch (effectID)
        {
            case 0:
                // 아무 연출 없음
                break;

            case 1:
                GameObject button = GameObject.Find("StoreButton");
                Debug.Log("줌!"+button);
                if (button != null)
                    cameraController?.FocusOn(button.transform, 4f);
                break;

            case 2:
                // 씬 전환 예시
                //yield return new WaitForSeconds(1f);
                //Debug.Log("따잇슝");
                SceneManager.LoadScene("BattleScene");
                break;

            case 3:
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
}
