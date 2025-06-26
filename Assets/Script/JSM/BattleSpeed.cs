using UnityEngine;
using TMPro;

public class BattleSpeed : MonoBehaviour
{
    public TextMeshProUGUI speedLabel;

    private bool isDoubleSpeed = false;

    public void ToggleSpeed()
    {
        isDoubleSpeed = !isDoubleSpeed;
        Time.timeScale = isDoubleSpeed ? 2f : 1f;

        if (speedLabel != null)
            speedLabel.text = isDoubleSpeed ? "X1" : "X2";
    }
}
