using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;


public class UnitCardSlot : MonoBehaviour
{
    [SerializeField] private TMP_Text UnitICardNametext;        // 유닛 이름

    [SerializeField] public Image UnitIcon;                     // 유닛 아이콘
    public int shopPrice;
    public GameObject duplicate;
    public TextMeshProUGUI dupTxt;
    private Coroutine blinkCoroutine;

    public void init(PickInfo Alliance) //4
    {
        UnitICardNametext.text = Alliance.Name;

        var stats = UnitDataManager.Instance.GetStats(Alliance.ID);
        shopPrice = Alliance.duplication;
        Debug.Log(stats.ModelName);

        UnitIcon.sprite = Resources.Load<Sprite>($"SPUMImg/{stats.ModelName}"); //5

        if (!PlayerDataManager.Instance.AddUnit(Alliance.ID))
        {
            duplicate.SetActive(true);
            dupTxt.text = Alliance.duplication.ToString();
            if (blinkCoroutine != null) StopCoroutine(blinkCoroutine);
            blinkCoroutine = StartCoroutine(BlinkObject(duplicate));
        }
    }
    private IEnumerator BlinkObject(GameObject obj)
    {
        CanvasGroup group = obj.GetComponent<CanvasGroup>();
        if (group == null) group = obj.AddComponent<CanvasGroup>();

        CanvasGroup iconGroup = UnitIcon.GetComponent<CanvasGroup>();
        if (iconGroup == null) iconGroup = UnitIcon.gameObject.AddComponent<CanvasGroup>();

        float duration = 1f;

        while (true)
        {
            // Fade In (obj), Fade Out (icon)
            for (float t = 0; t < duration; t += Time.deltaTime)
            {
                float lerpT = t / duration;
                group.alpha = Mathf.Lerp(0f, 1f, lerpT);
                iconGroup.alpha = Mathf.Lerp(1f, 0f, lerpT);
                yield return null;
            }
            group.alpha = 1f;
            iconGroup.alpha = 0f;

            yield return new WaitForSeconds(0.5f);

            // Fade Out (obj), Fade In (icon)
            for (float t = 0; t < duration; t += Time.deltaTime)
            {
                float lerpT = t / duration;
                group.alpha = Mathf.Lerp(1f, 0f, lerpT);
                iconGroup.alpha = Mathf.Lerp(0f, 1f, lerpT);
                yield return null;
            }
            group.alpha = 0f;
            iconGroup.alpha = 1f;

            yield return new WaitForSeconds(0.5f);
        }
    }

}