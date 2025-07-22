public class HeroUnit : Unit
{
    public override void Initialize()
    {
        base.Initialize();
        SkillManager.Instance.UseSkill(stats.SkillID[1], this.isEnemy);
    }
}
