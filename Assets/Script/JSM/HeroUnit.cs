using UnityEngine;
public class HeroUnit : Unit
{
    public override void Initialize()
    {
        base.Initialize();
        SkillManager.Instance.UseHeroSkill(stats.ActiveSkillID, this);
    }
}
