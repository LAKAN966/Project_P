using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BaseCastle : MonoBehaviour
{
    public bool isEnemy; // true면 적, false면 아군

    public int maxHP = 1000;
    private int currentHP;

    public TMP_Text hpText;
    public Image hpGauge;

    public Collider2D hitCollider; // 충돌용 Collider (예: BoxCollider2D)

    private bool isDestroyed = false;

    private void Start()
    {
        currentHP = maxHP;
        UpdateUI();
    }

    public void TakeDamage(int amount)
    {
        if (isDestroyed) return;

        currentHP -= amount;
        if (currentHP <= 0)
        {
            currentHP = 0;
            OnDestroyed();
        }

        UpdateUI();
    }

    void UpdateUI()
    {
        if (hpText != null)
            hpText.text = $"{currentHP} / {maxHP}";

        if (hpGauge != null)
            hpGauge.fillAmount = (float)currentHP / maxHP;
    }

    void OnDestroyed()
    {
        isDestroyed = true;

        // 유닛 통과 가능하게 충돌 제거
        if (hitCollider != null)
            hitCollider.enabled = false;

        // 게임 종료 처리
        //GameManager.Instance.OnBaseDestroyed(isEnemy);
        Debug.Log(isEnemy ? "완전 승리!!" : "패배..");
    }
}
