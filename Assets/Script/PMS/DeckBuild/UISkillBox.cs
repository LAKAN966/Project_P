using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UISkillBox : MonoBehaviour
{
    [SerializeField] private Image activeIcon;
    [SerializeField] private Image passiveIcon;

    [SerializeField] private Sprite activeSprite;
    [SerializeField] private Sprite passiveSprite;

    [SerializeField] private Button activeBtn;
    [SerializeField] private Button passiveBtn;
    [SerializeField] private UISkillInfo skillInfo;

    public void OnEnable()
    {
        activeIcon.sprite = activeSprite;
        passiveIcon.sprite = passiveSprite;
        
    }

    public void SetSkill(UnitStats data)
    {

    }
}
